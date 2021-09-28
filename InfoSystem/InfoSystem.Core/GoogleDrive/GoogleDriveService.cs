using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using InfoSystem.Core.DataAbstraction;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography.X509Certificates;
//using Google.Apis.Drive.v2;

namespace InfoSystem.Core.GoogleDrive
{
    public class GoogleDriveService
    {
        public DriveService DriveService;

        private static readonly string[] _scopes = { DriveService.Scope.DriveFile, DriveService.Scope.Drive };
        private static readonly string _applicationName = "InfoSystem";
        private static readonly string _credPath = "token.json";
        private readonly ILogger<GoogleDriveService> _logger;
        private readonly GoogleDiscCredentials _googleDiscCredentials;
        private readonly IDriveRepository _driveRepository;
        private readonly IUserService _userService;
        private readonly ICompetitionSeasonRepository _competitionSeasonRepository;

        public GoogleDriveService(IOptions<GoogleDiscCredentials> googleDiscCredentials
            , IDriveRepository driveRepository
            , IUserService userService,
            ICompetitionSeasonRepository competitionSeasonRepository
            , ILogger<GoogleDriveService> logger)
        {
            _logger = logger;
            _logger.LogInformation("GoogleDriveService initialize started");
            try
            {
                _googleDiscCredentials = googleDiscCredentials.Value;
                DriveService = AuthenticateServiceAccount();
                _driveRepository = driveRepository;
                this._userService = userService;
                this._competitionSeasonRepository = competitionSeasonRepository;
                _logger.LogInformation("GoogleDriveService initialize done");
            }
            catch (Exception e)
            {
                _logger.LogError("GoogleDriveService initialize", e.Message);
            }

        }

        private DriveService AuthenticateServiceAccount()
        {
            if (!System.IO.File.Exists(_googleDiscCredentials.KeyFilePath))
            {
                _logger.LogInformation("Google disc credentials file does not exist");
                throw new Exception("Google disc credentials file does not exist");
            }
            string[] scopes = new string[] { DriveService.Scope.Drive };
            var cerfiticate = new X509Certificate2(_googleDiscCredentials.KeyFilePath, "notasecret", X509KeyStorageFlags.Exportable);
            try
            {
                ServiceAccountCredential credential = new ServiceAccountCredential(
                    new ServiceAccountCredential.Initializer(_googleDiscCredentials.ServiceAccountEmail)
                    {
                        Scopes = scopes
                    }.FromCertificate(cerfiticate));

                DriveService service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = _applicationName
                });

                return service;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "DriveService initialize was not successfull");
                return null;
            }
        }

        public List<GoogleDriveFile> GetListFiles(int pageSize = 10)
        {
            FilesResource.ListRequest listRequest = DriveService.Files.List();
            listRequest.PageSize = pageSize;
            listRequest.Fields = "nextPageToken, files(id, name)";

            var files = listRequest.Execute()
                 .Files;
            var list = files.Select(f => new GoogleDriveFile()
            {
                CreatedTime = f.CreatedTime,
                Id = f.Id,
                Name = f.Name,
                Size = f.Size,
                Version = f.Version
            }).ToList();
            return list;
        }

        public GoogleDriveFile GetRoot()
        {
            FilesResource.ListRequest listRequest = DriveService.Files.List();
            listRequest.PageSize = 10;
            listRequest.Fields = "nextPageToken, files(id, name)";

            var files = listRequest.Execute()
                 .Files;
            var rootFolder = files.Where(f => f.Name == "Akreditace")
                .Select(f => new GoogleDriveFile()
                {
                    CreatedTime = f.CreatedTime,
                    Id = f.Id,
                    Name = f.Name,
                    Size = f.Size,
                    Version = f.Version
                }).SingleOrDefault();
            return rootFolder;
        }



        public async Task RedactorReportUpload(IFormFile formFile, int competitionSeasonId, string userId)
        {
            _logger.LogInformation("Redactor report upload started");
            var userFolder = await _driveRepository.GetUserFolderId(userId);
            if (userFolder.FolderId == null)
            {
                var rootFolderId = GetRoot().Id;
                var newFolder = await CreateFolder(userFolder.Name, rootFolderId);

                await _userService.UpdateGoogleDriveDirection(userId, newFolder.Id);
                userFolder.FolderId = newFolder.Id;
            }

            var competitionSeasonFolder = await GetFolder(competitionSeasonId.ToString(), userFolder.FolderId);
            if (competitionSeasonFolder == null)
            {
                competitionSeasonFolder = await CreateFolder(competitionSeasonId.ToString(), userFolder.FolderId);
            }

            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = Path.GetFileName(formFile.FileName)
            };
            fileMetadata.Parents = new List<string>() { competitionSeasonFolder.Id };
            FilesResource.CreateMediaUpload request;
            using (var stream = formFile.OpenReadStream())
            {
                request = DriveService.Files.Create(
                    fileMetadata, stream, formFile.ContentType);
                request.Fields = "id";
                var progress = await request.UploadAsync();
            }
            var file = request.ResponseBody;
            var redactorReport = new RedactorReportUpload()
            {
                UserId = userId,
                CompetitionSeasonId = competitionSeasonId,
                FileId = file.Id
            };
            await _driveRepository.SaveRedactorReportData(redactorReport);
            _logger.LogInformation("Redactor report upload - successfully done");
        }

        public async Task<List<GoogleFileView>> RedactorReport(int competitionSeasonId, string userId)
        {
            var userFolder = await _driveRepository.GetUserFolderId(userId);
            if (userFolder == null)
            {
                //TODO
                //  _driveRepository.
                return null;
            }
            var competitionSeasonFolder = await GetFolder(competitionSeasonId.ToString(), userFolder.FolderId);
            if (competitionSeasonFolder == null) return null;

            FilesResource.ListRequest listRequest = DriveService.Files.List();
            //listRequest.Fields = "nextPageToken, files(id, name, IconLink, WebViewLink)";
            listRequest.Q = $"'{competitionSeasonFolder.Id}' in parents";
            var files = listRequest.Execute()
                 .Files;
            return files.Select(f => new GoogleFileView()
            {
                Id = f.Id,
                Name = f.Name,
                IconLink = f.IconLink,
                WebViewLink = f.WebViewLink
            }).ToList();
        }

        public async Task CompetitionSeasonPaparaziUpload(int competitionSeasonid, IFormFile formFile)
        {
            var folder = await _driveRepository.GetCompetitionSeasonFolderById(competitionSeasonid);

            if (folder.FolderId == null)
            {
                var folderMetadata = new Google.Apis.Drive.v3.Data.File();
                folderMetadata.Name = folder.FolderId;
                folderMetadata.MimeType = "application/vnd.google-apps.folder";

                var requestFolder = DriveService.Files.Create(folderMetadata);
                requestFolder.Fields = "id";
                var folderRequest = await requestFolder.ExecuteAsync();
                await _competitionSeasonRepository.UpdateGoogleDriveDirection(competitionSeasonid, folderRequest.Id);
                folder.FolderId = folderRequest.Id;
            }

            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = Path.GetFileName(formFile.FileName)
            };
            fileMetadata.Parents.Add(folder.FolderId);
            FilesResource.CreateMediaUpload request;
            using (var stream = formFile.OpenReadStream())
            {
                request = DriveService.Files.Create(
                    fileMetadata, stream, formFile.ContentType);
                request.Fields = "id";
                var progress = await request.UploadAsync();
            }
            var file = request.ResponseBody;
        }

        public async Task<List<GoogleFileView>> GetCompetitionSeasonMediaList(int competitionSeasonid)
        {
            var folder = await _driveRepository.GetCompetitionSeasonFolderById(competitionSeasonid);
            FilesResource.ListRequest listRequest = DriveService.Files.List();
            listRequest.Fields = "nextPageToken, files(id, name, IconLink, WebViewLink)";
            listRequest.Q = $"'{folder.FolderId}' in parents";
            var files = listRequest.Execute()
                 .Files;
            return files.Select(f => new GoogleFileView()
            {
                Id = f.Id,
                Name = f.Name,
                IconLink = f.IconLink,
                WebViewLink = f.WebViewLink
            }).ToList();
        }

        private async Task<Google.Apis.Drive.v3.Data.File> CreateFolder(string folderName, string parentFolderId)
        {
            var folderMetadata = new Google.Apis.Drive.v3.Data.File();
            folderMetadata.Name = folderName;
            folderMetadata.MimeType = "application/vnd.google-apps.folder";
            folderMetadata.Parents = new List<string>() { parentFolderId };
            var requestFolder = DriveService.Files.Create(folderMetadata);
            requestFolder.Fields = "id";
            var folderRequest = await requestFolder.ExecuteAsync();
            return folderRequest;
        }

        private async Task<Google.Apis.Drive.v3.Data.File> GetFile(string fileId)
        {
            try
            {
                FilesResource.ListRequest listRequest = DriveService.Files.List();
                listRequest.PageSize = 30;
                listRequest.Fields = "nextPageToken, files(id, name)";

                IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute()
                .Files;

                var file = await DriveService.Files.Get(fileId).ExecuteAsync();
                return file;
            }
            catch (IOException e)
            {
                _logger.LogError(e, "Getting file excepted.");
                return null;
            }
        }
        private async Task<IList<Google.Apis.Drive.v3.Data.File>> GetFiles(string parrentFolderId)
        {
            try
            {
                var oListReq = DriveService.Files.List();
                oListReq.Q = $"mimeType = 'application/vnd.google-apps.folder' and '{parrentFolderId}' in parents and trashed=false";
                oListReq.Fields = "items(id,alternateLink)";
                var res = await oListReq.ExecuteAsync();
                return res.Files;
            }
            catch (IOException e)
            {
                _logger.LogError(e, "Getting file excepted.");
                return null;
            }
        }

        private async Task<Google.Apis.Drive.v3.Data.File> GetFolder(string folderName,string parentFolderId)
        {
            try
            {
                var oListReq = DriveService.Files.List();
                oListReq.Q = $"mimeType = 'application/vnd.google-apps.folder' and name = '{folderName}' and '{parentFolderId}' in parents";// 
               // oListReq.Fields = "items(id,alternateLink)";
                var res = await oListReq.ExecuteAsync();
                return res.Files.FirstOrDefault();
            }
            catch (IOException e)
            {
                _logger.LogError(e, $"Folder was not found. Name:{folderName}, ParentFolderID:{parentFolderId}");
                return null;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Folder was not found. Name:{folderName}, ParentFolderID:{parentFolderId}");
                return null;
            }
        }

        private async Task<IList<Google.Apis.Drive.v3.Data.File>> GetFiles(Google.Apis.Drive.v3.Data.File parrentFolder)
        {
           return await GetFiles(parrentFolder.Id);
        }
    }
}

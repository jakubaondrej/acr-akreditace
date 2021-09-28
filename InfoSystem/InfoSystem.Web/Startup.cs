using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using InfoSystem.Core.DataAbstraction;
using InfoSystem.Data.Repositories;
using InfoSystem.Core.Users;
using InfoSystem.Core.Redactions;
using InfoSystem.Core.UserAccessRequests;
using InfoSystem.Core.Sports;
using InfoSystem.Core.Seasons;
using InfoSystem.Core.Championships;
using InfoSystem.Core.Competitions;
using InfoSystem.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using InfoSystem.Core.CompetitionSeasons;
using InfoSystem.Core.Accreditations;
using InfoSystem.Core.Emails;
using InfoSystem.Core.GoogleDrive;
using Microsoft.Extensions.Logging;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using InfoSystem.Web.Data;
using InfoSystem.Core.RedactorViewPaparaziMedia;

namespace InfoSystem.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            HostingEnvironment = env;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddCors();
            if (HostingEnvironment.IsProduction())
            {

                using (System.IO.StreamWriter file = new System.IO.StreamWriter("Info.txt", true))
                {
                    file.WriteLine($"{DateTime.Now.ToString()}: connection string to Database = '{Configuration.GetConnectionString("Mediadeleg")}'");
                }
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(
                        Configuration.GetConnectionString("Mediadelegat")));
                services.AddScoped<ApplicationDbContext>(s =>
                {
                    var options = new DbContextOptionsBuilder();
                    options.UseSqlServer(
                        Configuration.GetConnectionString("Mediadelegat"));
                    return new ApplicationDbContext(options.Options);
                });
                using (System.IO.StreamWriter file = new System.IO.StreamWriter("Info.txt", true))
                {
                    file.WriteLine($"{DateTime.Now.ToString()}: Mediadeleg database in use");
                }

            }
            else
            {
                var dbConnection = Configuration.GetConnectionString("Old");
                using (System.IO.StreamWriter file = new System.IO.StreamWriter("Info.txt", true))
                {
                    file.WriteLine($"{DateTime.Now.ToString()}: connection string to Database = '{dbConnection}'");
                }
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(dbConnection));
                services.AddScoped<ApplicationDbContext>(s =>
                {
                    var options = new DbContextOptionsBuilder();
                    options.UseSqlServer(dbConnection);
                    return new ApplicationDbContext(options.Options);
                });
                using (System.IO.StreamWriter file = new System.IO.StreamWriter("Info.txt", true))
                {
                    file.WriteLine($"{DateTime.Now.ToString()}: Mediadeleg database in use");
                }
            }



            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddScoped<IUserService, UserRepository>();
            services.AddScoped<UserService>();
            services.AddScoped<IRedactionRepository, RedactionRepository>();
            services.AddScoped<RedactionService>();
            services.AddScoped<ISportRepository, SportRepository>();
            services.AddScoped<SportService>();
            services.AddScoped<ISeasonRepository, SeasonRepository>();
            services.AddScoped<SeasonService>();
            services.AddScoped<IChampionshipRepository, ChampionshipRepository>();
            services.AddScoped<ChampionshipService>();
            services.AddScoped<ICompetitionRepository, CompetitionRepository>();
            services.AddScoped<CompetitionService>();
            services.AddScoped<ICompetitionSeasonRepository, CompetitionSeasonRepository>();
            services.AddScoped<CompetitionSeasonService>();
            services.AddScoped<IUserAccessRequestRepository, UserAccessRequestRepository>();
            services.AddScoped<UserAccessRequestService>();
            services.AddScoped<IAccreditationRepository, AccreditationRepository>();
            services.AddScoped<AccreditationService>();
            services.AddScoped<IRedactorViewPaparaziMediaRepository, RedactorViewPaparaziMediaRepository>();
            services.AddScoped<RedactorViewPaparaziMediaService>();
            services.AddScoped<IDriveRepository, DriveRepository>();
            services.AddScoped<GoogleDriveService>();

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
            });

            //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddEntityFrameworkStores<ApplicationDbContext>();

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(500);

                options.LoginPath = "/User/Login";
                options.AccessDeniedPath = "/User/AccessDenied";
                options.SlidingExpiration = true;
            });

            services.AddMvc(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                .RequireAuthenticatedUser()
                                .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            }).AddXmlSerializerFormatters();

            //Email config
            var emailSettingSection = Configuration.GetSection("EmailSettings");
            services.Configure<EmailSettings>(emailSettingSection);
            services.AddScoped<EmailService>();

            //Google Drive config
            var googleDiscCredentials = Configuration.GetSection("GoogleDiscCredentials");
            services.Configure<GoogleDiscCredentials>(googleDiscCredentials);
            services.AddScoped<GoogleDriveService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IServiceProvider serviceProvider, ILogger<Startup> logger)
        {
            logger.LogInformation($"Start application - Environment = {HostingEnvironment.EnvironmentName}; Production = {HostingEnvironment.IsProduction()}");
            if (HostingEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                UpdateDatabase(app);
            }
            else
            {

                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();

                UpdateDatabase(app);
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            logger.LogInformation("CreateRoles Started");
            try
            {
                CreateRoles(serviceProvider).Wait();
                logger.LogInformation("CreateRoles Done");
            }
            catch (Exception e)
            {
                logger.LogError(e, $"CreateRoles Failed - {e.Message}");
            }
        }

        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<User>>();
            IdentityResult roleResult;

            foreach (var roleName in Roles.GetAll())
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var poweruser = new User
            {
                UserName = "admin",
                Email = "admin@admin.ad",
            };
            string userPWD = "Admin11";
            var _user = await UserManager.FindByEmailAsync(poweruser.Email);

            if (_user == null)
            {
                var createPowerUser = await UserManager.CreateAsync(poweruser, userPWD);
                if (createPowerUser.Succeeded)
                {
                    await UserManager.AddToRoleAsync(poweruser, Roles.Admin);
                }
            }
        }

        private static void UpdateDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>())
                {
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter("Info.txt", true))
                    {
                        file.WriteLine($"{DateTime.Now.ToString()}: Update database..");
                    }
                    try
                    {
                        context.Database.Migrate();
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter("Info.txt", true))
                        {
                            file.WriteLine($"{DateTime.Now.ToString()}: Update database Successfully DONE");
                        }
                    }
                    catch (Exception e)
                    {
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter("Errors.txt", true))
                        {
                            file.WriteLine($"{DateTime.Now.ToString()}: Update database failed - {e.Message}");
                        }
                    }

                }
            }
        }
    }
}

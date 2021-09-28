using System;
using System.Collections.Generic;
using System.Text;
using InfoSystem.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InfoSystem.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<Story> Story { get; set; }
        public DbSet<Driver> Driver { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserAccessRequest> UserAccessRequest { get; set; }
        public DbSet<Redaction> Redaction { get; set; }
        public DbSet<Sport> Sport { get; set; }
        public DbSet<Championship> Championships { get; set; }
        public DbSet<Season> Season { get; set; }
        public DbSet<Competition> Competition { get; set; }
        public DbSet<CompetitionSeason> CompetitionSeason { get; set; }
        public DbSet<Accreditation> Accreditation { get; set; }
        public DbSet<RedactorReport> RedactorReport { get; set; }
        public DbSet<RedactorViewPaparaziMedia> RedactorViewPaparaziMedia { get; set; }

    }
}

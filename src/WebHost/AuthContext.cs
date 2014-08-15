﻿using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using Yo.WebHost.Models;

namespace Yo.WebHost {

    public class ApplicationUser : IdentityUser {
        public string CustomUserProperty { get; set; }
    }

    [DbConfigurationType(typeof(MySqlConfiguration))]
    public class AuthContext : IdentityDbContext<ApplicationUser> {
        public AuthContext()
            : base("AuthContext") {
                Database.SetInitializer(new MigrateDatabaseToLatestVersion<AuthContext, Migrations.Configuration>());
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
﻿using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Entities.Concrete;
using Core.Entities.Concrete;

namespace DataAccess.Concrete.EntityFramework.Contexts
{
	public class ProjectDbContext : DbContext
	{
		// This added for trouble which is from Ef Implementation. It need parameterless constructor.
		public ProjectDbContext()
        {

        }
		public ProjectDbContext(DbContextOptions<ProjectDbContext> options, IConfiguration configuration)
																				: base(options)
		{
			Configuration = configuration;
		}

		protected ProjectDbContext(DbContextOptions options, IConfiguration configuration)
																		: base(options)
		{
			Configuration = configuration;
		}

        public DbSet<Integration> Integrations { get; set; }
        public DbSet<Translate> Translates { get; set; }
        public DbSet<Log> Logs { get; set; }
		public DbSet<Language> Languages { get; set; }


		protected IConfiguration Configuration { get; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				// burası default db nin bağlantısı. Şu anda PostgreSQL default olarak kullanılıyor.
				base.OnConfiguring(optionsBuilder.UseNpgsql(Configuration.GetConnectionString("DArchPgContext")).EnableSensitiveDataLogging());

			}
		}

	}
}

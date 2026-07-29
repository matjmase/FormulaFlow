using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace FormulaFlow.Data.Design
{
    // Design-time factory for migrations and tooling
    public class FormulaFlowContextFactory : IDesignTimeDbContextFactory<FormulaFlowContext>
    {
        public FormulaFlowContext CreateDbContext(string[] args)
        {
            var basePath = Directory.GetCurrentDirectory();
            var configDir = FindConfigDirectory(basePath, "appsettings.json");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(configDir)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Could not find a connection string named 'DefaultConnection'.");

            var optionsBuilder = new DbContextOptionsBuilder<FormulaFlowContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new FormulaFlowContext(optionsBuilder.Options);
        }

        private static string FindConfigDirectory(string startDirectory, string fileName)
        {
            var dir = new DirectoryInfo(startDirectory);
            for (int i = 0; i < 6 && dir != null; i++)
            {
                if (File.Exists(Path.Combine(dir.FullName, fileName)))
                    return dir.FullName;

                dir = dir.Parent;
            }

            return startDirectory;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Gem4NetRepository.Model
{
    public class GemDbContextDesignTimeFactory : IDesignTimeDbContextFactory<GemDbContext>
    {
        public GemDbContext CreateDbContext(string[] args)
        {
            // 讀取環境名稱（預設 Development）
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

            // 建立設定來源：appsettings.json + appsettings.{Environment}.json + 環境變數
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            // 從設定或環境變數抓連線字串
            // 1) 先找 ConnectionStrings:Default
            // 2) 再找 環境變數 ConnectionStrings__Default
            var connectionString =
                config.GetConnectionString("Npgsql") ??
                Environment.GetEnvironmentVariable("ConnectionStrings__Default") ??
                throw new InvalidOperationException("找不到連線字串 ConnectionStrings:Default。");

            var optionsBuilder = new DbContextOptionsBuilder<GemDbContext>();
            optionsBuilder.UseNpgsql(connectionString); // ← 改成 UseNpgsql / UseMySql 皆可

            return new GemDbContext(options: optionsBuilder.Options, config);
        }
    }
}

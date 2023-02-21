using System.Collections.ObjectModel;
using System.Data;
using CottageApi.Core.Configurations;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;

namespace CottageApi.Extensions
{
    public static class LoggingExtensions
    {
        public static void ConfigureLogging(Config config)
        {
            const LogEventLevel minimumLogLevel = LogEventLevel.Information;

            var configuration = new LoggerConfiguration()
                .MinimumLevel.Is(minimumLogLevel)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerHandler", LogEventLevel.Warning)
                .MinimumLevel.Override("IdentityServer4.AccessTokenValidation", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .Enrich.WithThreadId()
                .Enrich.WithMachineName();

            configuration = configuration
                .WriteTo.MSSqlServer(
                    autoCreateSqlTable: true,
                    connectionString: config.Connections.LogsConnectionString,
                    tableName: "CottageApiLogs",
                    columnOptions: GetLogColumns());

            Log.Logger = configuration.CreateLogger();

            Log.Information("Logging configured");
        }

        private static ColumnOptions GetLogColumns()
        {
            var columnOptions = new ColumnOptions();
            columnOptions.Store.Remove(StandardColumn.MessageTemplate);
            columnOptions.Store.Remove(StandardColumn.Properties);
            columnOptions.AdditionalColumns = new Collection<SqlColumn>
            {
                new SqlColumn
                {
                    DataType = SqlDbType.NVarChar,
                    ColumnName = "RequestPath"
                },
                new SqlColumn
                {
                    DataType = SqlDbType.NVarChar,
                    ColumnName = "ThreadId"
                },
                new SqlColumn
                {
                    DataType = SqlDbType.NVarChar,
                    ColumnName = "SourceContext"
                },
                new SqlColumn
                {
                    DataType = SqlDbType.NVarChar,
                    ColumnName = "MachineName"
                }
            };

            return columnOptions;
        }
    }
}
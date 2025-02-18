﻿using Core.CrossCuttingConcerns.Logging.Log4Net;
using Core.CrossCuttingConcerns.Logging.Serilog.ConfigurationModels;
using Core.Utilities.IoC;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using NpgsqlTypes;
using Serilog.Sinks.PostgreSQL;

namespace Core.CrossCuttingConcerns.Logging.Serilog.Loggers
{
    public class PostgreSqlLogger : LoggerServiceBase
    {
        public PostgreSqlLogger()
        {
            var configuration = ServiceTool.ServiceProvider.GetService<IConfiguration>();

            var logConfig = configuration.GetSection("SeriLogConfigurations:PostgreConfiguration")
                .Get<PostgreConfiguration>() ?? throw new Exception(Utilities.Messages.SerilogMessages.NullOptionsMessage);

            IDictionary<string, ColumnWriterBase> columnWriters = new Dictionary<string, ColumnWriterBase>
                        {
                            { "MessageTemplate", new MessageTemplateColumnWriter(NpgsqlDbType.Text) },
                            { "Level", new LevelColumnWriter(true, NpgsqlDbType.Varchar) },
                            { "TimeStamp", new TimestampColumnWriter(NpgsqlDbType.Timestamp) },
                            { "Exception", new ExceptionColumnWriter(NpgsqlDbType.Text) },

                        };


            var seriLogConfig = new LoggerConfiguration()
                    .WriteTo.PostgreSQL(connectionString: logConfig.ConnectionString, tableName: "Logs", columnWriters, needAutoCreateTable: false)
                    .CreateLogger();
            Logger = seriLogConfig;
        }
    }
}

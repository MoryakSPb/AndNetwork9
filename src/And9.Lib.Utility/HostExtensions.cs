﻿using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace And9.Lib.Utility;

public static class HostExtensions
{
    public static IHostBuilder ConfigureAndNetConsole(this IHostBuilder builder, LogLevel logLevel = LogLevel.Information)
    {
        return builder.ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddSimpleConsole(options =>
            {
                options.IncludeScopes = true;
                options.SingleLine = false;
                options.TimestampFormat = "[yy/MM/dd HH:mm:ss.fff]    ";
                options.ColorBehavior = LoggerColorBehavior.Enabled;
                options.UseUtcTimestamp = true;
            });
            logging.SetMinimumLevel(logLevel);
        });
    }
}
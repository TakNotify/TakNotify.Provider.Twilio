// Copyright (c) Frandi Dwi 2020. All rights reserved.
// Licensed under the MIT License.
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using TakNotify;

namespace ConsoleApp
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.Development.json", true, true)
                .Build();

            Console.WriteLine("Starting Twilio Provider test...");

            // logger
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.SetMinimumLevel(LogLevel.Debug).AddConsole();
            });

            // twilio provider
            var twilioProvider = new TwilioProvider(new TwilioOptions
            {
                AccountSid = configuration["Twilio:AccountSid"],
                AuthToken = configuration["Twilio:AuthToken"]
            }, new HttpClient(), loggerFactory);

            // notification
            var notification = Notification.GetInstance(loggerFactory.CreateLogger<Notification>());
            notification.AddProvider(twilioProvider);

            // sms message
            var message = new SMSMessage
            {
                FromNumber = configuration["DefaultFromNumber"],
                ToNumber = configuration["TestNumber"],
                Content = $"This message was sent from {Environment.MachineName} at {DateTime.Now.ToLongTimeString()}."
            };

            // send the message
            var result = await notification.SendSmsWithTwilio(message);

            return result.IsSuccess ? 0 : 1;
        }
    }
}

// Copyright (c) Frandi Dwi 2020. All rights reserved.
// Licensed under the MIT License.
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Twilio.Clients;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace TakNotify
{
    /// <summary>
    /// The TakNotify provider to send SMS with Twilio service
    /// </summary>
    public class TwilioProvider : NotificationProvider
    {
        private readonly ITwilioRestClient _twilioClient;

        /// <summary>
        /// Create the instance of <see cref="TwilioProvider"/>
        /// </summary>
        /// <param name="twilioClient">The instance of <see cref="ITwilioRestClient"/></param>
        /// <param name="loggerFactory">The instance of <see cref="ILoggerFactory"/></param>
        public TwilioProvider(ITwilioRestClient twilioClient, ILoggerFactory loggerFactory)
            : base(new NotificationProviderOptions(), loggerFactory)
        {
            _twilioClient = twilioClient;
        }

        /// <summary>
        /// Create the instance of <see cref="TwilioProvider"/>
        /// </summary>
        /// <param name="options">The Twilio provider options</param>
        /// <param name="httpClient">The instance of <see cref="HttpClient"/></param>
        /// <param name="loggerFactory">The instance of <see cref="ILoggerFactory"/></param>
        public TwilioProvider(TwilioOptions options, HttpClient httpClient, ILoggerFactory loggerFactory) 
            : base(options, loggerFactory)
        {
            _twilioClient = new TwilioClient(options.AccountSid, options.AuthToken, httpClient);
        }

        /// <summary>
        /// Create the instance of <see cref="TwilioProvider"/>
        /// </summary>
        /// <param name="options">The Twilio provider options</param>
        /// <param name="httpClientFactory">The instance of <see cref="IHttpClientFactory"/></param>
        /// <param name="loggerFactory">The instance of <see cref="ILoggerFactory"/></param>
        public TwilioProvider(IOptions<TwilioOptions> options, IHttpClientFactory httpClientFactory, ILoggerFactory loggerFactory)
            : base(options.Value, loggerFactory)
        {
            _twilioClient = new TwilioClient(options.Value.AccountSid, options.Value.AuthToken, httpClientFactory.CreateClient());
        }

        /// <inheritdoc cref="NotificationProvider.Name"/>
        public override string Name => TwilioConstants.DefaultName;

        /// <inheritdoc cref="NotificationProvider.Send(MessageParameterCollection)"/>
        public override async Task<NotificationResult> Send(MessageParameterCollection messageParameters)
        {
            var smsMessage = new SMSMessage(messageParameters);

            Logger.LogDebug(TwilioLogMessages.Sending_Start, smsMessage.ToNumber);

            try
            {
                var message = await MessageResource.CreateAsync(
                        new PhoneNumber(smsMessage.ToNumber),
                        from: new PhoneNumber(smsMessage.FromNumber),
                        body: smsMessage.Content,
                        client: _twilioClient);

                Logger.LogDebug(TwilioLogMessages.Sending_End, smsMessage.ToNumber, message.Sid);
                return new NotificationResult(true);
            }
            catch (Exception ex)
            {
                Logger.LogWarning(ex, TwilioLogMessages.Sending_Failed, smsMessage.ToNumber, ex.Message);
                return new NotificationResult(new List<string> { ex.Message });
            }
        }
    }
}

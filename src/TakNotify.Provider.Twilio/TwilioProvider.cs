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
using TwilioHttp = Twilio.Http;

namespace TakNotify
{
    /// <summary>
    /// The TakNotify provider to send SMS with Twilio service
    /// </summary>
    public class TwilioProvider : NotificationProvider
    {
        private readonly ITwilioRestClient _twilioRestClient;
        
        /// <summary>
        /// Create the instance of <see cref="TwilioProvider"/>
        /// </summary>
        /// <param name="options">The Twilio provider options</param>
        /// <param name="twilioHttpClient">The instance of <see cref="TwilioHttp.HttpClient"/></param>
        /// <param name="loggerFactory">The instance of <see cref="ILoggerFactory"/></param>
        public TwilioProvider(TwilioOptions options, TwilioHttp.HttpClient twilioHttpClient, ILoggerFactory loggerFactory)
            : base(options, loggerFactory)
        {
            _twilioRestClient = new TwilioRestClient(options.AccountSid, options.AuthToken, httpClient: twilioHttpClient);
        }

        /// <summary>
        /// Create the instance of <see cref="TwilioProvider"/>
        /// </summary>
        /// <param name="options">The Twilio provider options</param>
        /// <param name="netHttpClient">The instance of <see cref="HttpClient"/></param>
        /// <param name="loggerFactory">The instance of <see cref="ILoggerFactory"/></param>
        public TwilioProvider(TwilioOptions options, HttpClient netHttpClient, ILoggerFactory loggerFactory) 
            : base(options, loggerFactory)
        {
            _twilioRestClient = new TwilioRestClient(options.AccountSid, options.AuthToken, 
                httpClient: new TwilioHttp.SystemNetHttpClient(netHttpClient));
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
            _twilioRestClient = new TwilioRestClient(options.Value.AccountSid, options.Value.AuthToken, 
                httpClient: new TwilioHttp.SystemNetHttpClient(httpClientFactory.CreateClient()));
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
                        client: _twilioRestClient);

                Logger.LogDebug(TwilioLogMessages.Sending_End, smsMessage.ToNumber, message.Sid);
                return new NotificationResult(true, new Dictionary<string, object> { { nameof(message.Sid), message.Sid } });
            }
            catch (Exception ex)
            {
                Logger.LogWarning(ex, TwilioLogMessages.Sending_Failed, smsMessage.ToNumber, ex.Message);
                return new NotificationResult(new List<string> { ex.Message });
            }
        }
    }
}

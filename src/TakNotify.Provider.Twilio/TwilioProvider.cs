using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Twilio;
using Twilio.Clients;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace TakNotify
{
    public class TwilioProvider : NotificationProvider
    {
        private readonly TwilioOptions _options;
        private readonly ITwilioRestClient _twilioClient;

        public TwilioProvider(TwilioOptions options, ILoggerFactory loggerFactory) 
            : base(options, loggerFactory)
        {
            _options = options;
            
            TwilioClient.Init(options.AccountSid, options.AuthToken);
            _twilioClient = TwilioClient.GetRestClient();
        }

        public TwilioProvider(IOptions<TwilioOptions> options, ILoggerFactory loggerFactory)
            : base(options.Value, loggerFactory)
        {
            _options = options.Value;

            TwilioClient.Init(options.Value.AccountSid, options.Value.AuthToken);
            _twilioClient = TwilioClient.GetRestClient();
        }

        public override string Name => TwilioConstants.DefaultName;

        public override async Task<NotificationResult> Send(MessageParameterCollection messageParameters)
        {
            var smsMessage = new SMSMessage(messageParameters);

            var messageResource = await MessageResource.CreateAsync(
                new PhoneNumber(smsMessage.ToNumber),
                from: new PhoneNumber(smsMessage.FromNumber),
                body: smsMessage.Content);

            throw new NotImplementedException();
        }
    }
}

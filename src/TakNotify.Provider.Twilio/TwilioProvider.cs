using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading.Tasks;
using Twilio.Clients;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace TakNotify
{
    public class TwilioProvider : NotificationProvider
    {
        private readonly ITwilioRestClient _twilioClient;

        public TwilioProvider(ITwilioRestClient twilioClient, ILoggerFactory loggerFactory)
            : base(new NotificationProviderOptions(), loggerFactory)
        {
            _twilioClient = twilioClient;
        }

        public TwilioProvider(TwilioOptions options, HttpClient httpClient, ILoggerFactory loggerFactory) 
            : base(options, loggerFactory)
        {
            _twilioClient = new TwilioClient(options.AccountSid, options.AuthToken, httpClient);
        }

        public TwilioProvider(IOptions<TwilioOptions> options, HttpClient httpClient, ILoggerFactory loggerFactory)
            : base(options.Value, loggerFactory)
        {
            _twilioClient = new TwilioClient(options.Value.AccountSid, options.Value.AuthToken, httpClient);
        }

        public override string Name => TwilioConstants.DefaultName;

        public override async Task<NotificationResult> Send(MessageParameterCollection messageParameters)
        {
            var smsMessage = new SMSMessage(messageParameters);

            Logger.LogDebug(TwilioLogMessages.Sending_Start, smsMessage.ToNumber);

            var message = await MessageResource.CreateAsync(
                new PhoneNumber(smsMessage.ToNumber),
                from: new PhoneNumber(smsMessage.FromNumber),
                body: smsMessage.Content,
                client: _twilioClient);

            Logger.LogDebug(TwilioLogMessages.Sending_End, smsMessage.ToNumber, message.Sid);

            return new NotificationResult(true);
        }
    }
}

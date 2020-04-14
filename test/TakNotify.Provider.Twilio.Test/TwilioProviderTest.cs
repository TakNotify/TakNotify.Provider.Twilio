using Microsoft.Extensions.Logging;
using Moq;
using TakNotify.Test;
using TwilioHttp = Twilio.Http;
using Xunit;
using Twilio.Rest.Api.V2010.Account;

namespace TakNotify.Provider.Twilio.Test
{
    public class TwilioProviderTest
    {
        private readonly Mock<ILoggerFactory> _loggerFactory;
        private readonly Mock<ILogger<Notification>> _logger;
        private readonly Mock<TwilioHttp.HttpClient> _twilioHttpClient;

        public TwilioProviderTest()
        {
            _loggerFactory = new Mock<ILoggerFactory>();
            _logger = new Mock<ILogger<Notification>>();
            _twilioHttpClient = new Mock<TwilioHttp.HttpClient>();

            _loggerFactory.Setup(lf => lf.CreateLogger(It.IsAny<string>())).Returns(_logger.Object);
        }

        [Fact]
        public async void Send_Success()
        {
            _twilioHttpClient.Setup(client => client.MakeRequestAsync(It.IsAny<TwilioHttp.Request>()))
                .ReturnsAsync(new TwilioHttp.Response(System.Net.HttpStatusCode.OK, "{'sid': '111'}"));

            var message = new SMSMessage
            {
                FromNumber = "0123",
                ToNumber = "0321",
                Content = "Test"
            };
            
            var provider = new TwilioProvider(new TwilioOptions(), _twilioHttpClient.Object, _loggerFactory.Object);
            var result = await provider.Send(message.ToParameters());
            var sid = result.ReturnedValues["Sid"].ToString();

            Assert.True(result.IsSuccess);
            Assert.Equal("111", sid);
            Assert.Empty(result.Errors);

            var startMessage = LoggerHelper.FormatLogValues(TwilioLogMessages.Sending_Start, message.ToNumber);
            _logger.VerifyLog(LogLevel.Debug, startMessage);

            var endMessage = LoggerHelper.FormatLogValues(TwilioLogMessages.Sending_End, message.ToNumber, sid);
            _logger.VerifyLog(LogLevel.Debug, endMessage);
        }

        [Fact]
        public async void Send_Failed()
        {
            _twilioHttpClient.Setup(client => client.MakeRequestAsync(It.IsAny<TwilioHttp.Request>()))
                .ReturnsAsync(new TwilioHttp.Response(System.Net.HttpStatusCode.BadRequest, "Error01"));

            var message = new SMSMessage
            {
                FromNumber = "0123",
                ToNumber = "0321",
                Content = "Test"
            };

            var provider = new TwilioProvider(new TwilioOptions(), _twilioHttpClient.Object, _loggerFactory.Object);
            var result = await provider.Send(message.ToParameters());

            Assert.False(result.IsSuccess);
            Assert.NotEmpty(result.Errors);
            Assert.Contains("Error01", result.Errors[0]);

            var startMessage = LoggerHelper.FormatLogValues(TwilioLogMessages.Sending_Start, message.ToNumber);
            _logger.VerifyLog(LogLevel.Debug, startMessage);

            var warningMessage = LoggerHelper.FormatLogValues(TwilioLogMessages.Sending_Failed, message.ToNumber, result.Errors);
            _logger.VerifyLog(LogLevel.Warning, warningMessage);
        }
    }
}

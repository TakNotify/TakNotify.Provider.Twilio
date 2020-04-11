using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Twilio.Clients;
using TwilioHttp = Twilio.Http;

namespace TakNotify
{
    public class TwilioClient : ITwilioRestClient
    {
        private readonly ITwilioRestClient _innerClient;

        public TwilioClient(string accountSid, string authToken, HttpClient httpClient)
        {
            _innerClient = new TwilioRestClient(
                accountSid,
                authToken,
                httpClient: new TwilioHttp.SystemNetHttpClient(httpClient));
        }

        public string AccountSid => _innerClient.AccountSid;

        public string Region => _innerClient.Region;

        public TwilioHttp.HttpClient HttpClient => _innerClient.HttpClient;

        public TwilioHttp.Response Request(TwilioHttp.Request request)
            => _innerClient.Request(request);

        public Task<TwilioHttp.Response> RequestAsync(TwilioHttp.Request request)
            => _innerClient.RequestAsync(request);
    }
}

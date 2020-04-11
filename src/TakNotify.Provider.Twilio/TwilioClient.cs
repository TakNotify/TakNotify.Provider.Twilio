// Copyright (c) Frandi Dwi 2020. All rights reserved.
// Licensed under the MIT License.
using System.Net.Http;
using System.Threading.Tasks;
using Twilio.Clients;
using TwilioHttp = Twilio.Http;

namespace TakNotify
{
    /// <summary>
    /// Custom implementation of <see cref="ITwilioRestClient"/>
    /// to allow integration with <see cref="System.Net.Http.HttpClient"/>
    /// </summary>
    public class TwilioClient : ITwilioRestClient
    {
        private readonly ITwilioRestClient _innerClient;

        /// <summary>
        /// Create the instance of <see cref="TakNotify.TwilioClient"/>
        /// </summary>
        /// <param name="accountSid">The Twilio Account SID</param>
        /// <param name="authToken">The Twilio Auth Token</param>
        /// <param name="httpClient">The instance of <see cref="System.Net.Http.HttpClient"/></param>
        public TwilioClient(string accountSid, string authToken, HttpClient httpClient)
        {
            _innerClient = new TwilioRestClient(
                accountSid,
                authToken,
                httpClient: new TwilioHttp.SystemNetHttpClient(httpClient));
        }

        /// <summary>
        /// The Account SID
        /// </summary>
        public string AccountSid => _innerClient.AccountSid;

        /// <summary>
        /// The region
        /// </summary>
        public string Region => _innerClient.Region;

        /// <summary>
        /// The HTTP client that is used in the request
        /// </summary>
        public TwilioHttp.HttpClient HttpClient => _innerClient.HttpClient;

        /// <summary>
        /// Make a request to Twilio service
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public TwilioHttp.Response Request(TwilioHttp.Request request)
            => _innerClient.Request(request);

        /// <summary>
        /// Make a request to Twilio service
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<TwilioHttp.Response> RequestAsync(TwilioHttp.Request request)
            => _innerClient.RequestAsync(request);
    }
}

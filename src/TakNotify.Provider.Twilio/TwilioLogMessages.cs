using System;
using System.Collections.Generic;
using System.Text;

namespace TakNotify
{
    /// <summary>
    /// The log messages
    /// </summary>
    public static class TwilioLogMessages
    {
        /// <summary>
        /// The message to display before sending sms
        /// </summary>
        public const string Sending_Start = "Sending SMS to {toAddresses}";

        /// <summary>
        /// The message to display after sending sms
        /// </summary>
        public const string Sending_End = "SMS has been sent to {toAddresses}. Sid: {messageSid}";
    }
}

// Copyright (c) Frandi Dwi 2020. All rights reserved.
// Licensed under the MIT License.
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

        /// <summary>
        /// The failed message
        /// </summary>
        public const string Sending_Failed = "Failed to send SMS to {toAddress}. Error: {error}";
    }
}

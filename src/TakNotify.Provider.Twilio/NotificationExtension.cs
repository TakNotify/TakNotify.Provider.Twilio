// Copyright (c) Frandi Dwi 2020. All rights reserved.
// Licensed under the MIT License.
using System.Threading.Tasks;

namespace TakNotify
{
    /// <summary>
    /// The extension for <see cref="INotification"/> to send SMS with Twilio
    /// </summary>
    public static class NotificationExtension
    {
        /// <summary>
        /// Send message with <see cref="TwilioProvider"/>
        /// </summary>
        /// <param name="notification">The notification object</param>
        /// <param name="message">The wrapper of SMS to be sent</param>
        /// <returns></returns>
        public static Task<NotificationResult> SendSmsWithTwilio(this INotification notification, SMSMessage message)
        {
            return notification.Send(TwilioConstants.DefaultName, message.ToParameters());
        }
    }
}

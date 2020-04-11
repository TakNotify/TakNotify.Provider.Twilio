using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TakNotify
{
    public static class NotificationExtension
    {
        public static Task<NotificationResult> SendSmsWithTwilio(this INotification notification, SMSMessage message)
        {
            return notification.Send(TwilioConstants.DefaultName, message.ToParameters());
        }
    }
}

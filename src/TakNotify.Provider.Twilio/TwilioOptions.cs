using System;
using System.Collections.Generic;
using System.Text;

namespace TakNotify
{
    public class TwilioOptions : NotificationProviderOptions
    {
        internal static string Parameter_AccountSid = $"{TwilioConstants.DefaultName}_{nameof(AccountSid)}";
        internal static string Parameter_AuthToken = $"{TwilioConstants.DefaultName}_{nameof(AuthToken)}";

        public TwilioOptions()
        {
            Parameters.Add(Parameter_AccountSid, "");
            Parameters.Add(Parameter_AuthToken, "");
        }

        public string AccountSid
        {
            get => Parameters[Parameter_AccountSid]?.ToString();
            set => Parameters[Parameter_AccountSid] = value;
        }

        public string AuthToken
        {
            get => Parameters[Parameter_AuthToken]?.ToString();
            set => Parameters[Parameter_AuthToken] = value;
        }


    }
}

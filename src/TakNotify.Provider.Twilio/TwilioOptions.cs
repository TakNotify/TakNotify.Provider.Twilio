// Copyright (c) Frandi Dwi 2020. All rights reserved.
// Licensed under the MIT License.
namespace TakNotify
{
    /// <summary>
    /// The options for <see cref="TwilioProvider"/>
    /// </summary>
    public class TwilioOptions : NotificationProviderOptions
    {
        internal static string Parameter_AccountSid = $"{TwilioConstants.DefaultName}_{nameof(AccountSid)}";
        internal static string Parameter_AuthToken = $"{TwilioConstants.DefaultName}_{nameof(AuthToken)}";
        internal static string Parameter_DefaultFromNumber = $"{TwilioConstants.DefaultName}_{nameof(DefaultFromNumber)}";

        /// <summary>
        /// Create the instance of <see cref="TwilioOptions"/>
        /// </summary>
        public TwilioOptions()
        {
            Parameters.Add(Parameter_AccountSid, "");
            Parameters.Add(Parameter_AuthToken, "");
            Parameters.Add(Parameter_DefaultFromNumber, "");
        }

        /// <summary>
        /// The Twilio account sid
        /// </summary>
        public string AccountSid
        {
            get => Parameters[Parameter_AccountSid]?.ToString();
            set => Parameters[Parameter_AccountSid] = value;
        }

        /// <summary>
        /// The Twilio auth token
        /// </summary>
        public string AuthToken
        {
            get => Parameters[Parameter_AuthToken]?.ToString();
            set => Parameters[Parameter_AuthToken] = value;
        }

        /// <summary>
        /// The default "From Number" that will be used if the <see cref="SMSMessage.FromNumber"/> is empty
        /// </summary>
        public string DefaultFromNumber
        {
            get => Parameters[Parameter_DefaultFromNumber].ToString();
            set => Parameters[Parameter_DefaultFromNumber] = value;
        }
    }
}

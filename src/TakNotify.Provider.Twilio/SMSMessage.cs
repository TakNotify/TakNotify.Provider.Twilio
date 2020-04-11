using System;
using System.Collections.Generic;
using System.Text;

namespace TakNotify
{
    /// <summary>
    /// The wrapper of an sms message properties
    /// </summary>
    public class SMSMessage
    {
        internal static string Parameter_FromNumber = $"{TwilioConstants.DefaultName}_{nameof(FromNumber)}";
        internal static string Parameter_ToNumber = $"{TwilioConstants.DefaultName}_{nameof(ToNumber)}";
        internal static string Parameter_Content = $"{TwilioConstants.DefaultName}_{nameof(Content)}";

        /// <summary>
        /// Create the instance of <see cref="SMSMessage"/>
        /// </summary>
        public SMSMessage()
        {

        }

        /// <summary>
        /// Create the instance of <see cref="SMSMessage"/>
        /// </summary>
        /// <param name="parameters">The collection of message parameters</param>
        public SMSMessage(MessageParameterCollection parameters)
        {
            if (parameters.ContainsKey(Parameter_FromNumber))
                FromNumber = parameters[Parameter_FromNumber];

            if (parameters.ContainsKey(Parameter_ToNumber))
                ToNumber = parameters[Parameter_ToNumber];

            if (parameters.ContainsKey(Parameter_Content))
                Content = parameters[Parameter_Content];
        }

        /// <summary>
        /// The sender number
        /// </summary>
        public string FromNumber { get; set; }

        /// <summary>
        /// The destination number
        /// </summary>
        public string ToNumber { get; set; }

        /// <summary>
        /// The content of the message
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Convert this object into message parameters
        /// </summary>
        /// <returns></returns>
        public MessageParameterCollection ToParameters()
        {
            var parameters = new MessageParameterCollection();

            if (!string.IsNullOrEmpty(FromNumber))
                parameters.Add(Parameter_FromNumber, FromNumber);

            if (!string.IsNullOrEmpty(ToNumber))
                parameters.Add(Parameter_ToNumber, ToNumber);

            if (!string.IsNullOrEmpty(Content))
                parameters.Add(Parameter_Content, Content);

            return parameters;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace BSD
{
    public class MessagePublisher
    {
        private readonly string _accountSid;
        private readonly string _authToken;
        private readonly string _fromPhoneNumber;
        private readonly string _toPhoneNumber;

        public MessagePublisher(string accountSid, string authToken, string fromPhoneNumber, string toPhoneNumber)
        {
            _accountSid = accountSid;
            _authToken = authToken;
            _fromPhoneNumber = fromPhoneNumber;
            _toPhoneNumber = toPhoneNumber;
        }

        public void InitTwilio()
        {
            TwilioClient.Init(_accountSid, _authToken);
        }

        public void SendMessage(string message)
        {
            var msg = MessageResource.Create(
                body: message,
                from: new PhoneNumber(_fromPhoneNumber),
                to: new PhoneNumber(_toPhoneNumber));
        }
      
    }
}

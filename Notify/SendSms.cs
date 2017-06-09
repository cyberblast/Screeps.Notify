using Screeps.ApiClient;
using System.Collections.Generic;

using System;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Screeps.Notify
{
    public class SendSms
    {
        public string TwilioSid { get; set; }
        public string TwilioToken { get; set; }
        public string SmsSender { get; set; }
        public List<string> Recipient { get; set; }

        private Http HttpClient;
        private string HttpUrl;

        public SendSms(string twilioSid, string twilioToken, string smsSender)
        {
            TwilioSid = twilioSid;
            TwilioToken = twilioToken;
            SmsSender = smsSender;
            Recipient = new List<string>();
        }

        public string Send(string message)
        {
            if (Recipient == null || Recipient.Count == 0)
                return null;

            if (HttpUrl == null)
                HttpUrl = string.Format("https://api.twilio.com/2010-04-01/Accounts/{0}/Messages", TwilioSid);

            if (HttpClient == null)
            {
                HttpClient = new Http();
                HttpClient.ContentType = "application/x-www-form-urlencoded";
                string encoded = Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(TwilioSid + ":" + TwilioToken));
                HttpClient.SetHeader("Authorization", "Basic " + encoded);
            }

            List<string> response = new List<string>();
            foreach (string recepient in Recipient)
            {
                string[] args = new string[3]
                {
                    HttpUtility.UrlEncode(SmsSender),
                    HttpUtility.UrlEncode(recepient),
                    HttpUtility.UrlEncode(message)
                };
                string data = string.Format("From={0}&To={1}&Body={2}", args);
                response.Add(
                    HttpClient.Post(HttpUrl, data) as string);
            }
            return string.Join(",", response);
        }
    }
}

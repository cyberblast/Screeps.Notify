using Screeps.ApiClient;

namespace Screeps.Notify
{
    public class SendHttp
    {
        Http http = null;

        public string ApiKey { get; set; }
        public string HttpUrl { get; set; }
        public string HttpUser { get; set; }
        public string HttpPassword { get; set; }

        public SendHttp(string url)
        {
            HttpUrl = url;
        }

        public string Send(object postdata)
        {
            if (string.IsNullOrEmpty(HttpUrl))
                return null;

            if (http == null)
            {
                http = new Http();
                http.UserAgent = "screeps_notify";
                if (!string.IsNullOrEmpty(ApiKey))
                    http.SetHeader("x-api-key", ApiKey);
                if (!string.IsNullOrEmpty(HttpUser))
                    http.SetCredential(HttpUser, HttpPassword);
            }
            
            return http.Post(HttpUrl, postdata) as string;
        }
    }
}

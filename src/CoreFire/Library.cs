using System;
using System.Net.Http;

namespace CoreFire
{
    public enum RequestMethod
    {
        Get,
        Post,
        Put,
        Patch,
        Delete,
    }

    public class RequestHelper
    {
        public string MakeSyncHttpGet(string url)
        {
            using (var client = new HttpClient())
                return client.GetStringAsync(url).Result;
        }
    }
}
using System;
using System.Net.Http;

namespace ClassLibrary
{
    public class Class1
    {
        public string MakeSyncHttpGet(string url)
        {
            using (var client = new HttpClient())
                return client.GetStringAsync(url).Result;
        }
    }
}
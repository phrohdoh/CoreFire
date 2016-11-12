using System;
using System.Collections.Generic;

namespace CoreFire
{
    public class FireClient
    {
        public readonly Dictionary<string, string> RequestParams = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public string Url;

        /// <summary></summary>
        /// <param name="url">The firebase URL of your db.</param>
        /// <c>new FireClient("https://your-db.firebaseio.com/")</c>
        public FireClient(string url)
        {
            Url = url;
        }

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Url);
        }
    }
}
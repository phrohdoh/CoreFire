using System;
using System.Collections.Generic;

namespace CoreFire
{
    public class FireClientBuilder
    {
        string url;
        string authToken;

        // We do not want any public ctors.
        FireClientBuilder() { }

        public static FireClientBuilder Create() => new FireClientBuilder();

        /// <param name="url">The firebase URL of your db.</param>
        /// <c>FireClient.Create().WithUrl("http://your-db.firebaseio.com");</c>
        public FireClientBuilder WithUrl(string url)
        {
            this.url = url;
            return this;
        }

        /// <param name="url">The firebase URL of your db.</param>
        /// <c>FireClient.Create().WithAuth("spqiQHnlwA6uS6Ur8H3ZrJinHbX951DzDySazIA");</c>
        public FireClientBuilder WithAuth(string authToken)
        {
            this.authToken = authToken;
            return this;
        }

        public FireClient Build()
        {
            return new FireClient
            {
                Url = url,
                AuthToken = authToken,
            };
        }
    }

    public class FireClient
    {
        public readonly Dictionary<string, string> RequestParams = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public string Url;
        public string AuthToken;

        internal FireClient() { }

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Url);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;

namespace CoreFire
{
    public class FireClientBuilder
    {
        Uri uri;
        string authToken;

        // We do not want any public ctors.
        FireClientBuilder() { }

        public static FireClientBuilder Create() => new FireClientBuilder();

        /// <param name="uri">The firebase URI of your db.</param>
        /// <c>FireClient.Create().WithUri("http://your-db.firebaseio.com");</c>
        public FireClientBuilder WithUri(Uri uri)
        {
            this.uri = uri;
            return this;
        }

        /// <param name="uri">The firebase URI of your db.</param>
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
                Uri = uri,
                AuthToken = authToken,
            };
        }
    }

    public class FireClient
    {
        public readonly Dictionary<string, string> RequestParams = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public Uri Uri;
        public string AuthToken;

        internal FireClient() { }

        public HttpResponseMessage PushSync(string absolutePath, object content)
        {
            if (!absolutePath.StartsWith("/"))
                absolutePath = "/" + absolutePath;

            if (absolutePath.EndsWith("/"))
                absolutePath = absolutePath.Substring(0, absolutePath.Length - 2);

            if (!absolutePath.EndsWith(".json"))
                absolutePath += ".json";

            var builder = new UriBuilder(Uri);
            builder.Path = string.Join("/", GetStringSegmentsWithoutTrailingDotJson()) + absolutePath;

            var finalUri = builder.Uri;
            Console.WriteLine(finalUri);

            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(content);
                return client.PostAsync(finalUri, new StringContent(json)).Result;
            }
        }

        /// <summary>
        /// Remove '.json'<br/>
        /// We may have '/foo/bar.json' and want '/foo/bar/baz.json'
        /// </summary>
        string[] GetStringSegmentsWithoutTrailingDotJson()
        {
            var segments = new List<string>();
            foreach (var seg in Uri.Segments)
            {
                if (string.IsNullOrWhiteSpace(seg) || seg == "/")
                    continue;

                segments.Add(seg.Replace(".json", "").Replace("/", ""));
            }

            return segments.ToArray();
        }
    }
}
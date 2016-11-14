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

        public static FireClientBuilder New() => new FireClientBuilder();
        public FireClient Build() => FireClient.New(uri, authToken);

        /// <param name="uri">The firebase URI of your db.</param>
        /// <c>FireClientBuilder.New().WithUri("http://your-db.firebaseio.com");</c>
        public FireClientBuilder WithUri(Uri uri)
        {
            this.uri = uri;
            return this;
        }

        /// <param name="uri">The authentication token of your db.</param>
        /// <c>FireClientBuilder.New().WithAuth("spqiQHnlwA6uS6Ur8H3ZrJinHbX951DzDySazIA");</c>
        public FireClientBuilder WithAuth(string authToken)
        {
            this.authToken = authToken;
            return this;
        }
    }

    /// <summary>Used to get the path resulting from a Push</summary>
    struct PushResponseObject
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class FireClient
    {
        public readonly Dictionary<string, string> RequestParams = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public Uri Uri { get; private set; }
        public string AuthToken { get; private set; }

        public bool IsAuthed => !string.IsNullOrWhiteSpace(AuthToken);

        internal static FireClient New(Uri uri, string authToken) => new FireClient
        {
            Uri = uri,
            AuthToken = authToken,
        };

        // I do not know how Firebase intends for users to
        // push/append onto an array.
        //
        // Take for example:
        // /foo = [ "bar", "baz" ]
        // client.PushSync("/foo", "qux");
        //
        // I would expect:
        // /foo = [ "bar", "baz", "qux" ]
        //
        // Instead it seems I have to:
        // 1) Get /foo
        // 2) Deserialize the result
        // 3) Mutate the resulting object
        // 4) Set /foo
        public string PushSync(string absolutePath, object content)
        {
            var json = JsonConvert.SerializeObject(content);
            var finalUri = BuildFinalUriFromAbsolutePath(absolutePath);
            var absPath = finalUri.AbsolutePath;
            var path = absPath.Substring(0, absPath.LastIndexOf(".json"));

            using (var client = new HttpClient())
            using (var response = client.PostAsync(finalUri, new StringContent(json)).Result)
            {
                var responseContent = response.Content.ReadAsStringAsync().Result;
                var responseObj = JsonConvert.DeserializeObject<PushResponseObject>(responseContent);
                return path + "/" + responseObj.Name;
            }
        }

        public void SetSync(string absolutePath, object content)
        {
            var json = JsonConvert.SerializeObject(content);
            var finalUri = BuildFinalUriFromAbsolutePath(absolutePath);

            using (var client = new HttpClient())
            using (var _ = client.PutAsync(finalUri, new StringContent(json)).Result)
            { }
        }

        public T GetSync<T>(string absolutePath)
        {
            var finalUri = BuildFinalUriFromAbsolutePath(absolutePath);

            using (var client = new HttpClient())
            using (var response = client.GetAsync(finalUri).Result)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<T>(content);
            }
        }

        Uri BuildFinalUriFromAbsolutePath(string absolutePath)
        {
            if (!absolutePath.StartsWith("/"))
                absolutePath = "/" + absolutePath;

            if (absolutePath.EndsWith("/"))
                absolutePath = absolutePath.Substring(0, absolutePath.Length - 2);

            if (!absolutePath.EndsWith(".json"))
                absolutePath += ".json";

            var builder = new UriBuilder(Uri);
            builder.Path = string.Join("/", GetStringSegmentsWithoutTrailingDotJson()) + absolutePath;

            if (IsAuthed)
                builder.Query += "auth=" + AuthToken;

            return builder.Uri;
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
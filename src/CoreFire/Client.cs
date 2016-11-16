using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;

namespace CoreFire
{
    /// <summary>Builds `Client` objects via method chaining</summary>
    public class ClientBuilder
    {
        Uri uri;

        /* TODO: Implement authentication
        string authToken;
        */

        // We do not want any public ctors.
        internal ClientBuilder() { }

        /// <summary>Instantiates a `Client` with the settings you have chosen.</summary>
        public Client Build() => Client.New(uri/*, authToken*/);

        /// <param name="uri">The firebase URI of your db.</param>
        /// <c>ClientBuilder.New().WithUri("http://your-db.firebaseio.com");</c>
        public ClientBuilder WithUri(Uri uri)
        {
            this.uri = uri;
            return this;
        }

        /* TODO: The auth process has _completely_ changed.
        /* ref https://github.com/google/google-api-java-client/blob/dev/google-api-client/src/main/java/com/google/api/client/googleapis/auth/oauth2/GoogleCredential.java#L360
        /// <param name="authToken">The authentication token of your db.</param>
        /// <c>ClientBuilder.New().WithAuth("spqiQHnlwA6uS6Ur8H3ZrJinHbX951DzDySazIA");</c>
        public ClientBuilder WithAuth(string authToken)
        {
            this.authToken = authToken;
            return this;
        }
        */
    }

    /// <summary>Used to get the path resulting from a Push</summary>
    struct PushResponseObject
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class ClientException : Exception
    {
        internal ClientException(string message, Exception inner)
            : base(message, inner) { }
    }

    /// <summary>
    /// A client instance for pushing/setting/getting data to/from Firebase.<br/>
    /// To get started call `Client.Builder()` to create a ClientBuilder.
    /// </summary>
    public class Client
    {
        ///<summary>Will be used to store orderBy, etc.</summary>
        readonly Dictionary<string, string> requestParams = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public Uri Uri { get; private set; }

        /* TODO: Implement authentication
        public string AuthToken { get; private set; }
        public bool IsAuthed => !string.IsNullOrWhiteSpace(AuthToken);
        */

        public static ClientBuilder Builder() => new ClientBuilder();

        internal static Client New(Uri uri/*, string authToken*/) => new Client
        {
            Uri = uri,
            // AuthToken = authToken, // TODO: Implement authentication
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

        /// <summary>Push to an object in Firebase located at `absolutePath`</summary>
        /// <returns>Absolute path to the pushed `content` object in Firebase.</returns>
        /// <exception cref="ClientException">
        /// Thrown with inner <exception cref="ArgumentException"/> if <paramref name="absolutePath"/> is
        /// null, empty, or whitespace-only.
        /// </exception>
        public string PushSync(string absolutePath, object content)
        {
            if (string.IsNullOrWhiteSpace(absolutePath))
            {
                var message = nameof(absolutePath) + " must not be null, empty, or whitespace-only";
                var inner = new ArgumentException(message, nameof(absolutePath));
                throw new ClientException(message, inner);
            }

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

        /// <summary>Set an object in Firebase located at `absolutePath`</summary>
        /// <exception cref="ClientException">
        /// Thrown with inner <exception cref="ArgumentException"/> if <paramref name="absolutePath"/> is
        /// null, empty, or whitespace-only.
        /// </exception>
        public void SetSync(string absolutePath, object content)
        {
            if (string.IsNullOrWhiteSpace(absolutePath))
            {
                var message = nameof(absolutePath) + " must not be null, empty, or whitespace-only";
                var inner = new ArgumentException(message, nameof(absolutePath));
                throw new ClientException(message, inner);
            }

            var json = JsonConvert.SerializeObject(content);
            var finalUri = BuildFinalUriFromAbsolutePath(absolutePath);

            using (var client = new HttpClient())
            using (var _ = client.PutAsync(finalUri, new StringContent(json)).Result)
            { }
        }

        /// <summary>Get an object out of Firebase located at `absolutePath`</summary>
        /// <returns>An object of type `T`</returns>
        /// <exception cref="ClientException">
        /// Thrown with inner <exception cref="ArgumentException"/> if <paramref name="absolutePath"/> is
        /// null, empty, or whitespace-only.
        /// </exception>
        public T GetSync<T>(string absolutePath)
        {
            if (string.IsNullOrWhiteSpace(absolutePath))
            {
                var message = nameof(absolutePath) + " must not be null, empty, or whitespace-only";
                var inner = new ArgumentException(message, nameof(absolutePath));
                throw new ClientException(message, inner);
            }

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

            /* TODO: Implement authentication
            if (IsAuthed)
                builder.Query += "auth=" + AuthToken;
            */

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

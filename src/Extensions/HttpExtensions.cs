using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

// ReSharper disable MemberCanBePrivate.Global
namespace Gommon {
    public static partial class Extensions {
        /// <summary>
        ///     Checks whether the current <see cref="HttpResponseMessage"/> leads to an image, by checking its ContentType.
        /// </summary>
        /// <param name="msg">Current <see cref="HttpResponseMessage"/></param>
        /// <returns><see cref="bool"/></returns>
        public static bool IsImage([NotNull] this HttpResponseMessage msg)
            => msg.Content.Headers.ContentType.MediaType.StartsWith("image/");

        /// <summary>
        ///     Uses the current <see cref="HttpClient"/> to send a <code>POST</code>
        ///     request to the target <paramref name="url"/> with the JSON content in <paramref name="json"/>.
        /// </summary>
        /// <param name="http">The current HTTP Client.</param>
        /// <param name="url">The target URL</param>
        /// <param name="json">JSON content to send</param>
        /// <returns>The resulting <see cref="HttpResponseMessage"/>.</returns>
        public static Task<HttpResponseMessage> PostJsonAsync([NotNull] this HttpClient http, [NotNull] string url,
            [NotNull] string json)
            => http.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));
    }
}
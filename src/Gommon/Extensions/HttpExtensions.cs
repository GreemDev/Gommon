using System.Net.Http;

namespace Gommon
{
    public static partial class Extensions
    {
        /// <summary>
        ///     Checks whether or not the current <see cref="HttpResponseMessage"/> leads to an image, by checking its ContentType.
        /// </summary>
        /// <param name="msg">Current <see cref="HttpResponseMessage"/></param>
        /// <returns><see cref="bool"/></returns>
        public static bool IsImage(this HttpResponseMessage msg)
            => msg.Content.Headers.ContentType.MediaType.StartsWith("image/");
    }
}

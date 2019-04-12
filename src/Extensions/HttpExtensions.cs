using System.Net.Http;

namespace Gommon.Extensions
{
    public static class HttpExtensions
    {
        public static bool IsImage(this HttpResponseMessage msg)
            => msg.Content.Headers.ContentType.MediaType.StartsWith("image/");
    }
}

using System.Threading.Tasks;
using Discord;

namespace Gommon
{
    public static class EmbedExtensions
    {
        public static async Task<IUserMessage> SendToAsync(this EmbedBuilder e, IMessageChannel c) 
            => await c.SendMessageAsync(string.Empty, false, e.Build()).ConfigureAwait(false);

        public static async Task<IUserMessage> SendToAsync(this Embed e, IMessageChannel c)
            => await c.SendMessageAsync(string.Empty, false, e).ConfigureAwait(false);

        public static async Task<IUserMessage> SendToAsync(this EmbedBuilder e, IGuildUser u) 
            => await (await u.GetOrCreateDMChannelAsync().ConfigureAwait(false)).SendMessageAsync(string.Empty, false, e.Build()).ConfigureAwait(false);

        public static async Task<IUserMessage> SendToAsync(this Embed e, IGuildUser u)
            => await (await u.GetOrCreateDMChannelAsync().ConfigureAwait(false)).SendMessageAsync(string.Empty, false, e).ConfigureAwait(false);
    }
}

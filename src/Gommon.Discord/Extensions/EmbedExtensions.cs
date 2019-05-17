﻿using System.Threading.Tasks;
using Discord;

namespace Gommon
{
    public static class EmbedExtensions
    {
        public static Task<IUserMessage> SendToAsync(this EmbedBuilder e, IMessageChannel c) =>
            c.SendMessageAsync(string.Empty, false, e.Build());

        public static Task<IUserMessage> SendToAsync(this Embed e, IMessageChannel c) =>
            c.SendMessageAsync(string.Empty, false, e);

        public static async Task<IUserMessage> SendToAsync(this EmbedBuilder e, IGuildUser u) =>
            await (await u.GetOrCreateDMChannelAsync()).SendMessageAsync(string.Empty, false, e.Build());

        public static async Task<IUserMessage> SendToAsync(this Embed e, IGuildUser u) =>
            await (await u.GetOrCreateDMChannelAsync()).SendMessageAsync(string.Empty, false, e);
    }
}

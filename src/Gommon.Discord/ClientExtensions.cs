using Discord;

namespace Gommon.Discord
{
    public static class ClientExtensions
    {
        public static string GetInviteUrl(this IDiscordClient client, bool shouldHaveAdmin)
        {
            return shouldHaveAdmin is true
                ? $"https://discordapp.com/oauth2/authorize?client_id={client.CurrentUser.Id}&scope=bot&permissions=8"
                : $"https://discordapp.com/oauth2/authorize?client_id={client.CurrentUser.Id}&scope=bot&permissions=0";
        }
    }
}

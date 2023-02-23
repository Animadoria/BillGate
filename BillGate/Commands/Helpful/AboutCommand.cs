using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Emzi0767.Utilities;

namespace BillGate.Commands.Helpful;

public class AboutCommand : BaseCommandModule
{
    [Command]
    [Category(Constants.Help.HELPFUL)]
    public async Task About(CommandContext ctx)
    {
        DiscordEmbedBuilder embed = new DiscordEmbedBuilder
            {
            Author = new DiscordEmbedBuilder.EmbedAuthor
            {
                Name = "Bill Gate Bot",
                IconUrl = ctx.Client.CurrentUser.AvatarUrl
            },
            Color = DiscordColor.Blurple,
            Footer = new DiscordEmbedBuilder.EmbedFooter
            {
                Text = $"Bill Gate Bot · version {Constants.VERSION}"
            },
            Timestamp = DateTimeOffset.Now,
            Description =
                "Hello, I am the Bill Gate bot. No relation to Bill Gates. I'm just far more evil. IE is evil!"
        }.AddField("DSharpPlus", ctx.Client.VersionString, true)
         .AddField("OS", Environment.OSVersion.ToString(), true);


        await ctx.RespondAsync(new DiscordMessageBuilder()
                                  .WithEmbed(embed));
    }
}
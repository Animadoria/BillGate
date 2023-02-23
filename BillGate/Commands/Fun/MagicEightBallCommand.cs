using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace BillGate.Commands.Fun;

public class MagicEightBallCommand : BaseCommandModule
{
    private enum Type
    {
        Affirmative,
        NonCommittal,
        Negative
    }

    private static readonly Dictionary<Type, string[]> Replies = new()
    {
        [Type.Affirmative] = new[]
        {
            "{0}, it is certain.",
            "{0}, it is decidedly so.",
            "Without a doubt, {0}.",
            "Yes, {0}, definitely.",
            "{0}, you may rely on it.",
            "As I see it, {0}, yes.",
            "Most likely, {0}.",
            "Outlook good, {0}.",
            "Yes, {0}.",
            "{0}, signs point to yes."
        },
        [Type.NonCommittal] = new[]
        {
            "Reply hazy, {0}, try again.",
            "{0}, ask again later.",
            "Better not tell you now, {0}",
            "{0}, cannot predict now.",
            "{0}, concentrate and ask again.",
        },
        [Type.Negative] = new[]
        {
            "Don't count on it, {0}.",
            "{0}, my reply is no.",
            "My sources say no, {0}.",
            "Outlook not so good, {0}.",
            "{0}, very doubtful."
        }
    };

    [Command("8ball")]
    [Aliases("eightball")]
    [Description("Ask the Magic 8-Ball a question")]
    public async Task EightBall(CommandContext ctx,
                                [RemainingText, Description("The thing to ask the magic 8 ball")]
                                string query)
    {
        Type type = (Type)Random.Shared.Next(Replies.Count);
        string reply = Replies[type][Random.Shared.Next(Replies[type].Length)];

        DiscordEmbedBuilder embed = new DiscordEmbedBuilder()
        {
            Title = "🎱 Magic 8-Ball",
            Footer = new DiscordEmbedBuilder.EmbedFooter
            {
                Text = $"Requested by {ctx.User.Username}#{ctx.User.Discriminator}",
                IconUrl = ctx.User.AvatarUrl
            },
            Timestamp = DateTimeOffset.Now,
            Description = string.Format(reply, ctx.User.Mention),
            Color = type switch
            {
                Type.Affirmative => DiscordColor.Green,
                Type.NonCommittal => DiscordColor.Yellow,
                Type.Negative => DiscordColor.Red,
                _ => DiscordColor.Blurple
            }
        };

        await ctx.RespondAsync(new DiscordMessageBuilder()
                                  .WithEmbed(embed));
    }
}
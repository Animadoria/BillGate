using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using MarkovSharp.TokenisationStrategies;

namespace BillGate.Commands.Fun;

public class MarkovCommand : BaseCommandModule
{
    [Command, Aliases("evil")]
    [Category(Constants.Help.FUN)]
    public async Task Markov(CommandContext ctx)
    {
        var model = new StringMarkov(1);
        model.Learn(Program.Bot.Configuration.MarkovLines);

        await ctx.RespondAsync(model.Walk().First());
    }
}
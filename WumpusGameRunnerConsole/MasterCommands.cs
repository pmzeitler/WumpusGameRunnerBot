using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.Threading.Tasks;


namespace net.PhoebeZeitler.WumpusGameRunnerConsole
{
    /**
     * This is a list of base command handlers for all modules.
     **/
    public class MasterCommands
    {
        /**
        [Command("ping")] // let's define this method as a command
        [Description("Example ping command")] // this will be displayed to tell users what this command does when they invoke help
        [Aliases("pong")] // alternative names for the command
        public async Task Ping(CommandContext ctx) // this command takes no arguments
        {
            // let's trigger a typing indicator to let
            // users know we're working
            await ctx.TriggerTypingAsync();

            // let's make the message a bit more colourful
            var emoji = DiscordEmoji.FromName(ctx.Client, ":ping_pong:");

            // respond with ping
            await ctx.RespondAsync($"{emoji} Pong! Ping: {ctx.Client.Ping}ms");
        }
        */

        [Command("setupgame")]
        [Description("Sets up a game to run in this channel.\nSee listgametypes for a complete list of supported games.\nExample setupgame GoldenSkyStories")]
        [Aliases("setup_game,start_game")] 
        public async Task SetupGame(CommandContext ctx, [Description("The game type to run.")] string gametype) 
        {
            // let's trigger a typing indicator to let
            // users know we're working
            await ctx.TriggerTypingAsync();


            // respond with success
            await ctx.RespondAsync($"Done! Channel {Formatter.Bold(ctx.Channel.Name)} is now running a game of {Formatter.Bold(gametype)} with {ctx.User.Mention} as game runner.");
        }



    }
}

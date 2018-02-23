using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.Threading.Tasks;
using DSharpPlus.Exceptions;
using DSharpPlus.CommandsNext.Exceptions;

namespace net.PhoebeZeitler.WumpusGameRunnerConsole.GoldenSkyStoriesModule
{
    public class GoldenSkyStoriesModule : GameModuleBase
    {
        public GoldenSkyStoriesModule() : base()
        {

        }

        protected GSSModuleDataSource DataSource(CommandContext ctx)
        {
            ModuleDataSourceBase dataSource = this.ChannelData(ctx);
            if (dataSource == null)
            {
                throw new ChecksFailedException(ctx.Command, ctx, null);
            }
            else if (dataSource is GSSModuleDataSource)
            {
                return (GSSModuleDataSource)dataSource;
            }
            else
            {
                ctx.RespondAsync($"Sorry, I can't find any { Formatter.Bold(_moduleIdentifier.Name)} data for {Formatter.Bold(ctx.Channel.Name)}. This will probably throw an error.");
                throw new ChecksFailedException(ctx.Command, ctx, null);
            }
        }

        protected override async Task AddPlayer(CommandContext ctx, string[] arguments)
        {
            ModuleDataSourceBase dataSource = this.ChannelData(ctx);
            
            
        }

        protected override async Task ListPlayers(CommandContext ctx, string[] arguments)
        {
            DiscordUser gameRunner = await GetUserBySerialData(ctx, DataSource(ctx).GameRunnerID);
            await ctx.RespondAsync($"This channel's {Formatter.Bold(_moduleIdentifier.Name)} game is run by: {Formatter.Bold(gameRunner.Username)}");
        }

        protected override void RegisterDelegates()
        {
            //throw new NotImplementedException();
        }

        protected override async Task RemovePlayer(CommandContext ctx, string[] arguments)
        {
            throw new NotImplementedException();
        }

        protected override void SetupGameIdentifier()
        {
            this._moduleIdentifier = new SupportedGameIdentifier("Golden Sky Stories", "GSS",
                "Kamiya, Honpo, Cluney, and Gardner", "http://starlinepublishing.com/our-games/golden-sky-stories/",
                "Phoebe Zeitler (Bundled)", "https://github.com/pmzeitler/WumpusGameRunnerBot",
                new string[] { "goldenskystories", "goldensky", "gss", "yuuyakekoyake", "goldsky" });
        }

        protected override async Task SetupGame(CommandContext ctx, string[] arguments)
        {
            ModuleDataSourceBase saveMe = MasterDataSingleton.Instance.GetDataSource(ctx.Channel);
            if (saveMe == null || arguments.Contains("force"))
            {
                saveMe = new GSSModuleDataSource();
                saveMe.GameRunnerID = ctx.User.Id;
                MasterDataSingleton.Instance.SetDataSourceForChannel(ctx.Channel, saveMe, false);
                await ctx.RespondAsync($"Done! Channel {Formatter.Bold(ctx.Channel.Name)} is now hosting an instance of {Formatter.Bold(_moduleIdentifier.Name)}.");
            }
            else
            {
                await ctx.RespondAsync($"Another game is already running in {Formatter.Bold(ctx.Channel.Name)}. If you want to discard that game's data and start a new game, retry your command with the \"force\" argument.");
            }
        }

        protected override async Task SaveGame(CommandContext ctx, string[] arguments)
        {
            await MasterDataSingleton.Instance.SaveData(ctx.Channel);
        }

        protected override async Task LoadGame(CommandContext ctx, string[] arguments)
        {
            await MasterDataSingleton.Instance.LoadData(ctx.Channel, this._moduleIdentifier.ShortName);
        }
    }
}

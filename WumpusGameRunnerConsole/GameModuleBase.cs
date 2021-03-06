﻿using System;
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
    public abstract class GameModuleBase
    {
        protected delegate Task CommandType(CommandContext ctx, string[] arguments);

        private Dictionary<String, CommandType> _commandDelegates;
        protected SupportedGameIdentifier _moduleIdentifier;

        protected GameModuleBase()
        {
            this._commandDelegates = new Dictionary<string, CommandType>();
            this.RegisterBaseDelegates();
            this.RegisterDelegates();
            this.SetupGameIdentifier();
        }

        protected abstract void RegisterDelegates();
        protected abstract void SetupGameIdentifier();
        protected abstract Task AddPlayer(CommandContext ctx, string[] arguments);
        protected abstract Task RemovePlayer(CommandContext ctx, string[] arguments);
        protected abstract Task ListPlayers(CommandContext ctx, string[] arguments);
        protected abstract Task SetupGame(CommandContext ctx, string[] arguments);
        protected abstract Task SaveGame(CommandContext ctx, string[] arguments);
        protected abstract Task LoadGame(CommandContext ctx, string[] arguments);

        protected ModuleDataSourceBase ChannelData(CommandContext ctx)
        {
            ModuleDataSourceBase retval = MasterDataSingleton.Instance.GetDataSource(ctx.Channel);
            if (retval == null)
            {
                ctx.RespondAsync($"Sorry, I can't find any existing data for {Formatter.Bold(ctx.Channel.Name)}. This will probably throw an error.");
            }
            return retval;
        }

        public virtual async Task ProcessCommand(CommandContext ctx, string commandName, string[] arguments)
        {
            string checkName = commandName.ToLower();
            if (!_commandDelegates.ContainsKey(checkName))
            {
                await ctx.RespondAsync($"Sorry, but the command {Formatter.Bold(commandName)} is not supported for the game module {Formatter.Bold(_moduleIdentifier.Name)}.");
            }
            else
            {
                CommandType runMe = _commandDelegates[checkName];
                await runMe(ctx, arguments);
            }
        }

        protected async Task<DiscordUser> GetUserBySerialData(CommandContext ctx, ulong userId)
        {
            return await ctx.Client.GetUserAsync(userId);
        }
        protected async Task<DiscordChannel> GetChannelBySerialData(CommandContext ctx, ulong channelId)
        {
            return await ctx.Client.GetChannelAsync(channelId);
        }
        
        protected void RegisterDelegateWithAlias(CommandType commandDelegate, string commandName)
        {
            this._commandDelegates.Add(commandName.ToLower(), commandDelegate);
        }

        protected void RegisterDelegateWithAliases(CommandType commandDelegate, string[] aliases)
        {
            foreach (string commandName in aliases)
            {
                this._commandDelegates.Add(commandName.ToLower(), commandDelegate);
            }
        }
        
        private void RegisterBaseDelegates()
        {
            RegisterDelegateWithAliases(AddPlayer, new string[]{ "register_player", "reg_player", "add_player", "join_player", "regplayer", "registerplayer", "addplayer", "joinplayer" });
            RegisterDelegateWithAliases(RemovePlayer, new string[] { "remove_player", "drop_player", "leave_player", "removeplayer", "dropplayer", "leaveplayer" });
            RegisterDelegateWithAliases(ListPlayers, new string[] { "list_players", "whos_playing", "show_players", "listplayers", "whosplaying", "showplayers" });
            RegisterDelegateWithAliases(SetupGame, new string[] { "setup_game", "setupgame", "start_game", "startgame", "new_game", "newgame" });
            RegisterDelegateWithAliases(SaveGame, new string[] { "save_game", "savegame" });
            RegisterDelegateWithAliases(LoadGame, new string[] { "load_game", "loadgame" });
        }

    }
}

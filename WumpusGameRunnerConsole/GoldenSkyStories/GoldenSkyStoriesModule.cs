using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.Threading.Tasks;

namespace net.PhoebeZeitler.WumpusGameRunnerConsole.GoldenSkyStoriesModule
{
    public class GoldenSkyStoriesModule
    {
        private Dictionary<DiscordChannel, Dictionary<DiscordMember, String>> masterChannelList;
        


        [Command("reg_player"), Description("Register a character name."), Aliases("regplayer")]
        public async Task RegisterPlayer(CommandContext ctx, [Description("The character name for the player.")] string CharacterName)
        {
            // let's trigger a typing indicator to let
            // users know we're working
            await ctx.TriggerTypingAsync();

            DiscordMember user = ctx.Member;

            if (user == null)
            {
                await ctx.RespondAsync("You cannot register for a game via direct message. Please re-try your request in a channel.");
            }
            else
            {
                _RegisterPlayerInChannel(ctx.Channel, user, CharacterName);
                await ctx.RespondAsync($"Done! {user.DisplayName} is now registered for this game as {CharacterName}.");
            }
        }

        [Command("list_players"), Description("Returns the list of players registered for this game, and the characters they are playing."), Aliases("listp", "listplayers")]
        public async Task ListPlayers(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            DiscordChannel channel = ctx.Channel;

            if (channel == null)
            {
                await ctx.RespondAsync("You cannot list players for a game via direct message. Please re-try your request in a channel.");
            } else
            {
                Dictionary<DiscordMember, string> playerList = _GetPlayerList(channel);
                if (playerList.Count == 0)
                {
                    await ctx.RespondAsync("There is no Golden Sky Stories game currently running in this channel.");
                }
                else
                {
                    foreach (var item in playerList)
                    {
                        if (GSSModuleConfig.ListMentions)
                        {
                            await ctx.RespondAsync($"User {Formatter.Bold(item.Key.Mention)} is playing the character of {item.Value}.");
                        }
                        else
                        {
                            await ctx.RespondAsync($"User {Formatter.Bold(item.Key.DisplayName)} is playing the character of {item.Value}.");
                        }
                    }
                }
            }
        }


        [Command("list_characters"), Description("Returns the list of characters registered for this game, and the players who are playing them."), Aliases("listc", "listchars")]
        public async Task ListCharacters(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            DiscordChannel channel = ctx.Channel;

            if (channel == null)
            {
                await ctx.RespondAsync("You cannot list characters for a game via direct message. Please re-try your request in a channel.");
            }
            else
            {
                Dictionary<DiscordMember, string> playerList = _GetPlayerList(channel);
                if (playerList.Count == 0)
                {
                    await ctx.RespondAsync("There is no Golden Sky Stories game currently running in this channel.");
                }
                else
                {
                    foreach (var item in playerList)
                    {
                        if (GSSModuleConfig.ListMentions)
                        {
                            await ctx.RespondAsync($"Character {Formatter.Bold(item.Value)} is played by {item.Key.Mention}.");
                        }
                        else
                        {
                            await ctx.RespondAsync($"Character {Formatter.Bold(item.Value)} is played by {item.Key.DisplayName}.");
                        }
                    }
                }
            }
        }

        private Dictionary<DiscordMember, string> _GetPlayerList(DiscordChannel channel)
        {
            if (masterChannelList == null)
            {
                masterChannelList = new Dictionary<DiscordChannel, Dictionary<DiscordMember, string>>();
            }
            if (!masterChannelList.ContainsKey(channel))
            {
                masterChannelList.Add(channel, new Dictionary<DiscordMember, string>());
            }

            Dictionary<DiscordMember, string> registeredPlayers = masterChannelList[channel];
            return registeredPlayers;
        }

        private void _RegisterPlayerInChannel(DiscordChannel channel, DiscordMember user, string characterName)
        {
            Dictionary<DiscordMember, string> registeredPlayers = _GetPlayerList(channel);

            registeredPlayers[user] = characterName;
        }





    }
}

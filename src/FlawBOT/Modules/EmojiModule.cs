﻿using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Exceptions;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;
using FlawBOT.Common;
using FlawBOT.Properties;
using FlawBOT.Services;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlawBOT.Modules
{
    [SlashCommandGroup("emoji", "Slash command group for modal emoji commands.")]
    public class EmojiModule : ApplicationCommandModule
    {
        //[SlashCommand("create", "Add a new server emoji using a URL image.")]
        //[SlashRequirePermissions(Permissions.ManageEmojis)]
        //public async Task CreateEmoji(CommandContext ctx, [Option("url", "Image URL.")] Uri url, [Option("name", "Name for the emoji.")] string name)
        //{
        //    try
        //    {
        //        if (url is null)
        //        {
        //            if (!ctx.Message.Attachments.Any() ||
        //                !Uri.TryCreate(ctx.Message.Attachments[0].Url, UriKind.Absolute, out url))
        //                await BotServices.SendResponseAsync(ctx, Resources.ERR_EMOJI_IMAGE, ResponseType.Warning).ConfigureAwait(false);
        //            return;
        //        }

        //        if (string.IsNullOrWhiteSpace(name) || name.Length < 2 || name.Length > 50)
        //        {
        //            await BotServices.SendResponseAsync(ctx, Resources.ERR_EMOJI_NAME, ResponseType.Warning).ConfigureAwait(false);
        //            return;
        //        }

        //        var handler = new HttpClientHandler { AllowAutoRedirect = false };
        //        var http = new HttpClient(handler, true);
        //        var response = await http.GetAsync(url).ConfigureAwait(false);
        //        if (!response.Content.Headers.ContentType.MediaType.StartsWith("image/")) return;

        //        using (response = await http.GetAsync(url).ConfigureAwait(false))
        //        await using (var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
        //        {
        //            if (stream.Length >= 256000)
        //            {
        //                await BotServices.SendResponseAsync(ctx, Resources.ERR_EMOJI_SIZE, ResponseType.Warning).ConfigureAwait(false);
        //                return;
        //            }

        //            var emoji = await ctx.Guild.CreateEmojiAsync(name, stream).ConfigureAwait(false);
        //            await ctx.RespondAsync("Created the emoji " + Formatter.Bold(emoji.Name)).ConfigureAwait(false);
        //        }
        //    }
        //    catch
        //    {
        //        await BotServices.SendResponseAsync(ctx, Resources.ERR_EMOJI_ADD, ResponseType.Error).ConfigureAwait(false);
        //    }
        //}

        [SlashCommand("delete", "Remove a server emoji. Note: Bots can only delete emojis they created.")]
        [SlashRequirePermissions(Permissions.ManageEmojis)]
        public async Task DeleteEmoji(InteractionContext ctx, [Option("query", "Server emoji to delete.")] DiscordEmoji query)
        {
            try
            {
                var emoji = await ctx.Guild.GetEmojiAsync(query.Id).ConfigureAwait(false);
                await ctx.Guild.DeleteEmojiAsync(emoji).ConfigureAwait(false);
                await ctx.CreateResponseAsync("Deleted the emoji " + Formatter.Bold(emoji.Name)).ConfigureAwait(false);
            }
            catch (NotFoundException)
            {
                await BotServices.SendResponseAsync(ctx, Resources.NOT_FOUND_EMOJI, ResponseType.Missing)
                    .ConfigureAwait(false);
            }
        }

        [SlashCommand("rename", "Rename a server emoji.")]
        [SlashRequirePermissions(Permissions.ManageEmojis)]
        public async Task EditEmoji(InteractionContext ctx, [Option("query", "Emoji to rename.")] DiscordEmoji query, [Option("name", "New emoji name.")] string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    await BotServices.SendResponseAsync(ctx, Resources.ERR_EMOJI_NAME, ResponseType.Warning).ConfigureAwait(false);
                    return;
                }

                var emoji = await ctx.Guild.GetEmojiAsync(query.Id).ConfigureAwait(false);
                emoji = await ctx.Guild.ModifyEmojiAsync(emoji, name).ConfigureAwait(false);
                await ctx.CreateResponseAsync("Successfully renamed emoji to " + Formatter.Bold(emoji.Name)).ConfigureAwait(false);
            }
            catch (NotFoundException)
            {
                await BotServices.SendResponseAsync(ctx, Resources.NOT_FOUND_EMOJI, ResponseType.Missing).ConfigureAwait(false);
            }
        }

        [SlashCommand("info", "Retrieve server emoji information.")]
        public async Task GetEmojiInfo(InteractionContext ctx, [Option("query", "Server emoji.")] DiscordEmoji query)
        {
            var emoji = await ctx.Guild.GetEmojiAsync(query.Id).ConfigureAwait(false);
            var output = new DiscordEmbedBuilder()
                .WithDescription(emoji.Name + " (" + emoji.Guild.Name + ")")
                .AddField("Created by", (emoji.User is null ? "<unknown>" : emoji.User.Username) + " on " + emoji.CreationTimestamp.Date)
                .WithThumbnail(emoji.Url)
                .WithColor(Program.Settings.DefaultColor);
            await ctx.CreateResponseAsync(output.Build()).ConfigureAwait(false);
        }

        [SlashCommand("list", "Retrieve a list of server emojis.")]
        [SlashRequireBotPermissions(Permissions.ManageEmojis)]
        public async Task GetEmojiList(InteractionContext ctx)
        {

            //var output = new DiscordEmbedBuilder()
            //    .WithTitle($"{ctx.Guild.Name} Emojis")
            //    .WithThumbnail(ctx.Guild.IconUrl)
            //    .WithColor(Program.Settings.DefaultColor);

            var emojiList = new StringBuilder();
            foreach (var x in ctx.Guild.Emojis.Values)
                emojiList.Append($":{x.Name}: :x:").Append(!x.Equals(ctx.Guild.Emojis.Last().Value) ? ", " : string.Empty);
            //    output.AddField(x.Name, x.GetDiscordName(), true);
            //await ctx.CreateResponseAsync(output.Build()).ConfigureAwait(false);

            await BotServices.SendResponseAsync(ctx, emojiList.ToString()).ConfigureAwait(false);
        }
    }
}
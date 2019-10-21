﻿using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using FlawBOT.Framework.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FlawBOT.Framework.Services
{
    public class BotServices
    {
        public static async Task SendEmbedAsync(CommandContext ctx, string message, EmbedType type = EmbedType.Default)
        {
            var prefix = "";
            DiscordColor color;
            switch (type)
            {
                case EmbedType.Good:
                    color = DiscordColor.Green;
                    break;

                case EmbedType.Warning:
                    prefix = ":warning: ";
                    color = DiscordColor.Yellow;
                    break;

                case EmbedType.Missing:
                    prefix = ":mag: ";
                    color = DiscordColor.Wheat;
                    break;

                case EmbedType.Error:
                    prefix = ":no_entry: ";
                    color = DiscordColor.Red;
                    break;

                default:
                    color = new DiscordColor("#00FF7F");
                    break;
            }
            var output = new DiscordEmbedBuilder()
                .WithTitle(prefix + message)
                .WithColor(color);
            await ctx.RespondAsync(embed: output.Build());
        }

        public static bool CheckUserInput(string input)
        {
            return (string.IsNullOrWhiteSpace(input)) ? false : true;
        }

        public static bool CheckChannelName(string input)
        {
            return (string.IsNullOrWhiteSpace(input) || input.Length > 100) ? false : true;
        }

        public static int LimitToRange(int value, int min = 1, int max = 100)
        {
            if (value <= min) { return min; }
            if (value >= max) { return max; }
            return value;
        }

        public static async Task<bool> RemoveMessage(DiscordMessage message)
        {
            try
            {
                await message.DeleteAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static async Task<MemoryStream> CheckImageInput(CommandContext ctx, string input)
        {
            var stream = new MemoryStream();
            if (!Uri.TryCreate(input, UriKind.Absolute, out _) && (!input.EndsWith(".img") || !input.EndsWith(".png") || !input.EndsWith(".jpg")))
                await SendEmbedAsync(ctx, "An image URL ending with .img, .png or .jpg is required!", EmbedType.Warning).ConfigureAwait(false);
            else
            {
                using (var client = new WebClient())
                {
                    var results = client.DownloadData(input);
                    stream.Write(results, 0, results.Length);
                    stream.Position = 0;
                }
            }
            return stream;
        }

        public void LoadBotConfiguration()
        {
            var json = new StreamReader(File.OpenRead("config.json"), new UTF8Encoding(false)).ReadToEnd();
            TokenHandler.Tokens = JsonConvert.DeserializeObject<TokenData>(json);
        }
    }
}
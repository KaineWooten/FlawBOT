using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using FlawBOT.Common;
using FlawBOT.Core.Properties;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FlawBOT.Modules
{
    [Cooldown(3, 5, CooldownBucketType.Channel)]
    public class MiscModule : BaseCommandModule
    {
        #region COMMAND_COINFLIP

        [Command("coinflip")]
        [Aliases("coin", "flip")]
        [Description("Flip a coin")]
        public Task CoinFlip(CommandContext ctx)
        {
            var random = new Random();
            var output = new DiscordEmbedBuilder()
                .WithDescription(ctx.User.Mention + " flipped " + Formatter.Bold(Convert.ToBoolean(random.Next(0, 2)) ? "Heads" : "Tails"))
                .WithColor(SharedData.DefaultColor);
            return ctx.RespondAsync(embed: output.Build());
        }

        #endregion COMMAND_COINFLIP

        #region COMMAND_COLOR

        [Command("color")]
        [Aliases("clr")]
        [Description("Retrieve color values for a given HEX code")]
        public async Task GetColor(CommandContext ctx,
            [Description("HEX color code to process")] DiscordColor color)
        {
            var regex = new Regex(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$", RegexOptions.Compiled).Match(color.ToString());
            if (!regex.Success)
                await BotServices.SendEmbedAsync(ctx, Resources.ERR_COLOR_INVALID, EmbedType.Warning);
            else
            {
                var output = new DiscordEmbedBuilder()
                    .AddField("HEX:", $"{color.Value:X}", true)
                    .AddField("RGB:", $"{color.R} {color.G} {color.B}", true)
                    .AddField("Decimal:", color.Value.ToString(), true)
                    .WithColor(color);
                await ctx.RespondAsync(embed: output.Build());
            }
        }

        #endregion COMMAND_COLOR

        #region COMMAND_DICEROLL

        [Command("diceroll")]
        [Aliases("dice", "roll", "rolldice")]
        [Description("Roll a six-sided die")]
        public Task RollDice(CommandContext ctx)
        {
            var random = new Random();
            var output = new DiscordEmbedBuilder()
                .WithDescription(ctx.User.Mention + " rolled a " + Formatter.Bold(random.Next(1, 7).ToString()))
                .WithColor(SharedData.DefaultColor);
            return ctx.RespondAsync(embed: output.Build());
        }

        #endregion COMMAND_DICEROLL

        #region COMMAND_HELLO

        [Command("hello")]
        [Aliases("hi", "howdy")]
        [Description("Welcome another user to the server")]
        public async Task Greet(CommandContext ctx,
            [Description("User to say hello to")] [RemainingText] DiscordMember member)
        {
            if (member is null)
                await ctx.RespondAsync($":wave: Hello, " + ctx.User.Mention);
            else
                await ctx.RespondAsync($":wave: Welcome " + member.Mention + " to " + ctx.Guild.Name + ". Enjoy your stay!");
        }

        #endregion COMMAND_HELLO

        #region COMMAND_TTS

        [Command("tts")]
        [Description("Sends a text-to-speech message")]
        [RequirePermissions(Permissions.SendTtsMessages)]
        public Task SayTTS(CommandContext ctx,
            [Description("Text to convert to speech")] [RemainingText] string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return ctx.RespondAsync("I need something to say...");
            return ctx.RespondAsync(Formatter.BlockCode(Formatter.Strip(text)), isTTS: true);
        }

        #endregion COMMAND_TTS
    }
}
﻿using System;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using FlawBOT.Framework.Models;
using FlawBOT.Framework.Services;

namespace FlawBOT.Modules
{
    [Cooldown(3, 5, CooldownBucketType.Channel)]
    public class PokemonModule : BaseCommandModule
    {
        #region COMMAND_POKEMON

        [Command("pokemon")]
        [Aliases("poke")]
        [Description("Retrieve a Pokemon card")]
        public async Task Pokemon(CommandContext ctx,
            [Description("Name of the pokemon")] [RemainingText] string query)
        {
            var results = await PokemonService.GetPokemonCardsAsync(query);
            if (results.Cards.Count == 0)
                await BotServices.SendEmbedAsync(ctx, "Pokemon not found!", EmbedType.Missing);
            else
            {
                foreach (var value in results.Cards)
                {
                    var card = PokemonService.GetExactPokemonAsync(value.ID);
                    var output = new DiscordEmbedBuilder()
                        .WithTitle(card.Name + $" (PokeDex ID: {card.NationalPokedexNumber})")
                        .AddField("Subtype", card.SubType ?? "Unknown", true)
                        .AddField("Health Points", card.Hp ?? "Unknown", true)
                        .AddField("Artist", card.Artist ?? "Unknown", true)
                        .AddField("Rarity", card.Rarity ?? "Unknown", true)
                        .AddField("Series", card.Series ?? "Unknown", true)
                        .WithImageUrl(card.ImageUrlHiRes ?? card.ImageUrl)
                        .WithColor(DiscordColor.Gold)
                        .WithFooter("Type next in the next 10 seconds for the next card");

                    var types = new StringBuilder();
                    foreach (var type in card.Types)
                        types.Append(type);
                    output.AddField("Type(s)", types.ToString() ?? "Unknown", true);
                    await ctx.RespondAsync(embed: output.Build());

                    var interactivity = await ctx.Client.GetInteractivity().WaitForMessageAsync(m => m.Channel.Id == ctx.Channel.Id && m.Content.ToLowerInvariant() == "next", TimeSpan.FromSeconds(10));
                    if (interactivity.Result == null) break;
                    await BotServices.RemoveMessage(interactivity.Result);
                }
            }
        }

        #endregion COMMAND_POKEMON
    }
}
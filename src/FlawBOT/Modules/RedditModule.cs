﻿using DSharpPlus.SlashCommands;
using FlawBOT.Common;
using FlawBOT.Properties;
using FlawBOT.Services;
using System.Threading.Tasks;

namespace FlawBOT.Modules
{
    [SlashCommandGroup("reddit", "Slash command group for Reddit commands.")]
    public class RedditModule : ApplicationCommandModule
    {
        /// <summary>
        /// Returns hot-topics from Reddit.
        /// </summary>
        [SlashCommand("hot", "Returns hot-topics from Reddit.")]
        public Task RedditHot(InteractionContext ctx, [Option("query", "Subreddit")] string query)
        {
            return RedditPost(ctx, query, RedditCategory.Hot);
        }

        /// <summary>
        /// Returns new-topics from Reddit.
        /// </summary>
        [SlashCommand("new", "Returns new-topics from Reddit.")]
        public Task RedditNew(InteractionContext ctx, [Option("query", "Subreddit")] string query)
        {
            return RedditPost(ctx, query, RedditCategory.New);
        }

        /// <summary>
        /// Returns top-topics from Reddit.
        /// </summary>
        [SlashCommand("top", "Returns top-topics from Reddit.")]
        public Task RedditTop(InteractionContext ctx, [Option("query", "Subreddit")] string query)
        {
            return RedditPost(ctx, query, RedditCategory.Top);
        }

        private static async Task RedditPost(InteractionContext ctx, string query, RedditCategory category)
        {
            var output = RedditService.GetRedditResults(query, category);
            if (output is null)
            {
                await BotServices.SendResponseAsync(ctx, Resources.NOT_FOUND_COMMON, ResponseType.Missing).ConfigureAwait(false);
                return;
            }
            await ctx.CreateResponseAsync(output).ConfigureAwait(false);
        }
    }
}
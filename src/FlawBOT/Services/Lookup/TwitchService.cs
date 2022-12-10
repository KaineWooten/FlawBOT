﻿using System.Threading.Tasks;
using TwitchLib.Api;
using TwitchLib.Api.Helix.Models.Streams.GetStreams;

namespace FlawBOT.Services.Lookup
{
    public class TwitchService : HttpHandler
    {
        public static async Task<GetStreamsResponse> GetTwitchDataAsync(string clientId, string accessToken, string query)
        {
            var service = new TwitchAPI
            {
                Settings =
                {
                    ClientId = clientId,
                    AccessToken = accessToken,
                }
            };
            return await service.Helix.Streams.GetStreamsAsync(query).ConfigureAwait(false);
        }
    }
}
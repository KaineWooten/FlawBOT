﻿using FlawBOT.Models;
using FlawBOT.Properties;
using Newtonsoft.Json;
using System.Net;
using System.Threading.Tasks;

namespace FlawBOT.Services.Lookup
{
    public class DictionaryService : HttpHandler
    {
        public static async Task<DictionaryData> GetDictionaryDefinitionAsync(string query)
        {
            var results = await Http
                .GetStringAsync(string.Format(Resources.URL_Dictionary, WebUtility.UrlEncode(query.Trim())))
                .ConfigureAwait(false);
            return JsonConvert.DeserializeObject<DictionaryData>(results);
        }
    }
}
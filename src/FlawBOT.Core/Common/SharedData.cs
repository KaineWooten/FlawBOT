using DSharpPlus.Entities;
using Newtonsoft.Json;
using System;
using System.Reflection;

namespace FlawBOT.Common
{
    public class SharedData
    {
        public static string Name { get; } = "BrunoBOT";
        public static string Version { get; } = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        public static DiscordColor DefaultColor { get; set; } = new DiscordColor("#00FF7F");
        public static DateTime ProcessStarted { get; set; }
    }

    public class TokenHandler
    {
        public static TokenData Tokens { get; set; } = new TokenData();
    }

    public class TokenData
    {
        [JsonProperty("prefix")]
        public string CommandPrefix { get; private set; }

        [JsonProperty("discord")]
        public string DiscordToken { get; private set; }
    }

    public enum EmbedType
    {
        Default,
        Good,
        Warning,
        Missing,
        Error
    }
}
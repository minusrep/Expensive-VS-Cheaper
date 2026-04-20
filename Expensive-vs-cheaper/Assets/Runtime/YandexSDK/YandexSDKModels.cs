using Unity.Plastic.Newtonsoft.Json;

namespace DoubleB.Runtime.Runtime.YandexSDK
{
    public sealed class YandexFullscreenAdResult
    {
        [JsonProperty("wasShown")]
        public bool WasShown { get; set; }
    }

    public sealed class YandexRewardedAdResult
    {
        [JsonProperty("wasShown")]
        public bool WasShown { get; set; }

        [JsonProperty("rewarded")]
        public bool Rewarded { get; set; }
    }

    internal sealed class YandexSDKCallbackMessage
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("payload")]
        public string Payload { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }
    }

    internal sealed class YandexSDKAvailableMethodResult
    {
        [JsonProperty("available")]
        public bool Available { get; set; }
    }
}

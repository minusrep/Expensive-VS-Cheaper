using System;

namespace DoubleB.Runtime.Runtime.YandexSDK
{
    [Serializable]
    public sealed class YandexFullscreenAdResult
    {
        public bool wasShown;
    }

    [Serializable]
    public sealed class YandexRewardedAdResult
    {
        public bool wasShown;
        public bool rewarded;
    }

    [Serializable]
    internal sealed class YandexSDKCallbackMessage
    {
        public string id;
        public bool success;
        public string payload;
        public string error;
    }

    [Serializable]
    internal sealed class YandexSDKAvailableMethodResult
    {
        public bool available;
    }
}

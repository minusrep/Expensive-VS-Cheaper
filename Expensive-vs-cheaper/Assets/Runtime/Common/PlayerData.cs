using UniRx;
using Unity.Plastic.Newtonsoft.Json;

namespace DoubleB.Runtime.Runtime.Common
{
    public class PlayerData
    {
        [JsonIgnore]
        public ReactiveProperty<int> HighScore { get; } = new ReactiveProperty<int>();

        [JsonProperty("highScore")]
        public int SerializedHighScore
        {
            get => HighScore.Value;
            set => HighScore.Value = value < 0 ? 0 : value;
        }
    }
}
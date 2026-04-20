using DoubleB.Runtime.Runtime.Gameplay;
using DoubleB.Runtime.Runtime.Router;

namespace DoubleB.Runtime.Runtime.Common
{
    public class GameModel
    {
        public GameplayModel GameplayModel { get; set; }
        public UIRouterModel UIRouterModel { get; private set; }
        public PlayerData PlayerData { get; private set; }

        public string Language { get; } = "ru";

        public GameModel(UIRouterModel uiRouterModel, GameplayModel gameplayModel, PlayerData playerData = null)
        {
            UIRouterModel = uiRouterModel;
            GameplayModel = gameplayModel;
            PlayerData = playerData ?? new PlayerData();
        }
    }
}

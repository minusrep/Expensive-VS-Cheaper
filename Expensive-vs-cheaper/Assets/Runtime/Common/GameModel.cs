using DoubleB.Runtime.Runtime.Gameplay;
using DoubleB.Runtime.Runtime.Router;

namespace DoubleB.Runtime.Runtime.Common
{
    public class GameModel
    {
        public GameplayModel GameplayModel { get; set; }
        public UIRouterModel UIRouterModel { get; private set; }

        public GameModel(UIRouterModel uiRouterModel, GameplayModel gameplayModel)
        {
            UIRouterModel = uiRouterModel;
            GameplayModel = gameplayModel;
        }
    }
}
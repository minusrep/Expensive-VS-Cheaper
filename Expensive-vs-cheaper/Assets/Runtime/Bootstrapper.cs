using DoubleB.Runtime.Runtime.Common;
using DoubleB.Runtime.Runtime.Constants;
using DoubleB.Runtime.Runtime.Descriptions;
using DoubleB.Runtime.Runtime.Gameplay;
using DoubleB.Runtime.Runtime.Gameplay.ItemSequence;
using DoubleB.Runtime.Runtime.Router;
using DoubleB.Runtime.Runtime.ViewDescriptions;
using UnityEngine;
using UnityEngine.UIElements;

namespace DoubleB.Runtime.Runtime
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private UIDocument _uiDocument;
        [SerializeField] private UIAssetCollection _uiAssetCollection;
        [SerializeField] private DescriptionCollection _descriptionCollection;
        
        private GameFlowPresenter _gameFlowPresenter;
        
        private void Start()
        {
            var uiRouterModel = new UIRouterModel(UIConstants.Windows.MainMenu);
            var itemSequenceModel = new ItemSequenceModel(_descriptionCollection.ItemSequence);
            var gameplayModel = new GameplayModel(itemSequenceModel);
            
            
            var gameModel = new GameModel(uiRouterModel, gameplayModel);
            var gameView = new GameView(_uiDocument);
            
            var uiRouter = new UIRouterPresenter(gameModel, gameView,  _uiAssetCollection, _descriptionCollection);
            _gameFlowPresenter = new GameFlowPresenter(gameModel);
            
            _gameFlowPresenter.Enable();
            uiRouter.Enable();
        }
    }
}

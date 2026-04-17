using System.Linq;
using DoubleB.Runtime.Gameplay;
using DoubleB.Runtime.Runtime.Descriptions;
using DoubleB.Runtime.Runtime.ViewDescriptions;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace DoubleB.Runtime
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private UIDocument _uiDocument;
        [SerializeField] private UIAssetCollection _uiAssetCollection;
        [SerializeField] private ItemSequenceDescription _itemSequenceDescription;
        
        private void Start()
        {
            var uiRouterModel = new UIRouterModel(UIConstants.MainMenu);
            var gameModel = new GameModel(uiRouterModel);
            var gameView = new GameView(_uiDocument);
            var uiRouter = new UIRouterPresenter(gameModel, gameView,  _uiAssetCollection, _itemSequenceDescription);
            uiRouter.Enable();
        }
    }
}

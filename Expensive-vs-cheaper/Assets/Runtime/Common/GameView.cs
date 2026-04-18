using UnityEngine.UIElements;

namespace DoubleB.Runtime
{
    public class GameView
    {
        public VisualElement Root => _uiDocument.rootVisualElement;

        public UIWindowLayerView UIWindowLayerView { get; }
 
        public UIPopupLayerView UIPopupLayerView { get; }
        
        private readonly UIDocument _uiDocument;

        public GameView(UIDocument uiDocument)
        {
            _uiDocument = uiDocument;
            
            UIWindowLayerView = new UIWindowLayerView(_uiDocument.rootVisualElement.Q<VisualElement>(UIConstants.WindowLayer));
            UIPopupLayerView = new UIPopupLayerView(_uiDocument.rootVisualElement.Q<VisualElement>(UIConstants.PopupLayer));
        }
    }
}
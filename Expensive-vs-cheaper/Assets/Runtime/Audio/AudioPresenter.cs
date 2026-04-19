using Codice.CM.Common;
using DoubleB.Runtime.Runtime.Common;
using DoubleB.Runtime.Runtime.Constants;
using DoubleB.Runtime.Runtime.Core;
using DoubleB.Runtime.Runtime.Gameplay;
using DoubleB.Runtime.Runtime.ViewDescriptions;
using UnityEngine;

namespace DoubleB.Runtime.Runtime.Audio 
{
    public class AudioPresenter : IPresenter
    {
        private readonly AudioAssetCollection _audioAssetCollection;
        private AudioClip _clickSound;
        private AudioClip _successSound;
        private AudioClip _failureSound;
        
        private readonly AudioView _view;
        private readonly GameModel _model;

        public AudioPresenter(GameModel model, AudioView view, AudioAssetCollection audioAssetCollection)
        {
            _model = model;
            _view = view;
            _audioAssetCollection = audioAssetCollection;
        }

        public void Enable()
        {
            _clickSound = _audioAssetCollection.Get(UIConstants.Sounds.Click).Value;
            _successSound = _audioAssetCollection.Get(UIConstants.Sounds.Success).Value;
            _failureSound = _audioAssetCollection.Get(UIConstants.Sounds.Failure).Value;
            
            _model.UIRouterModel.PopupRouterModel.OnChangeState += PlaySound;
            _model.UIRouterModel.WindowRouterModel.OnChangeState += PlaySound;
            _model.GameplayModel.OnGetResult += PlayChoiceSound;
        }

        public void Disable()
        {
            _model.UIRouterModel.PopupRouterModel.OnChangeState -= PlaySound;
            _model.UIRouterModel.WindowRouterModel.OnChangeState -= PlaySound;
        }

        private void PlaySound()
        {
            _view.AudioSource.PlayOneShot(_clickSound);
        }

        private void PlayChoiceSound(ItemChoiceResult result)
        {
            _view.AudioSource.PlayOneShot(result == ItemChoiceResult.Success ? _successSound : _failureSound);
        }
    }
}
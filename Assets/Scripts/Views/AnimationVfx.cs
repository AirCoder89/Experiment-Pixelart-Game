using System;
using AirCoder.Controllers.Animations;
using AirCoder.Controllers.Animations.Components;
using AirCoder.Controllers.VisualEffects;
using AirCoder.Core;
using AirCoder.Helper;
using UnityEngine;
using Application = AirCoder.Core.Application;

namespace AirCoder.Views
{
    public class AnimationVfx : SpriteView, IVfx
    {
        public static event Action<IVfx> OnComplete; 
        
        private bool _isPlay;
        private GameAnimator _animator;
        public GameAnimator Animator => _animator;
        
        public AnimationVfx(Application inApp, string inName, string inLayerName) : base(inApp, inName, inLayerName)
        {
        }

        public void Play()
        {
            if (_animator == null) _animator = AddComponent(new GameAnimator(this, this.SpriteRenderer));
            _animator.RemoveEventsListener();
            _animator.OnCompleteEvent += AnimationCompleted;
            _animator.Play();
        }

        private void AnimationCompleted(AnimationData inAnimator)
        {
            OnComplete?.Invoke(this);
        }
        
        public void Remove()
        {
            _animator.Stop();
            SystemFacade.Pool.Despawn(this.gameObject.transform);
            SystemFacade.Pool.StoreGameView(this);
        }

        public Transform GetTransform()
        {
            return gameObject.transform;
        }
    }
}
using AirCoder.Extensions;
using AirCoder.Helper;
using UnityEngine;
using Application = AirCoder.Core.Application;

namespace AirCoder.Views
{
    public class GhostView : SpriteView
    {
        private SpriteView _targetView;
        private float _duration;
        public GhostView(Application inApp, string inLayerName) 
            : base(inApp, $"FX[{StringExtensions.GenerateString(8, true)}]", inLayerName)
        {
            gameObject.SetActive(false);
        }

        public void Start(SpriteView inView, float inDuration, Color inColor)
        {
            // --------------------------------------------------------------------------------
            // Setup variables
            // --------------------------------------------------------------------------------
            
            gameObject.SetActive(true);
            
            _duration = inDuration;
            _targetView = inView;
           
            SetSprite(_targetView.SpriteRenderer.sprite);
            SetOrderInLayerUnder(inView);
            Color = inColor;
            
            transform.localScale = _targetView.transform.localScale;
            SystemFacade.Level.Level.AddView(this, _targetView.transform.localPosition);
        }

        public override void Tick()
        {
            base.Tick();
            _duration -= Application.DeltaTime;
            SpriteRenderer.DecreaseAlpha(Application.DeltaTime);
            if (_duration <= 0f) Dispose();
        }

        public void Dispose()
        {
            SystemFacade.Level.Level.RemoveView(name, index);
            _targetView = null;
            gameObject.SetActive(false);
            SystemFacade.Pool.StoreGameView(this);
        }
    }
}
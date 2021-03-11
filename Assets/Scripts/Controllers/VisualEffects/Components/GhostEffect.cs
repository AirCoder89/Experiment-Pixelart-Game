using System;
using AirCoder.Core;
using AirCoder.Helper;
using AirCoder.Views;
using UnityEngine;
using Application = AirCoder.Core.Application;

namespace AirCoder.Controllers.VisualEffects.Components
{
    public class GhostEffect : GameComponent
    {
        private float _duration;
        private float _density;
        private Color _color;
        private float _counter;
        
        public GhostEffect(GameView inGameView, float inDuration, Color inColor, float inDensity) : base(inGameView)
        {
            if(!(inGameView is SpriteView)) throw new Exception($"Ghost effect component must attached to a SpriteView");
            _duration = inDuration;
            _density = inDensity;
            _color = inColor;
            _counter = 0f;

            SystemFacade.Level.AddComponent(this);
        }

        public override void Destroy()
        {
            SystemFacade.Level.RemoveComponent(this);
            base.Destroy();
        }

        public override void Tick()
        {
            base.Tick();
            if (_density > 0f)
            {
                _counter += Application.DeltaTime;
                if (!(_counter >= _density)) return;
                    _counter = 0f;
                    CreateClone();
            }
            else CreateClone();
        }

        private void CreateClone()
        {
            var fx = SystemFacade.Pool.GetConcreteGameView<GhostView>();
            var spView = gameView as SpriteView;
            if(fx != null)
            {
                fx.Start(spView, _duration, _color);
                return;
            }
            //- create new fx
            fx = new GhostView(SystemFacade.Application, spView.SortingLayer);
            fx.Start(spView, _duration, _color);
        }
    }
}
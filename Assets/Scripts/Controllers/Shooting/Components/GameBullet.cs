using AirCoder.Core;
using AirCoder.Helper;
using AirCoder.Views;
using UnityEngine;
using Application = AirCoder.Core.Application;

namespace AirCoder.Controllers.Shooting.Components
{
    public class GameBullet : GameComponent
    {//owner/dir/pos/data

        private Vector2 _direction;
        private float _speed;
        private bool _isRun;
        public GameBullet(GameView inGameView) : base(inGameView)
        {
            _isRun = false;
            SystemFacade.Shooting.AddComponent(this);
        }

        public override void Destroy()
        {
            _isRun = false;
            base.Destroy();
            SystemFacade.Shooting.RemoveComponent(this);
        }

        public void Launch(Vector2 inPosition, Vector2 inDirection, float inSpeed)
        {
            gameView.gameObject.transform.TransformDirection(_direction);
            gameView.gameObject.transform.position = inPosition;
            _speed = inSpeed;
            _direction = inDirection;
            _isRun = true;
        }

        public override void Pause()
        {
            _isRun = false;
        }

        public override void Tick()
        {
            base.Tick();
            if(!_isRun) return;
            gameView.gameObject.transform.Translate(_direction * (_speed * Application.DeltaTime));
        }
    }
}
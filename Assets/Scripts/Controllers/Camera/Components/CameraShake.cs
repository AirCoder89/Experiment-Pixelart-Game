using AirCoder.Core;
using AirCoder.Helper;
using AirCoder.Views;
using UnityEngine;
using Application = AirCoder.Core.Application;

namespace AirCoder.Controllers.Camera.Components
{
    public class CameraShake : GameComponent
    {
        private float _shakeDuration = 0f;
        private float _shakeAmount = 0.7f;
       // private float _decreaseFactor = 1.0f;
        private Vector3 _originalPos;
        private bool _isShake;
        private float _timeCounter;
        private float _decreaseFactor;
        
        public CameraShake(GameView inGameView) : base(inGameView)
        {
            _isShake = false;
        }

        public override void Destroy()
        {
            SystemFacade.Camera.RemoveComponent(this);
            base.Destroy();
        }

        public void Shake(float inDuration, float inShakeAmount, float inDecreaseFactor)
        {
            if(_isShake) return;
            _timeCounter = 0f;
            _originalPos = gameView.transform.localPosition;
            SystemFacade.Camera.AddComponent(this);
            _shakeAmount = inShakeAmount;
            _decreaseFactor = inDecreaseFactor;
            _shakeDuration = inDuration;
            _isShake = true;
            _isRun = true;
        }
        
        public override void Tick()
        {
            if(!_isRun || !_isShake) return;
            
            _timeCounter += Application.DeltaTime;
            if (_timeCounter >= _shakeDuration)
            {
                //shake complete
                SystemFacade.Camera.RemoveComponent(this);
                _isShake = false;
                _timeCounter = 0f;
                gameView.transform.localPosition = _originalPos;
            }
            else
            {
                //shaking
                var randomYPos = gameView.transform.localPosition.y + ((Random.Range(-1, 1) * _shakeAmount) - _decreaseFactor);
                gameView.transform.localPosition = new Vector3(gameView.transform.localPosition.x, randomYPos, gameView.transform.localPosition.z); 
            }
        }

    }
}
using AirCoder.Core;
using AirCoder.Helper;
using AirCoder.Views;
using UnityEngine;
using Application = AirCoder.Core.Application;

namespace AirCoder.Controllers.Camera.Components
{
    public class LookAt : GameComponent
    {
        private float _speed;
        private Transform _target;
        private bool _limitRotation;

        private Vector2 _vLimit;
        private Vector2 _hLimit;
        private Axis _axis;
        
        public LookAt(GameView inGameView, Transform inTarget,Axis inAxis, float inSpeed, bool inLimitRotation, Vector2 inHLimit, Vector2 inVLimit) : base(inGameView)
        {
            _axis = inAxis;
            _speed = inSpeed;
            _hLimit = inHLimit;
            _vLimit = inVLimit;
            _limitRotation = inLimitRotation;
            _target = inTarget;
            SystemFacade.Camera.AddComponent(this);
        }
        
        public override void Destroy()
        {
            base.Destroy();
            SystemFacade.Camera.RemoveComponent(this);
            _target = null;
        }
        
        public override void Tick() //Late Tick
        {
            base.Tick();
            if(_target == null )  return;
           
            var rot = Quaternion.LookRotation(_target.position - gameView.gameObject.transform.position).eulerAngles;
            
            switch (_axis)
            {
                case Axis.None : return;
                case Axis.Horizontal:
                {
                    rot.x = gameView.gameObject.transform.localRotation.eulerAngles.x;
                    if (_limitRotation && _hLimit != Vector2.zero) rot.y = Mathf.Clamp(rot.y, _hLimit.x, _hLimit.y);
                    rot.z = gameView.gameObject.transform.localRotation.eulerAngles.z;
                    break;
                }
                case Axis.Vertical:
                {
                    if (_limitRotation && _vLimit != Vector2.zero) rot.x = Mathf.Clamp(rot.x, _vLimit.x, _vLimit.y);
                    rot.y = gameView.gameObject.transform.localRotation.eulerAngles.y;
                    rot.z = gameView.gameObject.transform.localRotation.eulerAngles.z;
                    break;
                }
                case Axis.Both:
                {
                    if (_limitRotation)
                    {
                        if(_vLimit != Vector2.zero) rot.x = Mathf.Clamp(rot.x, _vLimit.x, _vLimit.y);
                        if(_hLimit != Vector2.zero) rot.y = Mathf.Clamp(rot.y, _hLimit.x, _hLimit.y);
                    }
                    rot.z = gameView.gameObject.transform.localRotation.eulerAngles.z;
                    break;
                }
            }
            
            var targetRotation = Quaternion.Euler(rot);
            gameView.gameObject.transform.rotation = Quaternion.Slerp(gameView.gameObject.transform.rotation, targetRotation, Application.DeltaTime * _speed);

            
        }

        

    }
}
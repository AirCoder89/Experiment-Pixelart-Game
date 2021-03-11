using System;
using AirCoder.Controllers.GamePhysics.Components;
using AirCoder.Core;
using AirCoder.Helper;
using AirCoder.Views;
using UnityEngine;
using Application = AirCoder.Core.Application;

namespace AirCoder.Controllers.Camera.Components
{
    public enum Axis { None, Horizontal , Vertical , Both }
    
    public class TargetFollow : GameComponent
    {
        private Axis _axis;
        private Vector2 _limit;
        private Vector3 _offset;
        private float _followSpeed;
        private float _targetSpeed;
        private Transform _target;

        private bool _hasLimit;
        public TargetFollow(GameView inGameView, Transform inTarget, Axis inAxis,Vector3 inOffset, float inSpeed, Vector2 inLimit) : base(inGameView)
        {
            _limit = inLimit;
            _hasLimit = inAxis == Axis.Both && (_limit != Vector2.zero);
            _targetSpeed = 0f;
            _target = inTarget;
            _axis = inAxis;
            _offset = inOffset;
            _followSpeed = inSpeed;
            SystemFacade.Camera.AddComponent(this);
        }

        public void SetTargetSpeed(float inTargetSpeed)
        {
            _targetSpeed = inTargetSpeed;
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
                var targetPos = GetTargetPos();
                var position = gameView.gameObject.transform.position;
                if (_hasLimit && (targetPos.y > _limit.y || targetPos.y < _limit.x))
                {
                    Log("Limited !!");
                    targetPos = new Vector3(targetPos.x, position.y, targetPos.z);
                };
                
                position = Vector3.Lerp(position, targetPos, (_followSpeed + _targetSpeed) * Application.DeltaTime);
                gameView.gameObject.transform.position = position;
                gameView.position = position;
        }

        private Vector3 GetTargetPos()
        {
            switch (_axis)
            {
                case Axis.None : return Vector3.zero;
                case Axis.Horizontal : 
                    var x = _target.position.x + _offset.x;
                    var y = gameView.gameObject.transform.position.y;
                    return new Vector3(x, y, gameView.gameObject.transform.position.z);
                case Axis.Vertical :
                    x = gameView.gameObject.transform.position.x;
                    y = _target.position.y + + _offset.y;
                    return new Vector3(x, y, gameView.gameObject.transform.position.z);
                case Axis.Both :
                    x = _target.position.x + _offset.x;
                    y = _target.position.y + + _offset.y;
                    return new Vector3(x, y, gameView.gameObject.transform.position.z);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        
    }
}
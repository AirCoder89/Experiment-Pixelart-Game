using AirCoder.Core;
using AirCoder.Helper;
using AirCoder.Views;
using UnityEngine;
using Application = AirCoder.Core.Application;

namespace AirCoder.Controllers.Camera.Components
{
    public class CameraFollow : GameComponent
    {
        private Vector3 _offset;
        private float _followSpeed;
        
        public CameraFollow(GameView inGameView, Vector3 inOffset, float inSpeed) : base(inGameView)
        {
            _offset = inOffset;
            _followSpeed = inSpeed;
            SystemFacade.Camera.AddComponent(this);
        }

        public override void Tick() //Late Tick
        {
            base.Tick();
            if(SystemFacade.Player.Player == null )  return;
            var x = SystemFacade.Player.Player.position.x + _offset.x;
            var y = SystemFacade.Player.Player.position.y + _offset.y;

            var targetPos = new Vector3(x, y, -10f + _offset.z);
            var position = gameView.gameObject.transform.position;
            position = Vector3.Slerp(position, targetPos, _followSpeed * Application.DeltaTime);
            gameView.gameObject.transform.position = position;
            gameView.position = position;
        }

        public override void Destroy()
        {
            base.Destroy();
            SystemFacade.Camera.RemoveComponent(this);
        }
    }
}
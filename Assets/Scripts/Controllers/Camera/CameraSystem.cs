using System;
using System.Linq;
using AirCoder.Controllers.Camera.Components;
using AirCoder.Core;
using AirCoder.Helper;
using AirCoder.Views;
using NaughtyAttributes;
using UnityEngine;
using Application = AirCoder.Core.Application;

namespace AirCoder.Controllers.Camera
{
    public class CameraSystem : GameSystem,ILateTick
    {
        public CameraView CameraView { get;}
        
        private TargetFollow _targetFollow;
        private readonly CameraSystemConfig _config;
        
        public CameraSystem(GameController inController, Application inApp, SystemConfig inConfig = null) : base(inController, inApp, inConfig)
        {
            // --------------------------------------------------------------------------------
            // Set system config
            // --------------------------------------------------------------------------------
            _config = inConfig as CameraSystemConfig;
            if(_config == null) throw new Exception($"System config must be not null.");
            
            // --------------------------------------------------------------------------------
            // Create Camera view
            // --------------------------------------------------------------------------------
            //cameraView = new CameraView(this.application, Camera.gameObject); //here if we want to set an existing camera from unity
            CameraView = new CameraView(this.application, "Camera", _config);
            ViewsContainer.AddView(CameraView);
            _targetFollow = null;
            // --------------------------------------------------------------------------------
            // Add Camera Shake Component
            // --------------------------------------------------------------------------------
            //Todo : Add camera shake
        }
        
        public void AddTargetToFollow(Transform inTarget)
        {
            // --------------------------------------------------------------------------------
            // Add Target Follow Component
            // --------------------------------------------------------------------------------
            _targetFollow = CameraView.AddComponent(new TargetFollow(CameraView, ViewFacade.Player.gameObject.transform, _config.followAxis,
                _config.offset, _config.followSpeed, _config.followLimit));
          //  cameraView.AddComponent(new LookAt(cameraView, inTarget,Axis.Horizontal, lookSpeed, true, horizontalLimit, verticalLimit));
        }

        public void UpdateTargetFollowSpeed(float inTargetSpeed)
        {
            _targetFollow.SetTargetSpeed(inTargetSpeed);
        }

        public void SetResolution(int inScreenWidth, int inScreenHeight, int inCurrentRes)
        {
            CameraView.camera.pixelRect = new Rect(0, 0, inScreenWidth, inScreenHeight);
            //cameraView.camera.orthographicSize = screenHeight / 2;
            CameraView.camera.transform.position = new Vector3(inScreenWidth / 2, -inScreenHeight / 2, -10f);
            
            //width  = inScreenWidth / inCurrentRes;
            //height = inScreenHeight / inCurrentRes;
           // camX = defaultRes.x - width/2;
           // camY = defaultRes.y - height/2;
        }

        public void LateTick()
        {
            if((!IsRun)) return;
            foreach (var component in _components.ToList())
                component.Tick();
        }

       
    }
}
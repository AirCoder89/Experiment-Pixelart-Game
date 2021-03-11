using System;
using AirCoder.Controllers.Camera;
using AirCoder.Controllers.Camera.Components;
using AirCoder.Core;
using UnityEngine;
using Application = AirCoder.Core.Application;

namespace AirCoder.Views
{
    public class CameraView : GameView
    {
        public Camera camera { get; }
        private CameraShake _shake { get; }

        private CameraSystemConfig _config;
        
        public CameraView(Application inApp, GameObject inGameObject) : base(inApp, inGameObject)
        {
            camera = gameObject.GetComponent<Camera>();
            if(camera == null) throw new Exception($"target Camera view Component must have a Camera");

            _shake = AddComponent(new CameraShake(this));
        }

        public CameraView(Application inApp, string inName, CameraSystemConfig inConfig) : base(inApp, inName)
        {
            // --------------------------------------------------------------------------------
            // Create and setup new camera
            // --------------------------------------------------------------------------------
            _config = inConfig;
            camera = gameObject.AddComponent<Camera>();
            camera.tag = _config.tag;
            camera.backgroundColor = _config.backgroundColor;
            camera.clearFlags = _config.clearFlag;
            camera.orthographic = true;
            camera.orthographicSize = _config.orthographicSize;
            
            _shake = AddComponent(new CameraShake(this));
        }

        public void ShakeCamera()
        {
            _shake?.Shake(_config.shakeDuration, _config.shakeAmount, _config.shakeDecreaseAmount); 
        }
      
       
    }
}
using AirCoder.Core;
using AirCoder.Helper;
using UnityEngine;
using Application = AirCoder.Core.Application;

namespace AirCoder.Controllers
{
    public class RendererSystem : GameSystem //Gfx
    {
        public Vector3 ResetVector { get; private set; }
        public Vector3 CurrentResetVector { get; private set; }
        public int ScreenWidth { get; private set; }
        public int ScreenHeight { get; private set; }
        public int CurrentResolution { get; private set; }
        
        public RendererSystem(GameController inController, Application inApp, SystemConfig inConfig = null) : base(inController, inApp, inConfig)
        {
            // --------------------------------------------------------------------------------
            // Setup Screen Resolution
            // --------------------------------------------------------------------------------
            ScreenWidth = application.parameters.resolution.width;
            ScreenHeight = application.parameters.resolution.height;
            
            ResetVector = new Vector3(1.0f, 1.0f, 1.0f);
            
            CurrentResolution = Mathf.CeilToInt(ScreenHeight / 267F);
            var tmpRes = Mathf.CeilToInt (ScreenWidth / 440F);
            if (tmpRes > CurrentResolution) CurrentResolution = tmpRes;
            
            //CurrentResetVector  = new Vector3(CurrentResolution, CurrentResolution, 1.0f);
            CurrentResetVector  = ResetVector;
            
            // --------------------------------------------------------------------------------
            // Setup Camera
            // --------------------------------------------------------------------------------
            SystemFacade.Camera.SetResolution(ScreenWidth, ScreenHeight, CurrentResolution);
        }
        
    }
}
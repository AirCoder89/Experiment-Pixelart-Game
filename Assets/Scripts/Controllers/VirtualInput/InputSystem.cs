using System;
using AirCoder.Core;
using UnityEngine;
using Application = AirCoder.Core.Application;

namespace AirCoder.Controllers.VirtualInput
{
    public class InputSystem : GameSystem
    {
        private readonly InputSystemConfig _config;
        
        public InputSystem(GameController inController, Application inApp, SystemConfig inConfig = null) : base(inController, inApp, inConfig)
        {
            _config = inConfig as InputSystemConfig;
            if(_config == null) throw new Exception($"System config must be not null.");
            _config.Initialize();
        }
        
        public bool IsShiftButton() =>  IsRun && _config && Input.GetButton(_config.shift);
        public bool IsShiftButtonDown() =>  IsRun && _config && Input.GetButtonDown(_config.shift);
        public bool IsShiftButtonUp() =>  IsRun && _config && Input.GetButtonUp(_config.shift);
        
        public float GetAxis(InputBehaviour inBehaviour)
        {
            if (!IsRun || !_config) return 0f;
            return Input.GetAxisRaw(_config.GetBehaviourAxis(inBehaviour));
        }

        public bool GetButton(InputBehaviour inBehaviour) =>  IsRun && _config && Input.GetButton(_config.GetBehaviourAxis(inBehaviour));
        public bool GetButtonDown(InputBehaviour inBehaviour) => IsRun && _config && Input.GetButtonDown(_config.GetBehaviourAxis(inBehaviour));
        public bool GetButtonUp(InputBehaviour inBehaviour) => IsRun && _config && Input.GetButtonUp(_config.GetBehaviourAxis(inBehaviour));

    }
}
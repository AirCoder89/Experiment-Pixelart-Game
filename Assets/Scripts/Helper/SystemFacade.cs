using AirCoder.Controllers;
using AirCoder.Controllers.AI;
using AirCoder.Controllers.Animations;
using AirCoder.Controllers.Camera;
using AirCoder.Controllers.GamePhysics;
using AirCoder.Controllers.Level;
using AirCoder.Controllers.NPC;
using AirCoder.Controllers.ObjectsPooling;
using AirCoder.Controllers.Player;
using AirCoder.Controllers.Shooting;
using AirCoder.Controllers.Sounds;
using AirCoder.Controllers.VirtualInput;
using AirCoder.Controllers.VisualEffects;
using AirCoder.Core;
using UnityEngine;
using Application = AirCoder.Core.Application;

namespace AirCoder.Helper
{
    public static class SystemFacade
    {
        private static Application _application;

        // --------------------------------------------------------------------------------
        // Facade Pattern & Dependencies Container
        // --------------------------------------------------------------------------------
        public static void Initialize(Application inApp)
        {
            _application = inApp;
        }
        
        //- Parameters
        public static Application Application => _application;
        public static AppParameters Parameters => _application.parameters;
        
        //- Camera
        private static CameraSystem _camera;
        public static CameraSystem Camera
        {
            get
            {
                if (_camera == null) _camera = _application.GetSystem<CameraSystem>();
                return _camera;
            }
        }
        
        //- Renderer
        private static RendererSystem _renderer;
        public static RendererSystem Renderer
        {
            get
            {
                if (_renderer == null) _renderer = _application.GetSystem<RendererSystem>();
                return _renderer;
            }
        }
        
        //- Level
        private static LevelSystem _level;
        public static LevelSystem Level
        {
            get
            {
                if (_level == null) _level = _application.GetSystem<LevelSystem>();
                return _level;
            }
        }
        
        //- Resources System
        private static ResourcesSystem _res;
        public static ResourcesSystem Resources
        {
            get
            {
                if (_res == null) _res = _application.GetSystem<ResourcesSystem>();
                return _res;
            }
        }
        
        //- Physics System
        private static PhysicsSystem _physics;
        public static PhysicsSystem Physics
        {
            get
            {
                if (_physics == null) _physics = _application.GetSystem<PhysicsSystem>();
                return _physics;
            }
        }
        
        //- Player System
        private static PlayerSystem _player;
        public static PlayerSystem Player
        {
            get
            {
                if (_player == null) _player = _application.GetSystem<PlayerSystem>();
                return _player;
            }
        }
        
        //- Input System
        private static InputSystem _input;
        public static InputSystem Input
        {
            get
            {
                if (_input == null) _input = _application.GetSystem<InputSystem>();
                return _input;
            }
        }
        
        //- NPC System
        private static NpcSystem _npc;
        public static NpcSystem NPC
        {
            get
            {
                if (_npc == null) _npc = _application.GetSystem<NpcSystem>();
                return _npc;
            }
        }
        
        //- Animation System
        private static AnimationSystem _anim;
        public static AnimationSystem Animations
        {
            get
            {
                if (_anim == null) _anim = _application.GetSystem<AnimationSystem>();
                return _anim;
            }
        }
        
        //- Sounds System
        private static SoundSystem _sounds;
        public static SoundSystem Sounds
        {
            get
            {
                if (_sounds == null) _sounds = _application.GetSystem<SoundSystem>();
                return _sounds;
            }
        }
        
        //- Pool System
        private static PoolSystem _pool;
        public static PoolSystem Pool
        {
            get
            {
                if (_pool == null) _pool = _application.GetSystem<PoolSystem>();
                return _pool;
            }
        }
        
        //- VFX System
        private static VfxSystem _vfx;
        public static VfxSystem VFX
        {
            get
            {
                if (_vfx == null) _vfx = _application.GetSystem<VfxSystem>();
                return _vfx;
            }
        }
        
        //- Shooting System
        private static ShootingSystem _shooting;
        public static ShootingSystem Shooting
        {
            get
            {
                if (_shooting == null) _shooting = _application.GetSystem<ShootingSystem>();
                return _shooting;
            }
        }
        
        //- AI System
        private static AISystem _ai;
        public static AISystem AI
        {
            get
            {
                if (_ai == null) _ai = _application.GetSystem<AISystem>();
                return _ai;
            }
        }
        
    }
}
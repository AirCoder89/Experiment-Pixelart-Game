using System;
using System.Collections;
using System.Collections.Generic;
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
using AirCoder.Extensions;
using AirCoder.Helper;
using AirCoder.Views;
using NaughtyAttributes;
using PathologicalGames;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AirCoder.Core
{
    public enum GameStatus
    {
        Idle = 0,
        Play = 1,
        Pause = 2
    }
    
    public class Application : MonoBehaviour
    {
        public ViewsContainer ViewsContainer { get; private set; }
        public static float DeltaTime { get; private set; }
        public static float FixedDeltaTime { get; private set; }
        
        [BoxGroup("Establishing Parameters")] [Required] public SpawnPool spawnPool;
        [BoxGroup("Establishing Parameters")] [Required][Expandable] public AppParameters parameters;
       
        [BoxGroup("System Config")][Required] [SerializeField][Expandable] private SystemConfig cameraConfig;
        [BoxGroup("System Config")][Required] [SerializeField][Expandable] private SystemConfig inputConfig;
        [BoxGroup("System Config")][Required] [SerializeField][Expandable] private SystemConfig soundConfig;
        [BoxGroup("System Config")][Required] [SerializeField][Expandable] private SystemConfig vfxConfig;
        [BoxGroup("System Config")][Required] [SerializeField][Expandable] private SystemConfig shootingConfig;
        [BoxGroup("System Config")][Required] [SerializeField][Expandable] private SystemConfig levelConfig;
        [BoxGroup("System Config")][Required] [SerializeField][Expandable] private SystemConfig playerConfig;
        [BoxGroup("System Config")][Required] [SerializeField][Expandable] private SystemConfig npcConfig;

        [Foldout("Tracking Data & Dependencies")][ReadOnly] public bool run;
        [Foldout("Tracking Data & Dependencies")][ReadOnly] public Level currentLevel;
        [Foldout("Tracking Data & Dependencies")][ReadOnly] public GameStatus gameStatus;
        
        private GameController _controllers;
        private long _lastTime = 0;
        private static Application _instance;
        
        private void Awake()
        {
            // --------------------------------------------------------------------------------
            // Application Singleton
            // --------------------------------------------------------------------------------
            if (_instance != null)  return;
            _instance = this;
        }
        
        private void Start()
        {
            // --------------------------------------------------------------------------------
            // Game Entry Point
            // --------------------------------------------------------------------------------
            InitializeHelpers();
            ApplyGameSettings();
            Initialize();
            
            StartGame();
        }

        private void InitializeHelpers()
        {
            // --------------------------------------------------------------------------------
            // Create and Initialize Helper classes
            // --------------------------------------------------------------------------------
            ViewsContainer = new ViewsContainer(this);
            ObjectMap.Initialize(this);
            SystemFacade.Initialize(this);
            ViewFacade.Initialize(this);
        }

        private void ApplyGameSettings()
        {
            // --------------------------------------------------------------------------------
            // Setup game settings
            // --------------------------------------------------------------------------------
            parameters.resolution.AssignFromScreenSize();
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            UnityEngine.Application.targetFrameRate = parameters.frameRate;
            Cursor.visible = parameters.cursorVisibility;
            
            QualitySettings.vSyncCount = parameters.vSyncCount;
            if (Screen.currentResolution.refreshRate > 65) {
                QualitySettings.vSyncCount = Mathf.FloorToInt(Screen.currentResolution.refreshRate/60);
            }
        }
        
        private void Initialize()
        {
            // --------------------------------------------------------------------------------
            // Create the GameController and add the necessary systems (the order is matter !)
            // --------------------------------------------------------------------------------
            _controllers = new GameController();
            _controllers
                .AddSystem(new CameraSystem(_controllers, this, cameraConfig))
                .AddSystem(new RendererSystem(_controllers, this, null))
                .AddSystem(new ResourcesSystem(_controllers, this, null))
                .AddSystem(new PhysicsSystem(_controllers, this, null))
                .AddSystem(new AnimationSystem(_controllers, this, null))
                .AddSystem(new InputSystem(_controllers, this, inputConfig))
                .AddSystem(new SoundSystem(_controllers, this, soundConfig))
                .AddSystem(new PoolSystem(_controllers, this, null))
                .AddSystem(new VfxSystem(_controllers, this, vfxConfig))
                .AddSystem(new ShootingSystem(_controllers, this, shootingConfig))
                .AddSystem(new AISystem(_controllers, this, null))
                .AddSystem(new LevelSystem(_controllers, this, levelConfig))
                .AddSystem(new PlayerSystem(_controllers, this, playerConfig))
                .AddSystem(new NpcSystem(_controllers, this, npcConfig));
        }
        
        public T GetSystem<T>() where T : GameSystem
        {
            return _controllers.GetSystem<T>();
        }
    
        [Button("Start Game")]
        private void StartGame()
        {
            currentLevel = SystemFacade.Level.Level;
            
            // --------------------------------------------------------------------------------
            // Generate Enemies
            // --------------------------------------------------------------------------------
            SystemFacade.NPC.GenerateEnemies();
            
            // --------------------------------------------------------------------------------
            // start playing music background
            // --------------------------------------------------------------------------------
            SystemFacade.Sounds.PlayMusic(MusicThemes.Mission);
            
            // --------------------------------------------------------------------------------
            // Start Systems
            // --------------------------------------------------------------------------------
            _controllers.Start();
            gameStatus = GameStatus.Play;
            run = true;
        }
        
        [Button("Pause App")]
        private void PauseApp()
        {
            gameStatus = GameStatus.Pause;
            _controllers.Pause();
            currentLevel?.Pause();
            run = false;
        }
        
        [Button("Resume App")]
        private void ResumeApp()
        {
            gameStatus = GameStatus.Play;
            run = true;
            _controllers.Resume();
            currentLevel?.Resume();
        }
        
        [Button("Reset App")]
        private void ResetApp()
        {
            gameStatus = GameStatus.Play;
            _controllers.Reset();
            currentLevel?.ResetView();
        }
    
        private void Update()
        {
            if(!run) return;
            DeltaTime = Time.deltaTime;
            _controllers.Tick();
            currentLevel?.Tick();
        }

        private void LateUpdate()
        {
            if(!run) return;
            _controllers.LateTick();
        }

        private void FixedUpdate()
        {
            if(!run) return;
            FixedDeltaTime = Time.fixedDeltaTime;
            _controllers.FixedTick();
        }

        public static bool CanDebug(Type inType = null)
        {
            // --------------------------------------------------------------------------------
            // Debug permissions
            // --------------------------------------------------------------------------------
            if (!_instance.parameters.debuggingFilter) return true;
            if (inType.IsSubclassOf(typeof(GameSystem))) return _instance.parameters.debugSystems;
            if (inType.IsSubclassOf(typeof(GameView))) return _instance.parameters.debugViews;
            if (inType.IsSubclassOf(typeof(GameComponent))) return _instance.parameters.debugComponents;
            if (inType.IsAssignableFrom(typeof(IHelper))) return  _instance.parameters.helperClasses;
            return _instance.parameters.debugOthers;
        }
    }
}

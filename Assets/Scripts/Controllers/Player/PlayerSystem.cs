using System;
using AirCoder.Controllers.Animations.Components;
using AirCoder.Controllers.GamePhysics.Components;
using AirCoder.Controllers.Player.Components;
using AirCoder.Controllers.VirtualInput;
using AirCoder.Controllers.VisualEffects.Components;
using AirCoder.Core;
using AirCoder.Helper;
using AirCoder.Views;
using NaughtyAttributes;
using UnityEngine;
using Application = AirCoder.Core.Application;
using Object = UnityEngine.Object;

namespace AirCoder.Controllers.Player
{
    public class PlayerSystem : GameSystem,IFixedTick
    {
        public PlayerView Player { get; private set;}
        public Sprite[] Sprites { get; private set; }
        
        private readonly InputSystem _input;
        private readonly PlayerSystemConfig _config;
        
        public PlayerSystem(GameController inController, Application inApp, SystemConfig inConfig = null) : base(inController, inApp, inConfig)
        {
            _config = inConfig as PlayerSystemConfig;
            if(_config == null) throw new Exception($"System config must be not null.");
            
            Sprites = null;
            Player = CreatePlayer();
            Player.OnTakeDamage += OnPlayerTakeDamage;
            Player.Destroyed += OnPlayerDie;
            _input = SystemFacade.Input;
        }

        private void OnPlayerDie(GameView inPlayer)
        {
           SystemFacade.Camera.CameraView.ShakeCamera();
           
            var dieSide = Player.LastHitDirection == Player.Controller.GetDirection()
                ? ActorActions.DieFront
                : ActorActions.DieBack;
            Log("OnPlayerDie " + dieSide);
            Player.Controller.DoDie(dieSide);
            SystemFacade.NPC.StopAllEnemies();
        }

        public void RemovePlayer()
        {
            Player.OnTakeDamage -= OnPlayerTakeDamage;
            Player.Destroyed -= OnPlayerDie;
            SystemFacade.Level.Level.RemoveView(Player.name, Player.index);
            ViewsContainer.RemoveView(Player);
            Object.Destroy(Player.gameObject);
            Player = null;
            
            Log("Game is over !!");
        }

        private void OnPlayerTakeDamage(GameView inPlayer, Vector2 inDirection)
        {
            
        }

        public PlayerView CreatePlayer()
        {
            // --------------------------------------------------------------------------------
            // Setup Player
            // --------------------------------------------------------------------------------
            //here we can add the ability to select a specific player (if we have more than one)
            Sprites = SystemFacade.Resources.LoadSpriteAtlas(ResourcesMap.Players);
            var player = new PlayerView(this.application, "Player", LayerNames.Player, _config.profile);
            player.SetSprite(Sprites[22]);
            SystemFacade.Level.Level.AddView(player, _config.startPosition);
            ViewsContainer.AddView(player);
            
            // --------------------------------------------------------------------------------
            // Setup Physics (Add Collider Component)
            // --------------------------------------------------------------------------------
            var boxSize = new Vector2(Sprites[22].bounds.size.x, Sprites[22].bounds.size.y);
            player.AddComponent(new HitBox(player, boxSize, _config.profile.mass));
            
            // --------------------------------------------------------------------------------
            // Setup Animation (Add Animator Component)
            // --------------------------------------------------------------------------------
            player.profile.SetAnimationsSource(this.Sprites); //set sprite sheet source to all animations
            player.AddComponent(new GameAnimator(player, player.SpriteRenderer));
            
            // --------------------------------------------------------------------------------
            // Setup Player Controller (Add Control View Component)
            // --------------------------------------------------------------------------------
            player.AddComponent(new ControlView(player, _config.profile));

            // --------------------------------------------------------------------------------
            // Add Parallax Effect to background
            // --------------------------------------------------------------------------------
             SystemFacade.Level.AddParallaxEffect(player);
            
            // --------------------------------------------------------------------------------
            // Setup Camera follow
            // --------------------------------------------------------------------------------
            SystemFacade.Camera.AddTargetToFollow(player.gameObject.transform);

            return player;
        }

        public void FixedTick()
        {
            if(Player == null || !IsRun) return;
                Player.Controller?.FixedTick();
        }
        
        public override void Tick()
        {
            if(Player == null || !IsRun || Player.Controller == null || !Player.IsAlive) return;

            // --------------------------------------------------------------------------------
            // Notify player controller Inputs
            // --------------------------------------------------------------------------------
            
            //- Movement
            if (_input.IsShiftButton())
            {
                SystemFacade.Camera.UpdateTargetFollowSpeed(_config.profile.quickMoveSpeed);
                Player.Controller.QuickMove(_input.GetAxis(InputBehaviour.Move));
            }
            else
            {
                SystemFacade.Camera.UpdateTargetFollowSpeed(_config.profile.normalMoveSpeed);
                Player.Controller.NormalMove(_input.GetAxis(InputBehaviour.Move));
            } 
            
            //- Jump
            if (_input.GetButton(InputBehaviour.Jump)) Player.Controller.Jump(ButtonState.Button);
            if (_input.GetButtonDown(InputBehaviour.Jump)) Player.Controller.Jump(ButtonState.ButtonDown);
            if (_input.GetButtonUp(InputBehaviour.Jump)) Player.Controller.Jump(ButtonState.ButtonUp);
            
            //- Duck
            if (_input.GetButton(InputBehaviour.Duck)) Player.Controller.Duck(ButtonState.Button);
            if (_input.GetButtonDown(InputBehaviour.Duck)) Player.Controller.Duck(ButtonState.ButtonDown);
            if (_input.GetButtonUp(InputBehaviour.Duck)) Player.Controller.Duck(ButtonState.ButtonUp);
            
            //- Shoot
            if (_input.GetButton(InputBehaviour.Shoot)) Player.Controller.Shoot(ButtonState.Button);
            if (_input.GetButtonDown(InputBehaviour.Shoot)) Player.Controller.Shoot(ButtonState.ButtonDown);
            if (_input.GetButtonUp(InputBehaviour.Shoot)) Player.Controller.Shoot(ButtonState.ButtonUp);
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using AirCoder.Controllers.AI.Components.FSM;
using AirCoder.Controllers.AI.Components.FSM.EnemyAI.Actions;
using AirCoder.Controllers.Animations.Components;
using AirCoder.Controllers.GamePhysics.Components;
using AirCoder.Controllers.NPC.Components;
using AirCoder.Core;
using AirCoder.Helper;
using AirCoder.Views;
using UnityEngine;
using Action = AirCoder.Controllers.AI.Components.FSM.Action;
using Application = AirCoder.Core.Application;
using Object = UnityEngine.Object;

namespace AirCoder.Controllers.NPC
{
    public class NpcSystem : GameSystem
    {
        public List<EnemyView> Enemies { get; private set; } 
        public Sprite[] Sprites { get; private set; }

        private readonly NpcSystemConfig _config;
        
        public NpcSystem(GameController inController, Application inApp, SystemConfig inConfig = null) : base(inController, inApp, inConfig)
        {
            _config = inConfig as NpcSystemConfig;
            if(_config == null) throw new Exception($"System config must be not null.");
            Sprites = null;
            
            // --------------------------------------------------------------------------------
            // Generate enemies
            // --------------------------------------------------------------------------------
            Enemies = null;
            ClearEnemiesList();
        }

        public void GenerateEnemies()
        {
            foreach (var enemy in _config.enemies)
            {
                if(!enemy.addToStage) continue;
                AddEnemy(enemy.startPosition, enemy);
            }
        }

        public void AddEnemy(Vector2 inPosition, NpcDataHolder inNpcData)
        {
            // --------------------------------------------------------------------------------
            // Setup Player
            // --------------------------------------------------------------------------------
            //here we can add ability to add a specific player (if we have more than one)
            Sprites = SystemFacade.Resources.LoadSpriteAtlas(ResourcesMap.Enemies);
            var enemy = new EnemyView(this.application, inNpcData.name, LayerNames.GameObjects, inNpcData.profile);
            enemy.SetSprite(Sprites[22]);
           
            // --------------------------------------------------------------------------------
            // Setup Physics (Add Collider Component)
            // --------------------------------------------------------------------------------
            var boxSize = new Vector2(Sprites[22].bounds.size.x, Sprites[22].bounds.size.y);
            enemy.AddComponent(new HitBox(enemy, boxSize, inNpcData.profile.mass, false));
            
            // --------------------------------------------------------------------------------
            // Setup Animation (Add Animator Component)
            // --------------------------------------------------------------------------------
            enemy.Profile.SetAnimationsSource(this.Sprites); //set sprite sheet source to all animations
            enemy.AddComponent(new GameAnimator(enemy, enemy.SpriteRenderer));
            
            // --------------------------------------------------------------------------------
            // Setup Controller (Add NpcController Component)
            // --------------------------------------------------------------------------------
            var controller = enemy.AddComponent(new NpcController(enemy, inNpcData.startFacing));
            
            // --------------------------------------------------------------------------------
            // Setup AI (Add StateMachine Component)
            // --------------------------------------------------------------------------------
            var entryState = SystemFacade.AI.CreateEnemyAi(controller, inNpcData);
            var stateMachine = enemy.AddComponent(new StateMachine(enemy, entryState));
            stateMachine.Start();
            
            SystemFacade.Level.Level.AddView(enemy, inPosition);
            ViewsContainer.AddView(enemy);
            Enemies.Add(enemy);
        }

        private void ClearEnemiesList()
        {
            if (Enemies == null)
            {
                Enemies = new List<EnemyView>();
                return;
            }

            foreach (var enemy in Enemies)
                enemy.Destroy();
            Enemies.Clear();
        }

        public override void Tick()
        {
            if((!IsRun)) return;
            foreach (var gameCollider in _components.ToList())
                gameCollider.Tick();
        }

        public override void PauseSystem()
        {
            base.PauseSystem();
            foreach (var gameCollider in _components.ToList())
                gameCollider.Pause();
        }

        public override void ResumeSystem()
        {
            base.ResumeSystem();
            foreach (var gameCollider in _components.ToList())
                gameCollider.Resume();
        }

        public void RemoveEnemy(EnemyView inEnemyView)
        {
            inEnemyView.RemoveAllComponents();
            inEnemyView.Visibility = false;
            SystemFacade.Level.Level.RemoveView(inEnemyView.name, inEnemyView.index);
            ViewsContainer.RemoveView(inEnemyView);
            Enemies.Remove(inEnemyView);
            Object.Destroy(inEnemyView.gameObject);
        }

        public void StopAllEnemies()
        {
            foreach (var enemy in Enemies)
            {
                enemy.GetComponent<StateMachine>().Pause();
                enemy.GetComponent<NpcController>().DoIdle();
            }
        }
    }
}
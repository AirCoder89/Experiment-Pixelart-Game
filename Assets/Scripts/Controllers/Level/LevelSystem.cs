using System;
using System.Linq;
using AirCoder.Controllers.Level.Components;
using AirCoder.Core;
using AirCoder.Helper;
using AirCoder.Views;
using UnityEngine;
using Application = AirCoder.Core.Application;

namespace AirCoder.Controllers.Level
{
    public class LevelSystem : GameSystem
    {
        public Core.Level Level { get; private set; }

        private readonly LevelSystemConfig _config;
        private SpriteView _background;
        
        public LevelSystem(GameController inController, Application inApp, SystemConfig inConfig = null) : base(inController, inApp, inConfig)
        {
            _config = inConfig as LevelSystemConfig;
            if(_config == null) throw new Exception($"System config must be not null.");
            
            ClearComponentList();

            Level = CreateLevel();
        }
        
        private Core.Level CreateLevel()
        {
            // --------------------------------------------------------------------------------
            // Setup Level
            // --------------------------------------------------------------------------------
            Level = CreateEmptyLevel();
            Level.Move(Vector2.zero);
            var levelSprite = SystemFacade.Resources.LoadSprite(ResourcesMap.Level);
            var levelView = new SpriteView(this.application, "Level", LayerNames.Level);
            levelView.SetSprite(levelSprite);
            Level.AddLevel(levelView, new Vector3(0, 0, 1));
            ViewsContainer.AddView(levelView);
            // --------------------------------------------------------------------------------
            // Setup Background
            // --------------------------------------------------------------------------------
            /*var bgPos = new Vector2(
                    (SystemFacade.Renderer.ScreenWidth / 2 / SystemFacade.Renderer.CurrentResolution)
                   ,(SystemFacade.Renderer.ScreenHeight / 2 / SystemFacade.Renderer.CurrentResolution));*/

            var bgSprite = SystemFacade.Resources.LoadSprite(ResourcesMap.Background);
            
            _background = new SpriteView(this.application, "Background", LayerNames.Background);
            _background.SetSprite(bgSprite);
            
            //- duplicate the background to fit the level platform
            var duplicated = new SpriteView(this.application, "Background", LayerNames.Background);
            duplicated.SetSprite(bgSprite);
            
             _background.AddChild(duplicated);
             duplicated.SetPosition(new Vector2(_background.GetSize().x, 0)); //apply offset
            
            
            Level.AddBackground(_background, _config.backgroundPosition);
            ViewsContainer.AddView(_background);

            return Level;
        }

        public void AddParallaxEffect(PlayerView inTarget)
        {
            _background.AddComponent(new ParallaxEffect(_background, inTarget, _config.speed, _config.strength));
        }
        
        
        private Core.Level CreateEmptyLevel()
        {
            var levelHolder = new GameObject("Level");
            levelHolder.transform.SetParent(null);
            levelHolder.transform.localScale = Vector3.one;
            return new Core.Level(levelHolder.transform, _config);
        }

        public override void Tick()
        {
            if((!IsRun)) return;
            foreach (var component in _components.ToList())
                component.Tick();
        }

        public override void PauseSystem()
        {
            base.PauseSystem();
            foreach (var component in _components.ToList())
                component.Pause();
        }

        public override void ResumeSystem()
        {
            base.ResumeSystem();
            foreach (var component in _components.ToList())
                component.Resume();
        }
    }
}
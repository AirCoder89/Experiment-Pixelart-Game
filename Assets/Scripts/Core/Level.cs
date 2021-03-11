using System.Collections.Generic;
using System.Linq;
using AirCoder.Controllers.Level;
using AirCoder.Controllers.VisualEffects;
using AirCoder.Helper;
using AirCoder.Views;
using NaughtyAttributes;
using UnityEngine;

namespace AirCoder.Core
{
    public class Level
    {
        
        private readonly Dictionary<string, List<GameView>> _views; //Id, view
        private readonly Transform _holder;
        private readonly LevelSystemConfig _config;
        private GameView _background;
        private GameView _level;
        
        public Level(Transform inHolder, LevelSystemConfig inConfig)
        {
            _config = inConfig;
            _holder = inHolder;
            _views = new Dictionary<string, List<GameView>>();
        }

        public void AddVfx(IVfx inVfx, Vector2 inPos)
        {
            inVfx.GetTransform().SetParent(_holder);
            inVfx.SetPosition(inPos);
            inVfx.SetScale(SystemFacade.Renderer.CurrentResetVector);
        }
        
        public void AddBackground(GameView inBg, Vector2 inPos)
        {
            _background = inBg;
            _background.SetParent(_holder);
            _background.SetPosition(inPos);
            _background.SetParent(null);
            _background.SetScale(SystemFacade.Renderer.CurrentResetVector);
        }

        public void AddLevel(GameView inLevel, Vector2 inPos)
        {
            _level = inLevel;
            _level.SetParent(this._holder);
            _level.SetPosition(inPos);
            _level.SetScale(SystemFacade.Renderer.ResetVector);
            
            //- add level collider
            var collider = Object.Instantiate(_config.levelCollider, this._holder);
            collider.transform.localPosition = inPos;
            collider.transform.localScale = SystemFacade.Renderer.ResetVector;
        }
        
        public void AddView(GameView inView, Vector2 inPosition)
        {
            if (!_views.ContainsKey(inView.name))  _views.Add(inView.name, new List<GameView>());
            if(_views[inView.name] == null) _views[inView.name] = new List<GameView>();

            inView.index = _views[inView.name].Count;
            _views[inView.name].Add(inView);

            inView.SetParent(this._holder);
            inView.SetPosition(inPosition);
        }

        public List<GameView> GetViews(string inViewName)
        {
            return !_views.ContainsKey(inViewName) ? null : _views[inViewName];
        }
        
        public GameView GetView(string inViewName, int inDex)
        {
            if (!_views.ContainsKey(inViewName)) return null;
            if (_views[inViewName] == null || _views[inViewName].Count == 0) return null;
            return _views[inViewName][inDex];
        }

        public bool RemoveViews(string inViewName)
        {
            return _views.ContainsKey(inViewName) && _views.Remove(inViewName);
        }

        public void RemoveView(string inViewName, int inDex)
        {
            if (!_views.ContainsKey(inViewName)) return;
            if (_views[inViewName] == null || _views[inViewName].Count == 0) return;
            _views[inViewName].RemoveAt(inDex);
        }
        
        public void Tick()
        {
            foreach (var view in _views.ToList())
            {
                if(view.Value == null) continue;
                    foreach (var gameView in view.Value.ToList())
                    {
                        gameView?.Tick();
                    }
            }
        }

        public void Move(Vector2 inPosition)
        {
            var currentRes = SystemFacade.Renderer.CurrentResolution;
            SetPosition(new Vector2(-inPosition.x * currentRes, inPosition.y * currentRes));
        }

        public void SetPosition(Vector2 inPosition)
        {
            var currentPos = _holder.localPosition;
            currentPos.x = inPosition.x;
            currentPos.y = inPosition.y;
            currentPos.z = 1f;
            _holder.localPosition = currentPos;
        }

        [Button("Pause")]
        public void Pause()
        {
            foreach (var view in _views)
            foreach (var gameView in view.Value)
                gameView.PauseView();
        }
        
        [Button("Resume")]
        public void Resume()
        {
            foreach (var view in _views)
            foreach (var gameView in view.Value)
                gameView.ResumeView();
        }
        
        [Button("Reset")]
        public void ResetView()
        {
            foreach (var view in _views)
            foreach (var gameView in view.Value)
                gameView.ResetView();
        }
        
        [Button("Hide All")]
        public void HideAllViews()
        {
            foreach (var view in _views)
            foreach (var gameView in view.Value)
                gameView.Visibility = false;
        }
    }
}
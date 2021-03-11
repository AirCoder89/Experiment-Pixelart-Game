using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

namespace AirCoder.Core
{
    public class GameController
    {
        private readonly Dictionary<Type, GameSystem> _systems;
        public GameController()
        {
            _systems = new Dictionary<Type, GameSystem>();
        }

        public GameController AddSystem(GameSystem inSystem)
        {
            if(_systems.ContainsKey(inSystem.GetType())) throw new Exception($"Cannot duplicate game systems");
            _systems.Add(inSystem.GetType(), inSystem);
            return this;
        }
        
        public T GetSystem<T>() where T : GameSystem
        {
            if (!_systems.ContainsKey(typeof(T))) return null;
            return (T) _systems[typeof(T)];
        }
    
        public void Tick()
        {
            foreach (var system in _systems.Values)
                system.Tick();
        }

        public void FixedTick()
        {
            foreach (var system in _systems.Values)
            {
                if (!(system is IFixedTick)) continue;
                    var f = system as IFixedTick;
                    f.FixedTick();
            }
        }
        
        public void LateTick()
        {
            foreach (var system in _systems.Values)
            {
                if (!(system is ILateTick)) continue;
                    var f = system as ILateTick;
                    f.LateTick();
            }
        }

        [Button("Start")]
        public void Start()
        {
            foreach (var system in _systems.Values)
            {
                system.StartSystem();
            }
        }
    
        [Button("Pause")]
        public void Pause()
        {
            foreach (var system in _systems.Values)
                system.PauseSystem();
        }
        
        [Button("Resume")]
        public void Resume()
        {
            foreach (var system in _systems.Values)
                system.ResumeSystem();
        }
        
        [Button("Reset")]
        public void Reset()
        {
            foreach (var system in _systems.Values)
                system.ResetSystem();
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using AirCoder.Core;
using AirCoder.Helper;
using AirCoder.Views;
using PathologicalGames;
using UnityEngine;
using Application = AirCoder.Core.Application;

namespace AirCoder.Controllers.ObjectsPooling
{
    [RequireComponent(typeof(SpawnPool))]
    public class PoolSystem : GameSystem
    {
        private Dictionary<Type, HashSet<GameView>> _viewsBuffer;
        private SpawnPool _p;
        private SpawnPool _pool
        {
            get
            {
                if (_p == null) _p = SystemFacade.Application.spawnPool;
                return _p;
            }
        }
        
        public PoolSystem(GameController inController, Application inApp, SystemConfig inConfig = null) : base(inController, inApp, inConfig)
        {
            _viewsBuffer = new Dictionary<Type, HashSet<GameView>>();
        }

        public bool HasItem(string inName)
            => _pool.prefabPools.ContainsKey(inName);

        public bool HasView<T>() =>  _viewsBuffer.ContainsKey(typeof(T));
        public void AddItem(Transform inTransform, string inName, bool inDespawn, Transform inParent)
            => _pool.Add(inTransform, inName, inDespawn, inParent);

        public void StoreGameView<T>(T inView) where T : GameView
        {
            if (!HasView<T>())
                _viewsBuffer.Add(typeof(T), new HashSet<GameView>());
            _viewsBuffer[typeof(T)].Add(inView);
        }

        public T GetConcreteGameView<T>() where T : GameView
        {
            if (!HasView<T>()) return null;
            if (_viewsBuffer[typeof(T)].Count == 0 || _viewsBuffer[typeof(T)] == null) return null;
            var t = _viewsBuffer[typeof(T)].First();
            _viewsBuffer[typeof(T)].Remove(t);
            return t as T;
        }
        
        public GameObject Spawn(string inObjectName)
            => _pool.Spawn(inObjectName).gameObject;
        
        public T Spawn<T>(string inObjectName)
            => _pool.Spawn(inObjectName).gameObject.GetComponent<T>();
        
        public T Spawn<T>(string inObjectName, Transform inParent)
            => _pool.Spawn(inObjectName, inParent).gameObject.GetComponent<T>();
        
        public T Spawn<T>(string inObjectName, Vector3 inPosition, Quaternion inRotation)
            => _pool.Spawn(inObjectName, inPosition, inRotation).gameObject.GetComponent<T>();

        public void Despawn(Transform inObjectTransform)
            => _pool.Despawn(inObjectTransform, _pool.transform);

    }
}
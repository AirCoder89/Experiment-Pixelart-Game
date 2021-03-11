using System.Collections.Generic;
using AirCoder.Core;
using AirCoder.Views;
using UnityEngine;
using Application = AirCoder.Core.Application;

namespace AirCoder.Helper
{
    public static class ObjectMap
    {
        private static Dictionary<GameObject, GameView> _objects;
        private static Application _application;
        
        public static void Initialize(Application inApp)
        {
            _application = inApp;
            _objects = new Dictionary<GameObject, GameView>();
        }

        public static bool HasObject(GameObject inGameObject) => _objects.ContainsKey(inGameObject);

        public static GameView GetGameView(GameObject inGameObject)
        {
            return !HasObject(inGameObject) ? null : _objects[inGameObject];
        }
        public static bool SubscribeObject(GameView inGameView)
        {
            if (HasObject(inGameView.gameObject)) return false;
            _objects.Add(inGameView.gameObject, inGameView);
            return true;
        }

        public static bool UnsubscribeObject(GameView inGameView)
        {
            if (!HasObject(inGameView.gameObject)) return false;
            _objects.Remove(inGameView.gameObject);
            return true;
        }
        
        public static bool UnsubscribeObject(GameObject inGameObject)
        {
            if (!HasObject(inGameObject)) return false;
            _objects.Remove(inGameObject);
            return true;
        }
    }
}
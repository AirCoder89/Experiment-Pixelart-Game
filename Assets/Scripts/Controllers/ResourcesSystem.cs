using System;
using System.Collections.Generic;
using AirCoder.Core;
using UnityEngine;
using Application = AirCoder.Core.Application;

namespace AirCoder.Controllers
{
    public class ResourcesSystem : GameSystem
    {
        private Dictionary<string, Sprite[]> _loadedSprites;
        
        public ResourcesSystem(GameController inController, Application inApp, SystemConfig inConfig = null) : base(inController, inApp, inConfig)
        {
            DisposeAllSprites();
        }
        
        public Sprite LoadSprite(string inName)
        {
            return Resources.Load<Sprite>(inName);
        }
        
        public Sprite[] LoadSpriteAtlas(string inName) {

            if (!_loadedSprites.ContainsKey(inName)) {
                _loadedSprites.Add(inName, Resources.LoadAll<Sprite>(inName));
            }
            return _loadedSprites[inName];
        }

        public void DisposeAllSprites()
        {
            if (_loadedSprites == null)
            {
                _loadedSprites = new Dictionary<string, Sprite[]>();
                return;
            }
            _loadedSprites?.Clear();
        }
    }
}
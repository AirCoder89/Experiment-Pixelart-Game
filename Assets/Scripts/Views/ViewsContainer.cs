using System.Collections.Generic;
using AirCoder.Core;
using AirCoder.Helper;
using UnityEngine;
using Application = AirCoder.Core.Application;

namespace AirCoder.Views
{
    public class ViewsContainer
    {
        public static Dictionary<string, GameView> views { get; private set; }
        private static Application _application;
        public ViewsContainer(Application inApp)
        {
            _application = inApp;
            views = new Dictionary<string, GameView>();
        }
        
        public static void AddView(GameView inView)
        {
            if(views.ContainsKey(inView.name)) return;
            views.Add(inView.name, inView);
            ObjectMap.SubscribeObject(inView);
        }

        public static GameView GetView(string inName)
        {
            return !views.ContainsKey(inName) ? null : views[inName];
        }

        public static bool RemoveView(string inName)
        {
            if(!views.ContainsKey(inName)) return false;
            ObjectMap.SubscribeObject(views[inName]);
            return views.Remove(inName);
        }
        
        public static bool RemoveView(GameView inView)
        {
            if(!views.ContainsKey(inView.name)) return false;
            ObjectMap.SubscribeObject(inView);
            return views.Remove(inView.name);
        }
        
    }
}

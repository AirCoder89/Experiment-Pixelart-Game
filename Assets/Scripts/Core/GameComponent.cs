using System.Collections.Generic;
using AirCoder.Extensions;
using AirCoder.Helper;
using AirCoder.Views;

namespace AirCoder.Core
{
    /// <summary>
    /// Game Component base class
    /// </summary>
    public abstract class GameComponent
    {
        public GameView gameView;

        protected bool _isRun;
        public GameComponent(GameView inGameView)
        {
            gameView = inGameView;
        }

        public virtual void Destroy()
        {
            //release references
            gameView = null;
        }

        public virtual void Pause()
        {
            _isRun = false;
        }

        public virtual void Resume()
        {
            _isRun = true;
        }

        public virtual void Tick()
        {
            if(!_isRun) return;
        }
        
        protected void Log(string inText)
        {
            var prefix = $"C-{GetType().Name}";
            Log(inText, DebugColor.None, prefix, SystemFacade.Parameters.componentsColor);
        }
        
        protected virtual void Log(string text, DebugColor color = DebugColor.None, bool clear = false) 
            => Debugger.Print(text, color, "",DebugColor.black, clear,GetType());

        protected virtual void Log(string text ,DebugColor color = DebugColor.None ,string prefix = "",DebugColor colorPrefix = DebugColor.None ,bool clear = false) 
            => Debugger.Print(text, color, prefix, colorPrefix,clear,GetType());

        protected void ClearLog() => Debugger.Clear();

        protected virtual void Log<T, TD>(Dictionary<T, TD> dic, string title = "" ,string prefix = "", bool clear = false)
        {
            if (typeof(TD).IsAssignableFrom(typeof(ILog)))
                Debugger.PrintILogDictionary(dic, title,clear, GetType());
            else 
                Debugger.PrintDictionary(dic, title, clear, GetType());
        }
        
        protected virtual void Log<T>(List<T> list,string title = "" ,string prefix = "",bool clear = false)
        {
            if (typeof(T).IsAssignableFrom(typeof(ILog)))
                Debugger.PrintILogList(list, title, clear, GetType());
            else 
                Debugger.PrintList(list, title, clear, GetType());
        }
        
    }
}
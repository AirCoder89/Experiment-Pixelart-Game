using AirCoder.Extensions;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace AirCoder.Core
{
    [System.Serializable]
    public struct GameResolution
    {
        public int width;
        public int height;

        public GameResolution(int w, int h)
        {
            width = w;
            height = h;
        }
        
        public void AssignFromScreenSize()
        {
            width = Screen.width;
            height = Screen.height;
        }
    }
    
    [CreateAssetMenu(menuName = "Application/Parameters")]
    public class AppParameters : ScriptableObject
    {
        public GameResolution resolution;
        public int frameRate = 60;
        public bool cursorVisibility = false;
        public int vSyncCount = 1;
        public int gravityScale = 80;
            
        [BoxGroup("Debugging")]
        public bool debuggingFilter;
        [HorizontalLine()]
        [BoxGroup("Debugging")][ShowIf("debuggingFilter")]
        public bool debugSystems;
        
        [BoxGroup("Debugging")][ShowIf("debuggingFilter")]
        public bool debugViews;
        
        [BoxGroup("Debugging")][ShowIf("debuggingFilter")]
        public bool debugComponents; 
        
        [BoxGroup("Debugging")][ShowIf("debuggingFilter")]
        public bool helperClasses;
        
        [BoxGroup("Debugging")][ShowIf("debuggingFilter")]
        public bool debugOthers;

        [HorizontalLine()] 
        [BoxGroup("Debugging")][ShowIf("debuggingFilter")] public DebugColor systemsColor;
        [BoxGroup("Debugging")][ShowIf("debuggingFilter")] public DebugColor viewsColor;
        [BoxGroup("Debugging")][ShowIf("debuggingFilter")] public DebugColor componentsColor;


    }
}
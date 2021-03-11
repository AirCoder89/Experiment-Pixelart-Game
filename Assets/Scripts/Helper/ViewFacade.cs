using AirCoder.Core;
using AirCoder.Views;
using UnityEngine;
using Application = AirCoder.Core.Application;

namespace AirCoder.Helper
{
    public static class ViewFacade
    {
        private static Application _application;

        public static void Initialize(Application inApp)
        {
            _application = inApp;
        }
        
        //- Player
        private static PlayerView _player;
        public static PlayerView Player => _player ?? (_player = ViewsContainer.GetView("Player") as PlayerView);

        //- Level
        private static SpriteView _level;
        public static SpriteView Level => _level ?? (_level = ViewsContainer.GetView("Level") as SpriteView);
        
        //- Background
        private static SpriteView _bg;
        public static SpriteView Background => _bg ?? (_bg = ViewsContainer.GetView("Background") as SpriteView);
        
        //- Camera
        private static CameraView _camera;
        public static CameraView Camera => _camera ?? (_camera = ViewsContainer.GetView("Camera") as CameraView);
        
        //- Enemies
        public static EnemyView Enemy(string inEnemyName) => ViewsContainer.GetView("inEnemyName") as EnemyView;
    }
}
using System;
using AirCoder.Core;
using AirCoder.Views;
using UnityEngine;

namespace AirCoder.Controllers.Shooting
{
    public interface IDestructible
    {
        event Action<GameView> Destroyed; 
        event Action<GameView, Vector2> OnTakeDamage; 
        bool IsAlive { get; }
        float Health { get; }
        void TakeDamage(float inDamage, Vector2 inDirection);
        void Destroy();
    }
}
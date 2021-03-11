using UnityEngine;

namespace AirCoder.Controllers.VisualEffects
{
    public interface IVfx
    {
        void Remove();
        void Play();
        void Tick();
        Transform GetTransform();
        void SetPosition(Vector2 inPos);
        void SetScale(Vector2 inScale);
    }
}
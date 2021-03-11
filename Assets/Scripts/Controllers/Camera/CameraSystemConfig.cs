using AirCoder.Controllers.Camera.Components;
using AirCoder.Core;
using NaughtyAttributes;
using UnityEngine;

namespace AirCoder.Controllers.Camera
{
    [CreateAssetMenu(menuName = "Application/Systems/CameraConfig")]
    public class CameraSystemConfig: SystemConfig
    {
        [BoxGroup("Establishing Parameters")] public string tag = "MainCamera";
        [BoxGroup("Establishing Parameters")] public Color backgroundColor = Color.black;
        [BoxGroup("Establishing Parameters")] public CameraClearFlags clearFlag = CameraClearFlags.Color;
        [BoxGroup("Establishing Parameters")] public float orthographicSize = 120f;
        
        [BoxGroup("Follow")] public Axis followAxis;
        [BoxGroup("Follow")] public Vector3 offset;
        [BoxGroup("Follow")] public Vector2 followLimit;
        [BoxGroup("Follow")][Range(0,20)] public float followSpeed;

        [BoxGroup("Shake")] public float shakeDuration = 2f;
        [BoxGroup("Shake")] public float shakeAmount = 1f;
        [BoxGroup("Shake")] public float shakeDecreaseAmount = 0.2f;
        
    }
}
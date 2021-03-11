using AirCoder.Core;
using NaughtyAttributes;
using UnityEngine;
using Vector2Int = AirCoder.Core.Vector2Int;

namespace AirCoder.Controllers.NPC
{
    [System.Serializable]
    public struct NpcDataHolder
    {
        public bool addToStage;
        public string name;
        public Vector2 startPosition;
        public ActorFacing startFacing;
        [MinMaxSlider(0, 20f)] public Vector2 idleDuration;
        [MinMaxSlider(0, 100f)] public Vector2 patrolDuration;
        public Vector2Int patrolRange;
        [Expandable] public NpcProfile profile;
    }
}
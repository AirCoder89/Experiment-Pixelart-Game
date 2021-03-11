using System.Collections.Generic;
using System.ComponentModel;
using AirCoder.Core;
using NaughtyAttributes;
using UnityEngine;

namespace AirCoder.Controllers.VirtualInput
{
    [CreateAssetMenu(menuName = "Application/Systems/InputConfig")]
    public class InputSystemConfig: SystemConfig
    {
        [SerializeField] private string shoot = "Shoot";
        [SerializeField] private string movement = "Horizontal";
        [SerializeField] private string duck = "Duck";
        [SerializeField] private string jump = "Jump";
        [SerializeField][BoxGroup("Shift Key")] public string shift = "Shift";
        
        private Dictionary<InputBehaviour, string> _playersBehavioursList;
        
        public void Initialize()
        {
            _playersBehavioursList = new Dictionary<InputBehaviour, string>();
            AssignBehaviour(InputBehaviour.Shoot, this.shoot);
            AssignBehaviour(InputBehaviour.Move, movement);
            AssignBehaviour(InputBehaviour.Jump, this.jump);
            AssignBehaviour(InputBehaviour.Duck, this.duck);
        }

        public string GetBehaviourAxis(InputBehaviour inBehaviour)
        {
            if (!_playersBehavioursList.ContainsKey(inBehaviour))
            {
                throw new WarningException($"Players Behaviours List doesn't contains the given behaviour {inBehaviour}");
            }
            return _playersBehavioursList[inBehaviour];
        }
        
        private void AssignBehaviour(InputBehaviour inBehaviour, string inBehaviourName)
        {
            if(string.IsNullOrEmpty(inBehaviourName)) return;
            _playersBehavioursList.Add(inBehaviour, inBehaviourName);
        }
    }
}
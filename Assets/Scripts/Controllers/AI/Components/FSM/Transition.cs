using NaughtyAttributes;

namespace AirCoder.Controllers.AI.Components.FSM
{
    [System.Serializable]
    public class Transition
    {
        public bool hasFalseState;
        [Required]public Decision decision;
        [Required]public State trueState;
        [ShowIf("hasFalseState")][Required]public State falseState;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterScramble
{
    public class PlayerStateManager : MonoBehaviour
    {
        public State CurrentState => _currentState;
        private State _currentState;

        public void SwitchState(State nextState)
        {
            _currentState = nextState;
        }
    }
}
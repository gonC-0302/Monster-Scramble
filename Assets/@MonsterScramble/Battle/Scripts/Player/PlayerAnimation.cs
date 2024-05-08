using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterScramble
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimation : MonoBehaviour
    {
        private Animator _anim;
        static readonly int BLEND_KEY = Animator.StringToHash("Blend");
        static readonly int ATTACK_KEY = Animator.StringToHash("Attack");

        void Start()
        {
            _anim = GetComponent<Animator>();
        }

        public void PlayMoveAnimation(float speed)
        {
            _anim.SetFloat(BLEND_KEY, speed);
        }

        public void PlayAttackAnimation()
        {
            _anim.SetTrigger(ATTACK_KEY);
        }
    }
}
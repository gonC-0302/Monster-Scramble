using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterScramble
{
    public class HitPoint : MonoBehaviour
    {
        private int _hp;
        [SerializeField] private int _maxHP;

        private void Start()
        {
            _hp = _maxHP;
        }

        public void GetHit(int damage)
        {
            _hp -= damage;
            if (_hp < 1)
            {
                _hp = 0;
                Destroy(gameObject);
            }
        }
    }
}
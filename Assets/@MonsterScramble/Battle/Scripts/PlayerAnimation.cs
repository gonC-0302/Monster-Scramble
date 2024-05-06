using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animation))]
public class PlayerAnimation : MonoBehaviour
{
    private Animator _anim;
    static readonly int BLEND_KEY = Animator.StringToHash("Blend");

    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    public void PlayMoveAnimation(float speed)
    {
        _anim.SetFloat(BLEND_KEY, speed);
    }
}

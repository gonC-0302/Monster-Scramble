using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    //AudioSource（スピーカー）を同時に鳴らしたい音の数だけ用意
    private AudioSource[] audioSourceList = new AudioSource[5];

    [SerializeField]
    private AudioClip _damageSE, _summonSE,_attackSE,_GetSummonCardSE;

    [SerializeField]
    private AudioSource _bgm;

    private void Awake()
    {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        //auidioSourceList配列の数だけAudioSourceを自分自身に生成して配列に格納
        for (var i = 0; i < audioSourceList.Length; ++i)
        {
            audioSourceList[i] = gameObject.AddComponent<AudioSource>();
        }
    }

    public void PlayBattleBGM()
    {
        _bgm.Play();
    }

    public void PlayDamageSE()
    {
        Play(_damageSE);
    }
    public void PlayGetSummonCardSE()
    {
        Play(_GetSummonCardSE);
    }

    public void PlayAttackSE()
    {
        Play(_attackSE);
    }


    public void PlaySummonSE()
    {
        Play(_summonSE);
    }

    //未使用のAudioSourceの取得 全て使用中の場合はnullを返却
    private AudioSource GetUnusedAudioSource()
    {
        for (var i = 0; i < audioSourceList.Length; ++i)
        {
            if (audioSourceList[i].isPlaying == false) return audioSourceList[i];
        }

        return null; //未使用のAudioSourceは見つかりませんでした
    }

    //指定されたAudioClipを未使用のAudioSourceで再生
    public void Play(AudioClip clip)
    {
        var audioSource = GetUnusedAudioSource();
        if (audioSource == null) return; //再生できませんでした
        audioSource.clip = clip;
        audioSource.Play();
    }
}

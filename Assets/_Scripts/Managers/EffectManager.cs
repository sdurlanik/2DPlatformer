using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance;
    private AudioSource _effectSource;

    [SerializeField] private AudioClip[] _effectClips;
    public enum EffectState
    {
        JUMP,
        DASH,
        LANDING,
        COLLECT,
        WON,
        ATTACK,
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; 
        }
        else
        {
            Destroy(gameObject);
        }

        _effectSource = GetComponent<AudioSource>();
    }

    public void PlayEffectSound(EffectState state)
    {
        switch (state)
        {
            case EffectState.JUMP:
                _effectSource.volume = .4f;
                SoundManager.Instance.PlaySound(_effectClips[0]);
                break;
            case EffectState.DASH:
                _effectSource.volume = .3f;
                SoundManager.Instance.PlaySound(_effectClips[1]);
                break;
            case EffectState.LANDING:
                _effectSource.volume = .2f;
                SoundManager.Instance.PlaySound(_effectClips[2]);
                break;
            case EffectState.COLLECT:
                _effectSource.volume = .8f;
                SoundManager.Instance.PlaySound(_effectClips[3]);
                break;
            case EffectState.WON:
                _effectSource.volume = 1;
                SoundManager.Instance.PlaySound(_effectClips[4]);
                break;
            case EffectState.ATTACK:
                _effectSource.volume = .5f;
                SoundManager.Instance.PlaySound(_effectClips[5]);
                break;
                
        }
    }
    
}



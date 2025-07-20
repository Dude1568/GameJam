using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Soundmanager : MonoBehaviour
{
    [SerializeField] private AudioSource soundEffectObject;
    public static Soundmanager instance;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public void PlaySoundEffect(AudioClip audioClip, Transform SpawnTransform, float Volume)
    {
        AudioSource audioSource = Instantiate(soundEffectObject, SpawnTransform.position, quaternion.identity);
        audioSource.clip = audioClip;
        audioSource.volume = Volume;
        audioSource.Play();
        float cliplength = audioSource.clip.length;

        Destroy(audioSource, cliplength);
    }
        public void PlayRandomSoundEffect(AudioClip[] audioClip, Transform SpawnTransform, float Volume)
    {
        int rand = UnityEngine.Random.Range(0, audioClip.Length);
        AudioSource audioSource = Instantiate(soundEffectObject, SpawnTransform.position, quaternion.identity);
        audioSource.clip = audioClip[rand];
        audioSource.volume = Volume;
        audioSource.Play();
        float cliplength = audioSource.clip.length;

        Destroy(audioSource, cliplength);
    }
}

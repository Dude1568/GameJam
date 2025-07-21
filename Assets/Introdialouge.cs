using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Introdialouge : MonoBehaviour
{
    Transform player;
    [SerializeField] AudioClip[] sounds;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(intro());
    }
    IEnumerator intro()
    {
        Soundmanager.instance.PlaySoundEffect(sounds[0],player,100);
         yield return new WaitForSeconds(5f);
        Soundmanager.instance.PlaySoundEffect(sounds[1],player,100);
        yield return new WaitForSeconds(5f);
        Soundmanager.instance.PlaySoundEffect(sounds[2],player,100);
        yield return new WaitForSeconds(5f);
        Soundmanager.instance.PlaySoundEffect(sounds[3],player,100);
        yield return new WaitForSeconds(5f);
        Soundmanager.instance.PlaySoundEffect(sounds[4],player,100);

        yield break;
    }

}

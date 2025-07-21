using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Introdialouge : MonoBehaviour
{
    Transform player;
    [SerializeField]GameObject menu;
    [SerializeField] GameObject goldMenu;
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
         yield return new WaitForSeconds(sounds[0].length-1.5f);
        Soundmanager.instance.PlaySoundEffect(sounds[1],player,100);
        yield return new WaitForSeconds(sounds[1].length-1.8f);
        Soundmanager.instance.PlaySoundEffect(sounds[2],player,100);
        menu.SetActive(true);
        yield return new WaitForSeconds(sounds[2].length);
        Soundmanager.instance.PlaySoundEffect(sounds[3],player,100);
        yield return new WaitForSeconds(sounds[3].length - 8f);
        goldMenu.SetActive(true);
        
        yield return new WaitForSeconds(8f);
        Soundmanager.instance.PlaySoundEffect(sounds[4],player,100);
        yield return new WaitForSeconds(sounds[4].length);
        yield break;
    }

}

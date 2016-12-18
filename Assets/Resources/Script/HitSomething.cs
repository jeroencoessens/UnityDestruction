using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSomething : MonoBehaviour
{
    private AudioSource Audio;
    public List<AudioClip> clips;

    void Start()
    {
        Audio = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        var random = Random.Range(0, clips.Count + 5);
        if (random > clips.Count - 1) return;

        var clip = clips[random];
        if(Audio != null && clip != null) Audio.PlayOneShot(clip);
    }
}

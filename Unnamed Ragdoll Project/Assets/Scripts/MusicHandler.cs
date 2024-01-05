using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicHandler : MonoBehaviour
{
    public int SongOn;
    AudioSource[] Songs;
    int PreSongOn;

    // Start is called before the first frame update
    void Start()
    {
        Songs = GetComponentsInChildren<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(PreSongOn != SongOn)
        {
            for (int i = 1; i < Songs.Length; i++)
            {
                Songs[i].Stop();
            }
            Songs[SongOn].Play();

            PreSongOn = SongOn;
        }
    }
}
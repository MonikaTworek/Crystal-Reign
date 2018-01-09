using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomSoundPlayer : MonoBehaviour {

    private List<AudioSource> sounds;
    private System.Random random = new System.Random();

	// Use this for initialization
	void Start () {
        sounds = GetComponents<AudioSource>().ToList();
	}
	
    public void Play(bool randDelay = false)
    {
        if (sounds == null) sounds = GetComponents<AudioSource>().ToList();
        AudioSource src = sounds[random.Next() % sounds.Count];
        if (randDelay)
        {
            src.PlayDelayed(((float)(random.Next() % 100)) / 1000f);
        }
        else
        {
           src.Play();
        }
    }
}

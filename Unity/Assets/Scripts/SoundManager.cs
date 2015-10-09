using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

    public AudioSource musicSource;
    public static SoundManager instanceSM = null; 


	// Update is called once per frame
	void Update () {
        if (!GetComponent<AudioSource>().isPlaying)
            GetComponent<AudioSource>().Play(); 

    }
}


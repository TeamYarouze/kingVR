using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMCtrl : MonoBehaviour {

    public AudioClip bgm;
    public AudioClip jingle;

    private AudioSource audioSource;

	// Use this for initialization
	void Start () {
		
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayBGM()
    {
        Stop();

        audioSource.clip = bgm;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void Stop()
    {
        audioSource.Stop();
    }

    public void PlayJingle()
    {
        Stop();

        audioSource.clip = jingle;
        audioSource.loop = false;
        audioSource.Play();
    }
}

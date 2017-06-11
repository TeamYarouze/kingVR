using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemBase : MonoBehaviour {

    private ParticleSystem psc = null;

	// Use this for initialization
	void Start () {
        psc = GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Play()
    {
        if( !psc ) return;

        psc.Play();
    }

    public void Stop()
    {
        if( !psc ) return;

        psc.Stop();
    }

    public bool IsPlay()
    {
        if( !psc ) return false;

        return psc.isPlaying;
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDown : MonoBehaviour {

    private Animator animator;
    private bool bFinish;
    public bool IsFinish
    {
        get { return bFinish; }
    }

	// Use this for initialization
	void Start () {
		
        animator = GetComponent<Animator>();
        bFinish = false;
	}
	
	// Update is called once per frame
	void Update () {
		

	}


    public void StartCountDown()
    {
        animator.SetTrigger("CountDownStart");

        bFinish = false;
        StartCoroutine(IsFinishCountDown());
    }

    private IEnumerator IsFinishCountDown()
    {
        while( true )
        {
            AnimatorStateInfo nowState = animator.GetCurrentAnimatorStateInfo(0);
            if (nowState.IsName("ReadyGoFinish"))
            {
                bFinish = true;
                break;
            }
            yield return null;
        }    
    }
}

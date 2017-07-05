using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalUI : MonoBehaviour {

    private Animator animator;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartGoalUI()
    {
        animator.SetTrigger("IsGoalTrigger");
    }

    public void StopGoalUI()
    {
        animator.SetTrigger("GoIdle");
    }
}

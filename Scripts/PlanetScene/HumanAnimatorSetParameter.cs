using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanAnimatorSetParameter : MonoBehaviour { // Scene(Planet)のPlayerにアタッチ

    public RotatePlanet rotatePlanet;
    Animator animator;
   
	void Start () {
        animator = GetComponent<Animator>();
	}
	void Update () {
        if (rotatePlanet.IsHumanRun())
        {
           animator.SetBool("isRun", true);
        }
        else
        {
            animator.SetBool("isRun", false);
        }
	}
}

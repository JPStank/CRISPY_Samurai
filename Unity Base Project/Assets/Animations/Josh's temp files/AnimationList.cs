using UnityEngine;
using System.Collections;

public class AnimationList : MonoBehaviour 
{
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.O))
        {
            animation.Play("Idle");
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            animation.Play("Block");
        }
	}
}

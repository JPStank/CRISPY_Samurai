using UnityEngine;
using System.Collections;

public class DemoMove : MonoBehaviour 
{
    public Animator anim;

    public float speed = 5.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
        {
            anim.SetBool("Forward", true);
        }
        else
        {
            anim.SetBool("Forward", false);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            anim.SetBool("Right", true);
        }
        else
        {
            anim.SetBool("Right", false);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            anim.SetBool("Left", true);
        }
        else
        {
            anim.SetBool("Left", false);
        }
	}
}

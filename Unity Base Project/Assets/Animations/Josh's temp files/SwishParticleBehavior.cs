using UnityEngine;
using System.Collections;

public class SwishParticleBehavior : MonoBehaviour 
{
    public TrailRenderer trail;

	void Start () 
    {
        if (trail == null)
        {
            Debug.LogError("YOU MUST SET THE TRAIL IDIOT!!");
        }

        trail.enabled = false;
	}
	
    public void ActivateTrail()
    {
        trail.enabled = true;
    }

    public void DisableTrail()
    {
        trail.enabled = false;
    }
}

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
        if (trail)
            trail.enabled = false;
	}
	
    public void ActivateTrail()
    {
        if (trail)
            trail.enabled = true;
    }

    public void DisableTrail()
    {
        if (trail)
            trail.enabled = false;
    }
}

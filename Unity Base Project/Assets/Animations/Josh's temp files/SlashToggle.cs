using UnityEngine;
using System.Collections;

public class SlashToggle : MonoBehaviour 
{
    public TrailRenderer trail = null;
	// Use this for initialization
	void Start () 
    {
	    if (trail == null)
        {
            trail = GetComponent<TrailRenderer>();
        }
        trail.enabled = false;
	}

	void ActivateTrail()
    {
        if (gameObject.tag == "Player")
            trail.enabled = true;
    }
    void DisableTrail()
    {
        if (gameObject.tag == "Player")
            trail.enabled = false;
    }
}

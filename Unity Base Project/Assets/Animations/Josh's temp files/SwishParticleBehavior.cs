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

    public IEnumerator KillSelfSequence()
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < 0.5f) // magic number limit
        {
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        if (trail)
            trail.enabled = false;
    }
	
    public void ActivateTrail()
    {
        if (trail)
        {
            trail.enabled = true;
            StartCoroutine(KillSelfSequence());
        }
    }

    public void DisableTrail()
    {
        if (trail)
            trail.enabled = false;
    }
}

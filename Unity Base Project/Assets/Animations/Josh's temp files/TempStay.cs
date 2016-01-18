using UnityEngine;
using System.Collections;

public class TempStay : MonoBehaviour 
{

    public Transform hips;

    // TODO: find a way around this hack
	void LateUpdate () 
    {
        hips.position = new Vector3(hips.position.x, 1.0f, hips.position.z);
	}
}

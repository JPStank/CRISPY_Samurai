using UnityEngine;
using System.Collections;

public class TempStay : MonoBehaviour
{

	public Transform hips;

	// TODO: find a way around this hack
	void LateUpdate()
	{
		if (gameObject.GetComponent<PuppetScript>().curState != PuppetScript.State.DEAD)
		{
			hips.position = new Vector3(hips.position.x, 1.0f, hips.position.z);
		}
		else
		{
			transform.position = new Vector3(transform.position.x, 1.0f, transform.position.z);

		}
	}
}

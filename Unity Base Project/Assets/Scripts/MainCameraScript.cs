using UnityEngine;
using System.Collections;

public class MainCameraScript : MonoBehaviour {

	public PuppetCameraScript camScript;
	public float minDist;
	
	// Use this for initialization
	void Start () {

		if (camScript == null)
		{
			GameObject player = GameObject.FindGameObjectWithTag("Player");
			camScript = player.transform.GetComponent<PuppetCameraScript>();
		}

		// initialize minimum distance to player
		if (minDist == 0.0f)
			minDist = 0.05f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void AdjustCamDist(Collider col)
	{
		if (col.transform.tag != "Player")
		{
			camScript.camOffsetRatio -= Time.deltaTime * camScript.camSpeed;
			if (camScript.camOffsetRatio < minDist)
				camScript.camOffsetRatio = minDist;
		}
	}

	void OnTriggerEnter(Collider col)
	{
		AdjustCamDist(col);
	}
	void OnTriggerStay(Collider col)
	{
		AdjustCamDist(col);
	}
}

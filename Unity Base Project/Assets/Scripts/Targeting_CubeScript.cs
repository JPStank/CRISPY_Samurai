using UnityEngine;
using System.Collections;

public class Targeting_CubeScript : MonoBehaviour {

	private float rotSpeed;
	public float scaleFloat;
	public float scaleSpeed;
	private float dir;
	private Vector3 orgScale;
	// Use this for initialization
	void Start () {
		if (rotSpeed == 0.0f)
			rotSpeed = 50.0f;

		scaleSpeed = 1.0f;
		dir = 1.0f;
		orgScale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(Time.deltaTime * rotSpeed, Time.deltaTime * rotSpeed, Time.deltaTime * rotSpeed);

		scaleFloat += Time.deltaTime * dir * scaleSpeed;
		 if (scaleFloat > 1.0f)
			dir = -1.0f;
		else if (scaleFloat < 0.0f)
			dir = 1.0f;

		float finalFloat = Mathf.Cos(scaleFloat);
		transform.localScale = orgScale * finalFloat;
	}
}

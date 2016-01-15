using UnityEngine;
using System.Collections;

public class Targeting_CubeScript : MonoBehaviour {

	private float rotSpeed;
	// Use this for initialization
	void Start () {
		if (rotSpeed == 0.0f)
			rotSpeed = 50.0f;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(Time.deltaTime * rotSpeed, Time.deltaTime * rotSpeed, Time.deltaTime * rotSpeed);
	}
}

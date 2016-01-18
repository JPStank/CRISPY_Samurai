using UnityEngine;
using System.Collections;

public class SpawnCircleBehavior : MonoBehaviour 
{
    public Transform outer;
    public Transform inner;

    public float speed = 90.0f;
	
	void Update () 
    {
        outer.Rotate(Vector3.forward, speed * Time.deltaTime);
        inner.Rotate(Vector3.forward, -0.5f * speed * Time.deltaTime);
	}
}

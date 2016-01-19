using UnityEngine;
using System.Collections;

public class SwishParticleBehavior : MonoBehaviour 
{
    public ParticleSystem ps;

    private Vector3 originalRotation;

    public Vector3 endRotation;

    public float duration = 0.1f;

    private float timer = 0.0f;

	void Start () 
    {
	    if (ps == null)
        {
            ps = GetComponentInChildren<ParticleSystem>();
            if (ps == null)
            {
                Debug.LogError("Must have Particle System added as child");
            }
        }

        originalRotation = transform.eulerAngles;
	}
	
	void Update() 
    {

        transform.Rotate(Vector3.right, -360.0f * Time.deltaTime);
        //timer += Time.deltaTime;
        //if (timer >= duration)
        //{
        //    Destroy(gameObject);
        //}
        //transform.rotation = Quaternion.Slerp(Quaternion.Euler(originalRotation), Quaternion.Euler(endRotation), timer / duration);
	}
}

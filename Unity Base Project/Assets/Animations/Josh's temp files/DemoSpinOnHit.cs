using UnityEngine;
using System.Collections;

public class DemoSpinOnHit : MonoBehaviour 
{
    public Transform poleToSpin;
    public float spinSpeed;
    public GameObject sparkEffect;

    private Quaternion oldPoleRotation;
    private bool spinning = false;
    private float spinTimer = 0.0f;
    private float spinTime = 1.0f;

	// Use this for initialization
	void Start () 
    {
        oldPoleRotation = poleToSpin.rotation;
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if (spinning)
        {
            poleToSpin.Rotate(Vector3.up, spinSpeed * Time.deltaTime);
            spinTimer += Time.deltaTime;
            if (spinTimer >= spinTime)
            {
                spinTimer = 0.0f;
                spinning = false;
                poleToSpin.rotation = oldPoleRotation;
            }
        }
	}

    void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "Weapon")
        {
            Instantiate(sparkEffect, other.contacts[0].point, Quaternion.identity);
            //other.contacts[0].point;

            spinning = true;
        }
    }
}

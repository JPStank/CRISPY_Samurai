using UnityEngine;
using System.Collections;

public class BloodyBag : MonoBehaviour 
{
    public GameObject bloodEffect;
	public float hitCost = 1.0f;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "Weapon")
        {
            Weapon w = other.gameObject.GetComponent<Weapon>();
            if (w)
            {
                if (w.owner.canHit)
                {
                    w.owner.canHit = false;
                    Instantiate(bloodEffect, other.contacts[0].point, bloodEffect.transform.rotation);                    
                }
            }
			//other.gameObject.GetComponent<>();
        }
    }
}

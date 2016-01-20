using UnityEngine;
using System.Collections;

public class BloodyBag : MonoBehaviour 
{
    public GameObject bloodEffect;
	public float hitCost = 1.0f;
	public GameObject player;

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
			Weapon temp = other.gameObject.GetComponent<Weapon>();
			if (temp)
			{
				//temp.owner.canHit = false;
				Instantiate(bloodEffect, other.contacts[0].point, bloodEffect.transform.rotation);

				if (player)
				{
					player.GetComponent<PuppetScript>().curBalance -= (hitCost * 5.0f);
					if (player.GetComponent<PuppetScript>().curBalance < 0.0f)
						player.GetComponent<PuppetScript>().curBalance = 0.0f;
				}

				if (temp.owner.tag == "Player")
				{
					temp.owner.GetComponent<PuppetScript>().curBalance += (hitCost * 3.0f);
					if (temp.owner.GetComponent<PuppetScript>().curBalance > temp.owner.GetComponent<PuppetScript>().maxBalance)
						temp.owner.GetComponent<PuppetScript>().curBalance = temp.owner.GetComponent<PuppetScript>().maxBalance;
				}
			}

			//other.gameObject.GetComponent<>();
        }
    }
}

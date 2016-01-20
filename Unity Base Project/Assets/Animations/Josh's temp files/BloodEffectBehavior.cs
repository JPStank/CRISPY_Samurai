using UnityEngine;
using System.Collections;

public class BloodEffectBehavior : MonoBehaviour 
{

    public AudioSource sfx;

    // Use this for initialization
    void Start()
    {
        if (sfx == null)
        {
            sfx = gameObject.GetComponent<AudioSource>();
        }
    }

    // Update is called once per frame
    void Update()
    {
		Destroy(gameObject, 4.5f);
    }
}

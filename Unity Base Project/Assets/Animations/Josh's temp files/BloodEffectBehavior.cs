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
        // kill self after 1 second
        Destroy(gameObject, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {

    }
}

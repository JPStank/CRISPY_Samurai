using UnityEngine;
using System.Collections;

public class Action : MonoBehaviour
{
    protected bool behaving = false;
    protected PuppetScript puppet;
	// Use this for initialization
	void Start ()
	{
        puppet = GetComponent<PuppetScript>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	virtual public COMPLETION_STATE Execute()
	{
        return COMPLETION_STATE.INVALID;
	}

    public bool isBehaving()
    {
        return behaving;
    }
}

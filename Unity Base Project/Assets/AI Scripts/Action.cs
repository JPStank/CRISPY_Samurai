using UnityEngine;
using System.Collections;

public class Action : ScriptableObject
{
    protected bool behaving = false;
    public PuppetScript puppet;
	public Animation animation;
	// Use this for initialization
	public void Start()
	{

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

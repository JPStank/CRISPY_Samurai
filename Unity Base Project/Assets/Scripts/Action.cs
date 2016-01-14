using UnityEngine;
using System.Collections;

public class Action : MonoBehaviour
{
    bool behaving = false;
	// Use this for initialization
	void Start ()
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

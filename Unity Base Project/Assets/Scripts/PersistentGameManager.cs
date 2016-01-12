using UnityEngine;
using System.Collections;

public class PersistentGameManager : MonoBehaviour {

    private static PersistentGameManager theGameManager = null;
	// Use this for initialization
	void Start () {
        if (theGameManager == null)
        {
            theGameManager = this;
            DontDestroyOnLoad(theGameManager);
        }
        else
        {
            DestroyImmediate(this);
            return;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

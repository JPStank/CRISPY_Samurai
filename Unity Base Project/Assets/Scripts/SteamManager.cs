using UnityEngine;
using System.Collections;

public class SteamManager : MonoBehaviour 
{
    public GameObject topSpawn;
    public GameObject leftSpawn;
    public GameObject rightSpawn;
    public GameObject steamEffect;

    public bool isAI = true;

    //public Vector3 test;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    void SpawnTop()
    {
        if (isAI)
            Instantiate(steamEffect, topSpawn.transform.position, Quaternion.Euler(270.0f, 90.0f, 0.0f));
    }
    void SpawnLeft()
    {
        if (isAI)
            Instantiate(steamEffect, leftSpawn.transform.position, Quaternion.Euler(0.0f, 0.0f, 0.0f));
    }
    void SpawnRight()
    {
        if (isAI)
            Instantiate(steamEffect, rightSpawn.transform.position, Quaternion.Euler(0.0f, -180.0f, 0.0f));
    }
}

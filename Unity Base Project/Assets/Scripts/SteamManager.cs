using UnityEngine;
using System.Collections;

public class SteamManager : MonoBehaviour 
{
    public GameObject topSpawn;
    public GameObject leftSpawn;
    public GameObject rightSpawn;
    public GameObject steamEffect;

    public bool isAI = true;
    public Vector3 test;

    private Vector3 leftOffset = new Vector3(0.0f, 90.0f, 0.0f);
    private Vector3 rightOffset = new Vector3(0.0f, 270.0f, 0.0f);
    private Vector3 topOffset = new Vector3(270.0f, 0.0f, 0.0f);

    void SpawnTop()
    {
        if (isAI)
            Instantiate(steamEffect, topSpawn.transform.position, Quaternion.Euler(transform.eulerAngles + topOffset));
    }
    void SpawnLeft()
    {
        if (isAI)
            Instantiate(steamEffect, leftSpawn.transform.position, Quaternion.Euler(transform.eulerAngles + leftOffset));
    }
    void SpawnRight()
    {
        if (isAI)
            Instantiate(steamEffect, rightSpawn.transform.position, Quaternion.Euler(transform.eulerAngles + rightOffset));
    }
}

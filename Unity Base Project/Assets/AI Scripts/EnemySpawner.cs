using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour 
{
    public GameObject spawnPoint = null;

    public GameObject[] enemies;

    public GameObject currEnemy;

    int index = 0; // use this to browse the list of enemies

	// Use this for initialization
	void Start () 
    {
        if (enemies.Length == 0)
        {
            Debug.LogWarning("YOU MUST SET ENEMIES IDIOT!!");
        }
        if (spawnPoint == null)
        {
            Debug.LogWarning("YOU MUST SET SPAWN LOCATION IDIOT!!");
        }
        //currEnemy = enemies[index];
        currEnemy = (GameObject)Instantiate(enemies[index], spawnPoint.transform.position, Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () 
    {
        PuppetScript p = currEnemy.GetComponent<PuppetScript>();

        if (p && p.curBalance <= 0.0f)
        {
            //p.ChangeState(PuppetScript.State.DEAD);
            Destroy(currEnemy, currEnemy.GetComponent<Animation>()["Death"].length + 0.5f);
            currEnemy = null;

            index++;
            if (index >= enemies.Length)
            {
                index = 0;
            }

            currEnemy = (GameObject)Instantiate(enemies[index], spawnPoint.transform.position, Quaternion.identity);
        }
	}
}

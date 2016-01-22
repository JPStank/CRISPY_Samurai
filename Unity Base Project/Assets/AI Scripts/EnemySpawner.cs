using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour 
{
    public GameObject spawnPoint = null;
    public bool singleSpawn = false;

    [Header("Spawn Points and Enemies must be same size")]
    public GameObject[] spawnPoints = new GameObject[3];
    public GameObject[] enemies;
    
    private List<GameObject> enemiesInScene;

    public GameObject currEnemy;

    public ArenaDoors doorFriends;
    public int killsRequired = 3;
    int killCount = 0;

    public float waitTime = 2.0f;
    private float timer = 0.0f;
    private bool waiting = false;

    int index = 0; // use this to browse the list of enemies
    List<int> toRemove = new List<int>(); // things to check for removal

	// Use this for initialization
	void Start () 
    {
        toRemove.Clear(); // ensure this is empty for later logic
        if (enemies.Length == 0)
        {
            Debug.LogWarning("YOU MUST SET ENEMIES IDIOT!!");
        }
        if (spawnPoint == null && singleSpawn)
        {
            Debug.LogWarning("YOU MUST SET SPAWN POINT IDIOT!!");
        }
        if (doorFriends == null)
        {
            Debug.LogWarning("I NEED DOOR FRIENDS PLS!!");
        }
        if (singleSpawn) // spawn first enemy
            currEnemy = (GameObject)Instantiate(enemies[index], spawnPoint.transform.position, Quaternion.identity);
        //else // spawn all of them
        //{
        //    if (enemies.Length == 0)
        //    {
        //        Debug.LogError("MULTI SPAWN ENABLED BUT NO ENEMIES GIVEN");
        //    }
        //    enemiesInScene = new GameObject[enemies.Length];
        //    for (int i = 0; i < enemies.Length; ++i)
        //    {
        //        enemiesInScene[i] = (GameObject)Instantiate(enemies[i], spawnPoints[i].transform.position, Quaternion.identity);
        //    }
        //}
	}
	public void SpawnAll()
    {
        killsRequired = enemies.Length;
        if (enemies.Length == 0)
        {
            Debug.LogError("MULTI SPAWN ENABLED BUT NO ENEMIES GIVEN");
        }
        if (enemies.Length != spawnPoints.Length)
        {
            Debug.LogError("SPAWN POINT NEEDED FOR EACH ENEMY");
        }
        enemiesInScene = new List<GameObject>();
        for (int i = 0; i < enemies.Length; ++i)
        {
            enemiesInScene.Add((GameObject)Instantiate(enemies[i], spawnPoints[i].transform.position, Quaternion.identity));
        }

    }
	// Update is called once per frame
	void Update () 
    {
        // this will check the status of all enemies
        if (singleSpawn == false && enemiesInScene != null)
        {
            for (int i = 0; i < enemiesInScene.Count; ++i)
            {
                PuppetScript p = enemiesInScene[i].GetComponent<PuppetScript>();

                if (p && p.curTallys <= 0.0f)
                {
                    Destroy(enemiesInScene[i], enemiesInScene[i].GetComponent<Animation>()["Death"].length + 0.5f);
                    toRemove.Add(i); // queue for removal

                    killCount++;
                    if (killCount >= killsRequired)
                    {
                        if (doorFriends)
                            doorFriends.MoveTheDoorsPls(false);
                    }
                }
            }

            // if things need to go, update list
            if (toRemove.Count > 0)
            {
                int remainingEnemies = enemiesInScene.Count - toRemove.Count;
                if (remainingEnemies <= 0) // all are dead
                {
                    enemiesInScene.Clear();
                    enemiesInScene = null;
                    return; // stop now
                }

                foreach(int kill in toRemove)
                {
                    enemiesInScene.RemoveAt(kill);
                }

                toRemove.Clear();
            }
        }
        // this governs single file spawner
        else if (singleSpawn && currEnemy)
        {
            PuppetScript p = currEnemy.GetComponent<PuppetScript>();

            if (p && p.curTallys <= 0.0f)
            {
                //p.ChangeState(PuppetScript.State.DEAD);
                //GameObject.FindGameObjectWithTag("Player").GetComponent<PuppetScript>().RemoveEnemy(currEnemy);
                Destroy(currEnemy, currEnemy.GetComponent<Animation>()["Death"].length + 0.5f);
                currEnemy = null;
                
                killCount++;
                if (killCount >= killsRequired)
                {
                    if (doorFriends)
                        doorFriends.MoveTheDoorsPls(false);
                }

                index++;
                if (index >= enemies.Length)
                {
                    index = 0;
                }
                waiting = true;
            }
        }
        if (waiting)
        {
            timer += Time.deltaTime;

            if (timer >= waitTime)
            {
                timer = 0.0f;
                waiting = false;
                currEnemy = (GameObject)Instantiate(enemies[index], spawnPoint.transform.position, Quaternion.identity);
            }
        }



	}
}

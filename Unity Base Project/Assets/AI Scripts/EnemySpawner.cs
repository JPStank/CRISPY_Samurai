using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour 
{
    public GameObject spawnPoint = null;

    public GameObject[] enemies;

    public GameObject currEnemy;



    public float waitTime = 2.0f;
    private float timer = 0.0f;
    private bool waiting = false;

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
            Debug.LogWarning("YOU MUST SET SPAWN POINT IDIOT!!");
        }
        //currEnemy = enemies[index];
        currEnemy = (GameObject)Instantiate(enemies[index], spawnPoint.transform.position, Quaternion.identity);

	}
	
	// Update is called once per frame
	void Update () 
    {
        if (currEnemy)
        {
            PuppetScript p = currEnemy.GetComponent<PuppetScript>();

            if (p && p.curTallys <= 0.0f)
            {
                //p.ChangeState(PuppetScript.State.DEAD);
                //GameObject.FindGameObjectWithTag("Player").GetComponent<PuppetScript>().RemoveEnemy(currEnemy);
                Destroy(currEnemy, currEnemy.GetComponent<Animation>()["Death"].length + 0.5f);
                currEnemy = null;

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

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI_Monitor : MonoBehaviour
{
	public List<AI_Controller> enemies;
	public int canAttack = 0; // Josh: Trying to force linear iteration of enemies
	// Use this for initialization
	void Start()
	{
		enemies = new List<AI_Controller>();
		ReadyToConnect();
	}

	// Update is called once per frame
	void Update()
	{

	}

	public int AddEnemy(AI_Controller addMe)
	{
		int ID;
		for (ID = 0; ID < enemies.Count; ID++)
		{
			if (enemies[ID] == addMe)
				break;

		}
		if(ID == enemies.Count)
			enemies.Add(addMe);
		return ID;
	}

	public void RemoveEnemy(int ID)
	{
		enemies.Clear();
		ReadyToConnect();
		AttackDone();
	}

	public bool CanIAttack(int ID)
	{
		return ID == canAttack;
	}

	public void AttackDone()
	{
        // simply iterate the list
        canAttack++;
        if (canAttack >= enemies.Count)
        {
            canAttack = 0;
        }
		//canAttack = Random.Range(0, enemies.Count); // Josh: make this behavior enabled on a bool?
	}

	public void ReadyToConnect()
	{
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		foreach (GameObject enemy in enemies)
		{
			enemy.SendMessage("ConnectToMonitor",this, SendMessageOptions.DontRequireReceiver);
		}
	}
}

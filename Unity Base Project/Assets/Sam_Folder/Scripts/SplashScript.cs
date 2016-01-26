using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SplashScript : MonoBehaviour
{
	public float timer;
	float initTime;
	public GameObject publisher, studio;

	// Use this for initialization
	void Start () 
	{
		initTime = timer;
		studio.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () 
	{
		timer -= Time.deltaTime;
		if(timer <= (initTime * 0.5f))
		{
			publisher.SetActive(false);
			studio.SetActive(true);
		}
		if(timer <= 0.0f)
		{
			Application.LoadLevel("Menu_Scene");
		}
	}
}

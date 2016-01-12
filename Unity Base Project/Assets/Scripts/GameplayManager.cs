using UnityEngine;
using System.Collections;

public class GameplayManager : MonoBehaviour {

	// Use this for initialization
	private PersistentGameManager m_pPersistentGameManager = null;
	public float m_fGameTimeRemaining;
	public float m_fGameTime = 120.0f;
	void Start () {
		m_pPersistentGameManager = FindObjectOfType<PersistentGameManager>(); // Now we have all the information we need from other scenes.
		m_fGameTimeRemaining = m_fGameTime;
	}
	
	// Update is called once per frame
	void Update () 
	{
		m_fGameTimeRemaining -= Time.deltaTime;
		if (m_fGameTimeRemaining < 0.0f)
			Application.LoadLevel("Credits_Scene");

		CheckInput();
		// Do stuff here based on Input such as enabling and disabling menus
	}

	void CheckInput()		// Check Input here and adjust variables accordingly
	{

	}
}

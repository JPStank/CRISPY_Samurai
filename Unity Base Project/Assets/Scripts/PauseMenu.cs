using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class PauseMenu : MonoBehaviour
{
	public GameObject GUI, Menu;
	public bool isPaused = false;
	bool action = true;
	GameObject player;

	public int selectedButton = 0;

	public List<Button> buttons;

	//public List<Toggle> toggles;

	//int selectedToggle = 0;

	// Use this for initialization
	void Start ()
	{
		player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.START, InputChecker.BUTTON_STATE.UP) &&
			InputChecker.GetAxis(InputChecker.PLAYER_NUMBER.ONE, InputChecker.JOYSTICK.LEFT, InputChecker.AXIS.Y) == 0)
		{
			action = false;
		}

		//if (isPaused)
		//{
		buttons[selectedButton].Select();
		//}

		if (InputChecker.GetAxis(InputChecker.PLAYER_NUMBER.ONE, InputChecker.JOYSTICK.LEFT, InputChecker.AXIS.Y) < 0 && !action && isPaused)
		{
			selectedButton--;
			if (selectedButton < 0)
			{
				selectedButton = 1;
			}
			action = true;
		}
		if (InputChecker.GetAxis(InputChecker.PLAYER_NUMBER.ONE, InputChecker.JOYSTICK.LEFT, InputChecker.AXIS.Y) > 0 && !action && isPaused)
		{
			selectedButton++;
			if (selectedButton > 1)
			{
				selectedButton = 0;
			}
			action = true;

		}
		if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.A, InputChecker.BUTTON_STATE.DOWN) && !action && isPaused)
		{
			buttons[selectedButton].onClick.Invoke();
			action = true;
		}


		if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.START, InputChecker.BUTTON_STATE.DOWN) && !action)
		{
			action = true;
			Pause();
		}
		

		//if (InputChecker.GetAxis(InputChecker.PLAYER_NUMBER.ONE, InputChecker.JOYSTICK.LEFT, InputChecker.AXIS.Y) < 0 && !action && isPaused)
		//{
		//	selectedToggle--;
		//	if (selectedToggle < 0)
		//	{
		//		selectedToggle = 2;
		//	}
		//	action = true;
		//}
		//else if (InputChecker.GetAxis(InputChecker.PLAYER_NUMBER.ONE, InputChecker.JOYSTICK.LEFT, InputChecker.AXIS.Y) > 0 && !action && isPaused)
		//{
		//	selectedToggle++;
		//	if (selectedToggle > 2)
		//	{
		//		selectedToggle = 0;
		//	}
		//	action = true;

		//}
		//if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.A, InputChecker.BUTTON_STATE.DOWN) && !action && isPaused)
		//{
		//	toggles[selectedToggle].isOn = !toggles[selectedToggle].isOn;
		//	action = true;
		//}

	}

	public void Pause()
	{
		Menu.gameObject.SetActive(!Menu.gameObject.activeSelf);
		GUI.GetComponent<Canvas>().enabled = !GUI.GetComponent<Canvas>().enabled;
		isPaused = !isPaused;
		if (isPaused)
			Time.timeScale = 0;
		else
			Time.timeScale = 1;
	}

	public void InvertY()
	{
		if (player)
		{
			player.GetComponent<PuppetCameraScript>().invertY = !player.GetComponent<PuppetCameraScript>().invertY;
		}
	}

	public void InvertX()
	{
		if (player)
		{
			player.GetComponent<PuppetCameraScript>().invertX = !player.GetComponent<PuppetCameraScript>().invertX;
		}
	}

	public void Mute()
	{
		GameObject temp = GameObject.Find("RUN DMC");
		if (temp)
		{
			temp.GetComponent<AudioSource>().mute = !temp.GetComponent<AudioSource>().mute;
		}
	}

	public void Exit()
	{
		Pause();
		Application.LoadLevel("Menu_Scene");
	}

}

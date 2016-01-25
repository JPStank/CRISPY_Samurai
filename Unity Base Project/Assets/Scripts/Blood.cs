using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class Blood : MonoBehaviour
{

	private Image left, right, top, bottom;
	PuppetScript player;

	float lastBalance;

	public float maxFadeTime;
	float fadeTimer;

	bool isVisible = false;

	// Use this for initialization
	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<PuppetScript>();

		left = GameObject.Find("GUI/Panel/Blood/Left").GetComponent<Image>();
		right = GameObject.Find("GUI/Panel/Blood/Right").GetComponent<Image>();
		top = GameObject.Find("GUI/Panel/Blood/Top").GetComponent<Image>();
		bottom = GameObject.Find("GUI/Panel/Blood/Bottom").GetComponent<Image>();

		if (player)
		{
			lastBalance = player.curTallys;
		}

		fadeTimer = maxFadeTime;
	}

	// Update is called once per frame
	void Update()
	{
		if (player && left && right && top && bottom)
		{
			if (lastBalance > player.curTallys)
			{
				isVisible = true;

				// Color white (1, 1, 1, 1) uses full alpha with no modified hue
				left.color = Color.white;
				right.color = Color.white;
				top.color = Color.white;
				bottom.color = Color.white;

				lastBalance = player.curTallys;
				fadeTimer = maxFadeTime;

			}
		}

		if (isVisible && left && right && top && bottom)
		{
			fadeTimer -= Time.deltaTime;

			float alphaRatio = fadeTimer / maxFadeTime;

			left.color = new Color(1.0f, 1.0f, 1.0f, alphaRatio);
			right.color = new Color(1.0f, 1.0f, 1.0f, alphaRatio);
			top.color = new Color(1.0f, 1.0f, 1.0f, alphaRatio);
			bottom.color = new Color(1.0f, 1.0f, 1.0f, alphaRatio);

			if (fadeTimer <= 0.0f)
			{
				isVisible = false;
				fadeTimer = maxFadeTime;
			}

		}
	}
}

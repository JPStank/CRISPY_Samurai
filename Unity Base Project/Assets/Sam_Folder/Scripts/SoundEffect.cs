using UnityEngine;
using System.Collections;

public class SoundEffect : MonoBehaviour
{
	public AudioSource sfx;
	public AudioSource[] variety;

	// Use this for initialization
	void Start () 
	{
		sfx = variety[Random.Range(0, variety.Length)];
		sfx.PlayDelayed(0.3f);
		Destroy(gameObject, 1.0f);
	}
}

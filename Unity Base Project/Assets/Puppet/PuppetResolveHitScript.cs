using UnityEngine;
using System.Collections;

public class PuppetResolveHitScript : MonoBehaviour {

	private PuppetScript Owner;

	// Use this for initialization
	void Start () {
	
	}
	
	public void Initialize(PuppetScript _owner)
	{
		if (_owner != null)
			Owner = _owner;
	}

	// Update is called once per frame
	void Update () {
	
	}

	// Resolve collisions when an attack is made.
	// returns 1 on success, -1 on failure
	public int ResolveHit(GameObject _otherObject)
	{
		// seperation of Player and Enemy.
		if (tag == "Player")
			return ResolveHit_Player(_otherObject);
		else if (tag == "Enemy")
			return ResolveHit_Enemy(_otherObject);
		return -1;
	}
	
	// Resolve collisions when an attack is made.
	// Player version!
	public int ResolveHit_Player(GameObject _enemyObject)
	{
		PuppetScript otherScript = _enemyObject.GetComponent<PuppetScript>();
		PuppetScript.State otherState = otherScript.curState;
		Vector3 fromOtherDir = transform.position - _enemyObject.transform.position;
		fromOtherDir.Normalize();

		string toPlay = Owner.animTable[(int)Owner.curState, (int)otherState];
		if (toPlay != null)
		{
			//if (degubber)
			//degubber.GetComponent<DebugMonitor>().UpdateText("New Anim: " + toPlay);

			animation.Play(toPlay);

			if (toPlay == "Idle")
				Owner.ChangeState(PuppetScript.State.IDLE);
			if (toPlay == "React Front" || toPlay == "React Side")
			{

				if (!Owner.godMode)
				{
					if (Owner.flashScript)
						Owner.flashScript.StartFlash();
					if (Owner.bloodHit && Owner.painEffect)
					{
						Instantiate(Owner.painEffect, transform.position, transform.rotation);
						Instantiate(Owner.bloodHit, transform.position, transform.rotation);
					}
					bool armorBlocked = false;
					if (Owner.armor != null)
					{
						Armor.ARMOR_PIECE pieceAffected = Armor.ARMOR_PIECE.INVALID;
						switch (otherState)
						{
							case PuppetScript.State.ATK_VERT:
								pieceAffected = Armor.ARMOR_PIECE.TOP;
								break;
							case PuppetScript.State.ATK_LTR:
								pieceAffected = Armor.ARMOR_PIECE.RIGHT;
								break;
							case PuppetScript.State.ATK_RTL:
								pieceAffected = Armor.ARMOR_PIECE.LEFT;
								break;
							case PuppetScript.State.ATK_STAB:
								pieceAffected = Armor.ARMOR_PIECE.CHEST;
								break;
						}
						if (pieceAffected != Armor.ARMOR_PIECE.INVALID)
							armorBlocked = Owner.armor.ProcessHit(pieceAffected);
						else
							Debug.Log("Invalid armor checking! Please debug and investigate!");
					}
					if (!armorBlocked)
						Owner.curTallys--;
				}
				if (Owner.curTallys <= 0.0f)
				{
					gameObject.layer = 10;
					if (Owner.bloodPool)
					{
						Instantiate(Owner.bloodPool, gameObject.transform.position, gameObject.transform.rotation);
						//Destroy(bloodFX, 2.0f);
					}
					animation.Play("Death");
					Owner.ChangeState(PuppetScript.State.DEAD);
					Owner.curTallys = 0.0f;
					return 1;
				}
				else if (otherState == PuppetScript.State.ATK_LTR
					|| otherState == PuppetScript.State.ATK_RTL
					|| otherState == PuppetScript.State.ATK_VERT)
				{
					rigidbody.AddForce(50000.0f * fromOtherDir);
				}

				//if (gameObject.tag == "Enemy")
				//{
				//	PuppetScript playerPuppet = GameObject.FindGameObjectWithTag("Player").GetComponent<PuppetScript>();
				//	playerPuppet.curTallys++;
				//	if (playerPuppet.curTallys > playerPuppet.maxTallys)
				//		playerPuppet.curTallys = playerPuppet.maxTallys;
				//}

				// New things, added by Dakota 1/13 whatever PM
				Owner.ChangeState(PuppetScript.State.FLINCH);
			}
		}
		return 1;
	}

	// Resolve collisions when an attack is made.
	// Enemy version!
	public int ResolveHit_Enemy(GameObject _playerObject)
	{
		PuppetScript otherScript = _playerObject.GetComponent<PuppetScript>();
		PuppetScript.State otherState = otherScript.curState;
		Vector3 fromOtherDir = transform.position - _playerObject.transform.position;
		fromOtherDir.Normalize();

		string toPlay = Owner.animTable[(int)Owner.curState, (int)otherState];
		if (toPlay != null)
		{
			//if (degubber)
			//degubber.GetComponent<DebugMonitor>().UpdateText("New Anim: " + toPlay);

			animation.Play(toPlay);

			if (toPlay == "Idle")
				Owner.ChangeState(PuppetScript.State.IDLE);
			if (toPlay == "React Front" || toPlay == "React Side")
			{

				if (!Owner.godMode)
				{
					if (Owner.flashScript)
						Owner.flashScript.StartFlash();
					if (Owner.bloodHit && Owner.painEffect)
					{
						Instantiate(Owner.painEffect, transform.position, transform.rotation);
						Instantiate(Owner.bloodHit, transform.position, transform.rotation);
					}
					bool armorBlocked = false;
					if (Owner.armor != null)
					{
						Armor.ARMOR_PIECE pieceAffected = Armor.ARMOR_PIECE.INVALID;
						switch (otherState)
						{
							case PuppetScript.State.ATK_VERT:
								pieceAffected = Armor.ARMOR_PIECE.TOP;
								break;
							case PuppetScript.State.ATK_LTR:
								pieceAffected = Armor.ARMOR_PIECE.RIGHT;
								break;
							case PuppetScript.State.ATK_RTL:
								pieceAffected = Armor.ARMOR_PIECE.LEFT;
								break;
							case PuppetScript.State.ATK_STAB:
								pieceAffected = Armor.ARMOR_PIECE.CHEST;
								break;
						}
						if (pieceAffected != Armor.ARMOR_PIECE.INVALID)
							armorBlocked = Owner.armor.ProcessHit(pieceAffected);
						else
							Debug.Log("Invalid armor checking! Please debug and investigate!");
					}
					if (!armorBlocked)
						Owner.curTallys -= 25;
				}
				if (Owner.curTallys <= 0.0f)
				{
					gameObject.layer = 10;
					if (Owner.bloodPool)
					{
						Instantiate(Owner.bloodPool, gameObject.transform.position, gameObject.transform.rotation);
						//Destroy(bloodFX, 2.0f);
					}
					animation.Play("Death");
					Owner.ChangeState(PuppetScript.State.DEAD);
					Owner.curTallys = 0.0f;
					return 1;
				}
				else if (otherState == PuppetScript.State.ATK_LTR
					|| otherState == PuppetScript.State.ATK_RTL
					|| otherState == PuppetScript.State.ATK_VERT)
				{
					rigidbody.AddForce(50000.0f * fromOtherDir);
				}

				if (gameObject.tag == "Enemy")
				{
					PuppetScript playerPuppet = GameObject.FindGameObjectWithTag("Player").GetComponent<PuppetScript>();
					playerPuppet.curTallys += 12.5f;
					if (playerPuppet.curTallys > playerPuppet.maxTallys)
						playerPuppet.curTallys = playerPuppet.maxTallys;
				}

				// New things, added by Dakota 1/13 whatever PM
				Owner.ChangeState(PuppetScript.State.FLINCH);
			}
		}
		return 1;
	}




}

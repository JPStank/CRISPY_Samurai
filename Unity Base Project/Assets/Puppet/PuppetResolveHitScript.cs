using UnityEngine;
using System.Collections;

public class PuppetResolveHitScript : MonoBehaviour
{

	private PuppetScript Owner;

	// Use this for initialization
	void Start()
	{

	}

	public void Initialize(PuppetScript _owner)
	{
		if (_owner != null)
			Owner = _owner;
	}

	// Update is called once per frame
	void Update()
	{

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
		PuppetScript enemyScript = _enemyObject.GetComponent<PuppetScript>();
		PuppetScript.State enemyState = enemyScript.curState;
		Vector3 fromOtherDir = transform.position - _enemyObject.transform.position;
		fromOtherDir.Normalize();

		string toPlay = Owner.animTable[(int)Owner.curState, (int)enemyState];
		if (toPlay != null)
		{
			animation.Play(toPlay);

			if (toPlay == "Idle")
				Owner.ChangeState(PuppetScript.State.IDLE);
			else if (toPlay == "React Front" || toPlay == "React Side")
			{
				if (!Owner.godMode)
				{
					if (Owner.flashScript) Owner.flashScript.StartFlash();
					if (Owner.bloodHit && Owner.painEffect)
					{
						Instantiate(Owner.painEffect, transform.position, transform.rotation);
						Instantiate(Owner.bloodHit, transform.position, transform.rotation);
					}
					bool armorBlocked = false;
					if (Owner.armor != null)
					{
						Armor.ARMOR_PIECE pieceAffected = Armor.ARMOR_PIECE.INVALID;
						switch (enemyState)
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
						Owner.curBalance -= 25;
				}
				if (Owner.curBalance <= 0.0f)
				{
					gameObject.layer = 10;
					if (Owner.bloodPool) Instantiate(Owner.bloodPool, transform.position, transform.rotation);
					animation.Play("Death");
					Owner.ChangeState(PuppetScript.State.DEAD);
					Owner.curBalance = 0.0f;
					return 1;
				}
				else if (enemyScript.IsAttackState()) rigidbody.AddForce(50000.0f * fromOtherDir);


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
			animation.Play(toPlay);

			if (toPlay == "Idle")
				Owner.ChangeState(PuppetScript.State.IDLE);
			else if (toPlay == "React Front" || toPlay == "React Side")
			{
				if (!Owner.godMode)
				{
					if (Owner.flashScript) Owner.flashScript.StartFlash();
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
						Owner.curBalance -= 25;
				}
				if (Owner.curBalance <= 0.0f)
				{
					gameObject.layer = 10;
					if (Owner.bloodPool) Instantiate(Owner.bloodPool, gameObject.transform.position, gameObject.transform.rotation);
					animation.Play("Death");
					Owner.ChangeState(PuppetScript.State.DEAD);
					Owner.curBalance = 0.0f;
					return 1;
				}
				// Apply knockback
				else if (otherScript.IsAttackState()) rigidbody.AddForce(50000.0f * fromOtherDir);

				// only for enemies
				otherScript.curBalance += 12.5f;
				if (otherScript.curBalance > otherScript.maxBalance)
					otherScript.curBalance = otherScript.maxBalance;

				// New things, added by Dakota 1/13 whatever PM
				Owner.ChangeState(PuppetScript.State.FLINCH);
			}
		}
		return 1;
	}




}

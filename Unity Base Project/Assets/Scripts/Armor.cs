using UnityEngine;
using System.Collections;

// Important!! Integrity Order in the array is as follows: Top - Left - Right - Chest

public class Armor : MonoBehaviour
{
	public enum ARMOR_PIECE { TOP = 0, LEFT, RIGHT, CHEST, INVALID };
	public float[] integrity;
	public float TopIntegrity, LeftIntegrity, RightIntegrity, ChestIntegrity;
	public float TopIntegrityDegredation;
	public float LeftIntegrityDegredation;
	public float RightIntegrityDegredation;
	public float ChestIntegrityDegredation;

	// Use this for initialization
	void Start()
	{
		integrity = new float[4];
		integrity[(int)ARMOR_PIECE.TOP] = TopIntegrity;
		integrity[(int)ARMOR_PIECE.LEFT] = LeftIntegrity;
		integrity[(int)ARMOR_PIECE.RIGHT] = RightIntegrity;
		integrity[(int)ARMOR_PIECE.CHEST] = ChestIntegrity;
	}

	// Update is called once per frame
	void Update()
	{

	}


	/// <summary>
	/// This function will take in which armor piece will take the attack.
	/// It will determine if the armor was able to protect against the hit
	/// </summary>
	/// <returns>bool - returns true if armor blocked the hit, false otherwise</returns>
	public bool ProcessHit(ARMOR_PIECE piece)
	{
		bool armorBlocked = false;
		float armorPercentage;
		switch (piece)
		{
			case ARMOR_PIECE.TOP:
				armorPercentage = Random.Range(0.0f, TopIntegrity);
				if (armorPercentage < integrity[(int)ARMOR_PIECE.TOP])
				{
					armorBlocked = true;
					integrity[(int)ARMOR_PIECE.TOP] -= TopIntegrityDegredation;
				}
				break;
			case ARMOR_PIECE.LEFT:
				armorPercentage = Random.Range(0.0f, LeftIntegrity);
				if (armorPercentage < integrity[(int)ARMOR_PIECE.LEFT])
				{
					armorBlocked = true;
					integrity[(int)ARMOR_PIECE.LEFT] -= LeftIntegrityDegredation;
				}
				break;
			case ARMOR_PIECE.RIGHT:
				armorPercentage = Random.Range(0.0f, RightIntegrity);
				if (armorPercentage < integrity[(int)ARMOR_PIECE.RIGHT])
				{
					armorBlocked = true;
					integrity[(int)ARMOR_PIECE.RIGHT] -= RightIntegrityDegredation;
				}
				break;
			case ARMOR_PIECE.CHEST:
				armorPercentage = Random.Range(0.0f, ChestIntegrity);
				if (armorPercentage < integrity[(int)ARMOR_PIECE.CHEST])
				{
					armorBlocked = true;
					integrity[(int)ARMOR_PIECE.CHEST] -= ChestIntegrityDegredation;
				}
				break;
			default:
				break;
		}
		return armorBlocked;
	}
}

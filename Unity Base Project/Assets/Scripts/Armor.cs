using UnityEngine;
using System.Collections;

// Important!! Integrity Order in the array is as follows: Top - Left - Right - Chest

public class Armor : MonoBehaviour
{
	public enum ARMOR_PIECE { TOP, LEFT, RIGHT, CHEST };
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
		integrity[0] = TopIntegrity;
		integrity[1] = LeftIntegrity;
		integrity[2] = RightIntegrity;
		integrity[3] = ChestIntegrity;
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
	bool ProcessHit(ARMOR_PIECE piece)
	{
		bool armorBlocked = false;
		float armorPercentage;
		switch (piece)
		{
			case ARMOR_PIECE.TOP:
				armorPercentage = Random.Range(0.0f, TopIntegrity);
				if (armorPercentage < integrity[0])
				{
					armorBlocked = true;
					integrity[0] -= TopIntegrityDegredation;
				}
				break;
			case ARMOR_PIECE.LEFT:
				armorPercentage = Random.Range(0.0f, LeftIntegrity);
				if (armorPercentage < integrity[1])
				{
					armorBlocked = true;
					integrity[1] -= LeftIntegrityDegredation;
				}
				break;
			case ARMOR_PIECE.RIGHT:
				armorPercentage = Random.Range(0.0f, RightIntegrity);
				if (armorPercentage < integrity[2])
				{
					armorBlocked = true;
					integrity[2] -= RightIntegrityDegredation;
				}
				break;
			case ARMOR_PIECE.CHEST:
				armorPercentage = Random.Range(0.0f, ChestIntegrity);
				if (armorPercentage < integrity[3])
				{
					armorBlocked = true;
					integrity[3] -= ChestIntegrityDegredation;
				}
				break;
			default:
				break;
		}
		return armorBlocked;
	}
}

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
	public MeshRenderer TopPiece, LeftPiece, RightPiece, ChestPiece;

	// Use this for initialization
	void Start()
	{
		integrity = new float[4];
		integrity[(int)ARMOR_PIECE.TOP] = TopIntegrity;
		integrity[(int)ARMOR_PIECE.LEFT] = LeftIntegrity;
		integrity[(int)ARMOR_PIECE.RIGHT] = RightIntegrity;
		integrity[(int)ARMOR_PIECE.CHEST] = ChestIntegrity;
		TopPiece.material.color = Color.green;
		LeftPiece.material.color = Color.green;
		RightPiece.material.color = Color.green;
		ChestPiece.material.color = Color.green;
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
		Debug.Log("Processing " + piece);

		bool armorBlocked = false;
		float armorPercentage;
		switch (piece)
		{
			case ARMOR_PIECE.TOP:
				armorPercentage = Random.Range(0.0f, TopIntegrity);
				Debug.Log(armorPercentage + " " + integrity[(int)ARMOR_PIECE.TOP]);
				if (armorPercentage < integrity[(int)ARMOR_PIECE.TOP])
				{
					armorBlocked = true;
					integrity[(int)ARMOR_PIECE.TOP] -= TopIntegrityDegredation;
					if (integrity[(int)ARMOR_PIECE.TOP] <= 0)
						TopPiece.enabled = false;
				}
				break;
			case ARMOR_PIECE.LEFT:
				armorPercentage = Random.Range(0.0f, LeftIntegrity);
				Debug.Log(armorPercentage + " " + integrity[(int)ARMOR_PIECE.LEFT]);
				if (armorPercentage < integrity[(int)ARMOR_PIECE.LEFT])
				{
					armorBlocked = true;
					integrity[(int)ARMOR_PIECE.LEFT] -= LeftIntegrityDegredation;
					if (integrity[(int)ARMOR_PIECE.LEFT] <= 0)
						LeftPiece.enabled = false;
				}
				break;
			case ARMOR_PIECE.RIGHT:
				armorPercentage = Random.Range(0.0f, RightIntegrity);
				Debug.Log(armorPercentage + " " + integrity[(int)ARMOR_PIECE.RIGHT]);
				if (armorPercentage < integrity[(int)ARMOR_PIECE.RIGHT])
				{
					
					armorBlocked = true;
					integrity[(int)ARMOR_PIECE.RIGHT] -= RightIntegrityDegredation;
					if (integrity[(int)ARMOR_PIECE.RIGHT] <= 0)
						RightPiece.enabled = false;
				}
				break;
			case ARMOR_PIECE.CHEST:
				armorPercentage = Random.Range(0.0f, ChestIntegrity);
				Debug.Log(armorPercentage + " " + integrity[(int)ARMOR_PIECE.CHEST]);
				if (armorPercentage < integrity[(int)ARMOR_PIECE.CHEST])
				{
					armorBlocked = true;
					integrity[(int)ARMOR_PIECE.CHEST] -= ChestIntegrityDegredation;
					if (integrity[(int)ARMOR_PIECE.CHEST] <= 0)
						ChestPiece.enabled = false;
				}
				break;
			default:
				break;
		}
		return armorBlocked;
	}
}

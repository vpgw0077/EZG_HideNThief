using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Itme", menuName = "New Item/item")]
public class Item : ScriptableObject
{
	public ItemName itemType;

	public enum ItemName
	{
		Generator,
		GasCan,
		Rock,
		FlashBang,
		SmokeShell,
		EnergyDrink,
		Battery

	}


}

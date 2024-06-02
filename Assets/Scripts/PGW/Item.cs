using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Itme", menuName = "New Item/item")]
public class Item : ScriptableObject
{
    public ItemName itemType;
    public int itemID;

    public enum ItemName
    {

        Rock,
        FlashBang,
        SmokeShell,
        EnergyDrink,
        Battery,


    }


}

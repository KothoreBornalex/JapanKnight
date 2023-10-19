using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DroppedItem;

[System.Serializable]
public class Weapons
{


    [Header("Global Weapon Informations")]
    public Items weaponName;
    public GameObject weaponDropped;
    public GameObject weaponPrefab;
    public GameObject attackEffect;
    public bool isRangeWeapon;

}

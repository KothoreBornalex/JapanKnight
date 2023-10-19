using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DroppedItem;


public interface IWeapon
{
    public void StartAttack()
    {

    }

    public void StopAttack()
    {

    }

}


[System.Serializable]
public class Weapons
{


    [Header("Global Weapon Informations")]
    public Items weaponName;
    public GameObject weaponDropped;
    public GameObject weaponPrefab;
    public GameObject attackEffect;
    public GameObject attackProjectile;

    public bool isRangeWeapon;

    [Header("Data Weapon Informations")]
    public int weaonDamage;
    public float attackTiming;
    public float weaponCoolDown;

    [Header("Animatin Data")]
    public float weaponRecoil;
    public float smoothInSpeed;
    public float smoothOutSpeed;


}

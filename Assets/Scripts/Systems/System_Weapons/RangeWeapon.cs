using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DroppedItem;

public class RangeWeapon : MonoBehaviour, IWeapon
{
    [SerializeField] private Items item;
    [SerializeField] private Transform _muzzleSpawn;
    [SerializeField] private Transform _bulletSpawn;



    public void StartAttack()
    {
        int currentWeapon = PlayerStateMachine.instance.GetWeaponIndex(item);
        //Instantiation Muzzle effect
        Instantiate<GameObject>(PlayerStateMachine.instance.WeaponsList.WeaponsList[currentWeapon].attackEffect, _muzzleSpawn.position, _muzzleSpawn.rotation);


        Vector2 forwardDirection = transform.up;

        // Calculate the rotation of the pistol in degrees
        float pistolRotation = transform.eulerAngles.z;
        float pistolRotationRad = pistolRotation * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(Mathf.Cos(pistolRotationRad), Mathf.Sin(pistolRotationRad));

        //Instantiation Projectile
        WeaponContactTrigger projectile = Instantiate<GameObject>(PlayerStateMachine.instance.WeaponsList.WeaponsList[currentWeapon].attackProjectile, _bulletSpawn.position, Quaternion.identity).GetComponent<WeaponContactTrigger>();
        projectile.direction = direction;

    }

    public void StopAttack()
    {

    }
}

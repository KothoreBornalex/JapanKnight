using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DroppedItem;

public class SlashWeapon : MonoBehaviour, IWeapon
{

    [SerializeField] private Items item;
    [SerializeField] private bool isLethal;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isLethal && collision.CompareTag("Enemy"))
        {
            int index = PlayerStateMachine.instance.GetWeaponIndex(item);
            collision.GetComponent<IStatistics>().DecreaseStat(IStatistics.StatName.Health, PlayerStateMachine.instance.WeaponsList.WeaponsList[index].weaonDamage);
        }
    }

    public void StartAttack()
    {
        isLethal = true;
    }

    public void StopAttack()
    {
        isLethal = false;
    }
}

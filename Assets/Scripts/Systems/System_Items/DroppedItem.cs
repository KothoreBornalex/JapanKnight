using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : MonoBehaviour, IPickupables
{
    public enum Items
    {
        NoItem,
        Healing,
        Katana,
        Lance,
        Pistolet,
        Fusil,
        Kunai,
        Shuriken,
        DoubleKatana
    }

    [SerializeField] private Items _item;
    [SerializeField] private bool isWeapon;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("trigger working");
        if (collision.CompareTag("Player"))
        {
            PickedUp();
        }
    }

    public void PickedUp()
    {

        if(isWeapon)
        {
            PlayerStateMachine.instance.SetWeapon(_item);
        }
        else
        {
            PlayerStateMachine.instance.IncreaseStat(IStatistics.StatName.Health, 5.0f);
        }

        Destroy(gameObject);
    }
}

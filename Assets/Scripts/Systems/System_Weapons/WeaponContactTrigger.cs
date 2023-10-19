using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DroppedItem;

public class WeaponContactTrigger : MonoBehaviour
{
    [SerializeField] private Items item;
    [SerializeField] private bool isLethal;
    public Vector3 direction;

    private void Update()
    {
        transform.Translate(direction * 4.0f * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isLethal && collision.CompareTag("Enemy"))
        {
            int index = PlayerStateMachine.instance.GetWeaponIndex(item);
            collision.GetComponent<IStatistics>().DecreaseStat(IStatistics.StatName.Health, PlayerStateMachine.instance.WeaponsList.WeaponsList[index].weaonDamage);
        }
    }
}

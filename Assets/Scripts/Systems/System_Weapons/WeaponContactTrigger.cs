using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DroppedItem;

public class WeaponContactTrigger : MonoBehaviour
{
    [SerializeField] private Items item;
    [SerializeField] private Factions _targetedFaction;
    [SerializeField] private bool isLethal;
    public Vector3 direction;

    public Factions TargetedFaction { get => _targetedFaction; set => _targetedFaction = value; }

    public enum Factions
    {
        Player,
        Enemy
    }

    private void Update()
    {
        transform.Translate(direction * 4.0f * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(_targetedFaction == Factions.Enemy)
        {
            if (isLethal && collision.CompareTag("Enemy"))
            {
                int index = PlayerStateMachine.instance.GetWeaponIndex(item);
                collision.GetComponent<IStatistics>().DecreaseStat(IStatistics.StatName.Health, PlayerStateMachine.instance.WeaponsList.WeaponsList[index].weaonDamage);
                Destroy(gameObject);
            }
        }

        if (_targetedFaction == Factions.Player)
        {
            if (isLethal && collision.CompareTag("Player"))
            {
                int index = PlayerStateMachine.instance.GetWeaponIndex(item);
                collision.GetComponent<IStatistics>().DecreaseStat(IStatistics.StatName.Health, PlayerStateMachine.instance.WeaponsList.WeaponsList[index].weaonDamage);
                Destroy(gameObject);
            }
        }

    }
}

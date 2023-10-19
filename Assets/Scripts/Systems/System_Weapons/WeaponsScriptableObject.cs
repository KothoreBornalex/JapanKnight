using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponsList", menuName = "ScriptableObjects/WeaponsList", order = 1)]
public class WeaponsScriptableObject : ScriptableObject
{
    [SerializeField] private List<Weapons> _weaponsList = new List<Weapons>();

    public List<Weapons> WeaponsList { get => _weaponsList;}
}

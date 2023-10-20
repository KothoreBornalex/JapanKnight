using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static IStatistics;

[CreateAssetMenu(fileName = "AI_Data", menuName = "ScriptableObjects/AI_Data", order = 1)]
public class AI_Data : ScriptableObject
{
    [SerializeField] private List<Statistics> _aiStatistics = new List<Statistics>();
    [SerializeField] private GameObject _deathObject;
    [SerializeField, Range(0, 15)] private float _attackRange;


    #region GETTERS & SETTERS
    public float AttackRange { get => _attackRange;}
    public List<Statistics> AiStatistics { get => _aiStatistics;}
    public GameObject DeathObject { get => _deathObject;}


    #endregion

}

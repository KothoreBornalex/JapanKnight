using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static IObjects;
using static IStatistics;

public class AIStats : MonoBehaviour, IStatistics
{
    [Header("Object Stats")]
    private IObjects objectScript;
    [SerializeField] private List<Statistics> _stats;

    public List<Statistics> Stats { get => _stats; set => _stats = value; }

    private void Start()
    {
        objectScript = GetComponent<IObjects>();
    }




    public void SetStat(StatName statName, float statValue)
    {

        foreach(Statistics stats in _stats)
        {
            if(stats._statName == statName)
            {
                stats._statCurrentValue = statValue;
            }
        }

    }

    public void DecreaseStat(StatName statName, float decreasingValue)
    {

        foreach (Statistics stats in _stats)
        {
            if (stats._statName == statName)
            {
                stats._statCurrentValue -= decreasingValue;
                stats._statCurrentValue = Mathf.Clamp(stats._statCurrentValue, 0, stats._statMaxValue);


                // For Actualizing the object state.
                if (stats._statCurrentValue <= 85 && stats._statCurrentValue >= 50)
                {
                    objectScript.SwitchState(ObjectStates.LittleDamaged);
                }

                if (stats._statCurrentValue <= 50 && stats._statCurrentValue >= 1)
                {
                    objectScript.SwitchState(ObjectStates.HighDamaged);
                }

                if (stats._statCurrentValue < 1)
                {
                    objectScript.SwitchState(ObjectStates.Destroyed);
                }
            }
        }




    }

    public void IncreaseStat(StatName statName, float increasingValue)
    {

        foreach (Statistics stats in _stats)
        {
            if (stats._statName == statName)
            {
                stats._statCurrentValue += increasingValue;
                stats._statCurrentValue = Mathf.Clamp(stats._statCurrentValue, 0, stats._statMaxValue);


                if (stats._statCurrentValue >= 85)
                {
                    objectScript.SwitchState(ObjectStates.Perfect);
                }

                if (stats._statCurrentValue <= 85 && stats._statCurrentValue >= 50)
                {
                    objectScript.SwitchState(ObjectStates.LittleDamaged);
                }

                if (stats._statCurrentValue <= 50 && stats._statCurrentValue >= 1)
                {
                    objectScript.SwitchState(ObjectStates.HighDamaged);
                }
            }
        }

    }



}
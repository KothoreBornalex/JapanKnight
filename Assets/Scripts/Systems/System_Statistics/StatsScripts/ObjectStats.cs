using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static IStatistics;
using static IObjects;
using NaughtyAttributes;

public class ObjectStats : MonoBehaviour, IStatistics
{


    [Button("Take Damage")] void TakeDamage() => DecreaseStat(StatName.Health, Random.Range(5, 15));
    [Button("Reset")] void LaunchReset() => Reset();



    [Header("Object Stats")]
    private IObjects objectScript;
    [SerializeField] private Statistics objectHealth;


    private void Reset()
    {
        objectScript = GetComponent<IObjects>();

        objectHealth = new Statistics();
        objectHealth._statName = StatName.Health;
        objectHealth._statMaxValue = 100;
        objectHealth._statCurrentValue = 100;
    }

    private void Start()
    {
        objectScript = GetComponent<IObjects>();
    }

    public void InitializeStats()
    {
        //Since the object only have one stat (Health) I don't initialize anything.
    }


    public void SetStat(StatName statName, float statValue)
    {
        if(statName == objectHealth._statName)
        {
            objectHealth._statCurrentValue = statValue;
        }
    }

    public void DecreaseStat(StatName statName, float decreasingValue)
    {
        //Since I only have one stat in this script, I don't need to do a for each to find the right stat.

        if (statName == objectHealth._statName)
        {
            objectHealth._statCurrentValue -= decreasingValue;
            objectHealth._statCurrentValue = Mathf.Clamp(objectHealth._statCurrentValue, 0, objectHealth._statMaxValue);


            // For Actualizing the object state.
            if (objectHealth._statCurrentValue <= 85 && objectHealth._statCurrentValue >= 50)
            {
                objectScript.SwitchState(ObjectStates.LittleDamaged);
            }

            if (objectHealth._statCurrentValue <= 50 && objectHealth._statCurrentValue >= 1)
            {
                objectScript.SwitchState(ObjectStates.HighDamaged);
            }

            if (objectHealth._statCurrentValue < 1)
            {
                objectScript.SwitchState(ObjectStates.Destroyed);
            }
        }


    }

    public void IncreaseStat(StatName statName, float increasingValue)
    {
        //Since I only have one stat in this script, I don't need to do a for each to find the right stat.

        if (statName == objectHealth._statName)
        {
            objectHealth._statCurrentValue += increasingValue;
            objectHealth._statCurrentValue = Mathf.Clamp(objectHealth._statCurrentValue, 0, objectHealth._statMaxValue);


            if (objectHealth._statCurrentValue >= 85)
            {
                objectScript.SwitchState(ObjectStates.Perfect);
            }

            if (objectHealth._statCurrentValue <= 85 && objectHealth._statCurrentValue >= 50)
            {
                objectScript.SwitchState(ObjectStates.LittleDamaged);
            }

            if (objectHealth._statCurrentValue <= 50 && objectHealth._statCurrentValue >= 1)
            {
                objectScript.SwitchState(ObjectStates.HighDamaged);
            }
        }
        
    }

}

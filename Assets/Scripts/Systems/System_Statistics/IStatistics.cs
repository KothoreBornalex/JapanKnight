using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static IStatistics;

public interface IStatistics
{
    [System.Serializable]
    public class Statistics
    {
        public StatName _statName;
        public float _statCurrentValue;
        public float _statMaxValue;

        public Statistics()
        {
        }

        public Statistics(StatName statName, float statCurrentValue, float statMaxValue)
        {
            _statName = statName;
            _statCurrentValue = statCurrentValue;
            _statMaxValue = statMaxValue;
        }

    }
    

    
    /*
    public enum StatName
    {
        Health,
        Armor,
        Stamina,
        Oxygen,
        GlobalSpeed,
        WalkSpeed,
        SprintSpeed
    }
    */

    public enum StatName
    {
        Health,
        Armor,

        Cloudys,
        Lavaite,
        Poiscaille,
        Almenos,
        Terrah,


        Villager,
        AllUnits,
        Swordmen,
        Lancer,
        Cavalry
    }

    public void InitializeStats()
    {

    }

    public void SetStat(StatName statName, float statValue)
    {

    }

    public void DecreaseStat(StatName statName, float decreasingValue)
    {

    }

    public void IncreaseStat(StatName statName, float increasingValue)
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static IObjects;


public interface IObjects
{
    public enum ObjectStates
    {
        Perfect,
        LittleDamaged,
        HighDamaged,
        Destroyed,
    }

    [System.Serializable]
    public class EffectEmission
    {
        public GameObject Prefab_Effect;
        public Transform SpawnPoint_Effect;
    }

    public void Destroyed()
    {

    }

    public void SwitchState(ObjectStates newState)
    {

    }


    

    


}
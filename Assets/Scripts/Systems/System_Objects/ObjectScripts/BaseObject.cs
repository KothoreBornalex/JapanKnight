using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using static IObjects;
public class BaseObject : MonoBehaviour, IObjects
{
    [Header("Base Object Variables")]
    [SerializeField] private ObjectStates ObjectState;

    [SerializeField] private List<EffectEmission> Destroyed_effectEmissions = new List<EffectEmission>();

    public void Destroyed()
    {
        //Instantiating all the destroyed effects.
        foreach(EffectEmission effect in Destroyed_effectEmissions)
        {
            Instantiate<GameObject>(effect.Prefab_Effect, effect.SpawnPoint_Effect);
        }
    }



    public void SwitchState(ObjectStates newState)
    {
        if(newState == ObjectStates.Perfect)
        {
            ObjectState = ObjectStates.Perfect;
        }

        if (newState == ObjectStates.LittleDamaged)
        {
            ObjectState = ObjectStates.LittleDamaged;
        }

        if (newState == ObjectStates.HighDamaged)
        {
            ObjectState = ObjectStates.HighDamaged;
        }

        if (newState == ObjectStates.Destroyed)
        {
            ObjectState = ObjectStates.Destroyed;
            Destroyed();
        }
    }


}

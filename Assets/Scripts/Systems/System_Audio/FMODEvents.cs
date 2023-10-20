using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{


   
    [field: Header("Music_Sounds")]
    [SerializeField] public EventReference musicMainMenu;
    
    [field: Header("Ambience_Sounds")]
    [SerializeField] public EventReference ambienceMainMenu;

    [field: Header("UI_Sounds")]
    [SerializeField] public EventReference UI_clickSound1;

    [SerializeField] public EventReference UI_selectSound1;

    [SerializeField] public EventReference UI_hoverSound1;

    [SerializeField] public EventReference UI_writeSound1;

    [SerializeField] public EventReference UI_notifSound1;



    [field: Header("Player_Sounds")]
    [SerializeField] public EventReference Player_Hurt;

    [SerializeField] public EventReference Player_Healed;

    [SerializeField] public EventReference Player_WeaponEquiped;


    [field: Header("Weapons_Sounds")]
    [SerializeField] public EventReference Weapons_KatanaSlash;
    [SerializeField] public EventReference Weapons_LanceSlash;
    [SerializeField] public EventReference Weapons_PistolShot;
    [SerializeField] public EventReference Weapons_FusilShot;


    public static FMODEvents instance;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
}

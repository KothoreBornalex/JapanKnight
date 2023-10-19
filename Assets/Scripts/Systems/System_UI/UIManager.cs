using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;


    [Header("UI Panels")]
    [SerializeField] Canvas canvas;

    [Header("URL")]
    [SerializeField] public string Discord_URL;



    [Header("UI Panels")]
    [SerializeField] private GameObject panelNotifs;
    [SerializeField] private GameObject panelHUD;



    [Header("UI Prefabs")]
    [SerializeField] private GameObject prefabNotif;


    [Header("HUD References")]
    [SerializeField] private Slider sliderLife;
    [SerializeField] private Slider sliderStamina;



    #region Initialize Manager Functions


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

    private void Start()
    {

    }


    #endregion





    public void SpawnNotifs(string notifMessage)
    {
        AudioManager.instance.PlayOneShot_GlobalSound(FMODEvents.instance.UI_notifSound1);
        NotifScript notif = Instantiate<GameObject>(prefabNotif, panelNotifs.transform).GetComponent<NotifScript>();

        notif.notifText.SetText(notifMessage);
    }


    #region Global Functions
    public void InitializedUI()
    {
        canvas.worldCamera = CameraManager.instance._camera;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenLink(string link)
    {
        Application.OpenURL(link);
    }

    #endregion

    #region UI Sounds Functions


    public void PlaySound_WritingDeletingSound()
    {
        //AudioManager.instance.PlayOneShot_GlobalSound(FMODEvents.instance.UI_writeSound1);
    }

    public void PlaySound_SelectingInputField()
    {
        //AudioManager.instance.PlayOneShot_GlobalSound(FMODEvents.instance.UI_selectSound1);
    }

    public void PlaySound_ClickingButton()
    {
        //AudioManager.instance.PlayOneShot_GlobalSound(FMODEvents.instance.UI_clickSound1);
    }

    #endregion


    #region Inputs Detection






    #endregion



    #region Toggles Functions

    public void ToggleHud()
    {
        if (panelHUD.activeInHierarchy)
        {
            panelHUD.SetActive(false);
        }
        else
        {
            panelHUD.SetActive(true);
        }
    }

    #endregion


}
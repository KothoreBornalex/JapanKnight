using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Global Variables")]
    public string loggedInScene;
    public string GameScene;
    public GameObject cameraParent;
    private Animator cameraAnimator;


    public GameObject errorWindow;
    public GameObject deleteCharacterWindow;




    [Header("UI Pages")]
    public GameObject Acceuil;
    public GameObject Credits;
    public GameObject Settings;


    [Header("UI Pages Settings")]
    public GameObject keyBindings;
    public GameObject audioSettings;
    public GameObject graphicSettings;







    enum IdentifiantsJoueur
    {
        Mail,
        Password,
        RememberMe
    }
    
    private void Start()
    {

    }





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

    public void StartFunction()
    {
        //Menu.SetActive(false);
        cameraAnimator.SetTrigger("LeaveMenu");
    }



    #region LaunchFunctions


    public void LaunchQuitGame()
    {
        UIManager.instance.QuitGame();
    }

    public void LaunchOpenDiscord()
    {
        string link = UIManager.instance.Discord_URL;
        UIManager.instance.OpenLink(link);
    }

    #endregion 



    public void PlayGame()
    {
        ChangeMap();
    }

    public void ChangeMap()
    {
        LevelManager.instance.LoadScene(GameScene);
    }



    public void SpawnErrorMessage(string errorTitle, string errorText)
    {
        GameObject window = Instantiate<GameObject>(errorWindow, transform);


        ErrorWindow errorWindowScript = window.GetComponent<ErrorWindow>();
        errorWindowScript.errorTitle.SetText(errorTitle);
        errorWindowScript.errorText.SetText(errorText);
    }




    private void Update()
    {

    }

    #region Settings UI Functions

    public void ActiveKeyBindingsUI()
    {
        keyBindings.SetActive(true);

        audioSettings.SetActive(false);
        graphicSettings.SetActive(false);
    }


    public void ActiveAudioSettingsUI()
    {
        audioSettings.SetActive(true);
        
        keyBindings.SetActive(false);
        graphicSettings.SetActive(false);
    }

    public void ActiveGraphicSettingsUI()
    {
        graphicSettings.SetActive(true);
        
        keyBindings.SetActive(false);
        audioSettings.SetActive(false);
    }
    #endregion


    public void OpenCredits()
    {
        Credits.SetActive(true);
    }



    public void OpenSettings()
    {
        Settings.SetActive(true);
    }


    public void CloseAnyPanel()
    {
        Settings.SetActive(false);
        Credits.SetActive(false);
    }
}

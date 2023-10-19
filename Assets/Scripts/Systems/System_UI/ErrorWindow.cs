using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ErrorWindow : MonoBehaviour
{
    public TMP_Text errorTitle;
    public TMP_Text errorText;


    public void CloseErrorWindow()
    {
        Destroy(gameObject);
    }
}

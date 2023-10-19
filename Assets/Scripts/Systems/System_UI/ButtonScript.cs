using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode] [RequireComponent(typeof(Image))]
public class ButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{

    [Header("Button Events")]
    [SerializeField] private UnityEvent PointerEnterEvent;
    [SerializeField] private UnityEvent PointerExitEvent;
    [SerializeField] private UnityEvent PointerClickEvent;


    private Image _button;
    void Start()
    {
        _button = GetComponent<Image>();
    }


    public void OnPointerEnter(PointerEventData eventData)
    {

        AudioManager.instance.PlayOneShot_GlobalSound(FMODEvents.instance.UI_hoverSound1);
   
        PointerEnterEvent.Invoke();
    }



    public void OnPointerExit(PointerEventData eventData)
    {

        PointerExitEvent.Invoke();
    }



    public void OnPointerDown(PointerEventData eventData)
    {
        AudioManager.instance.PlayOneShot_GlobalSound(FMODEvents.instance.UI_clickSound1);
        PointerClickEvent.Invoke();
    }


}


#if UNITY_EDITOR
[CustomEditor(typeof(ButtonScript))]
class ButtonScriptEditor : Editor
{

}
#endif
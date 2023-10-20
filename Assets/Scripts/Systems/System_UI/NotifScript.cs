using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotifScript : MonoBehaviour
{
    [Header("Notif Variables")]
    [SerializeField] private CanvasGroup canvaGroup;
    [SerializeField] private bool notifActivated;
    [SerializeField] public TMP_Text notifText;

    [SerializeField] [Range(2, 8)] private float notifLifeTime;
    private float currenTime;

    // Start is called before the first frame update
    void Awake()
    {
        canvaGroup.alpha = 0;
    }

    void Start()
    {
        notifActivated = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (notifActivated)
        {
            canvaGroup.alpha += Time.deltaTime;
        }
        else
        {
            canvaGroup.alpha -= Time.deltaTime / (notifLifeTime * 0.7f);
        }


        if(canvaGroup.alpha >= 1)
        {
            notifActivated = false;
        }




        currenTime += Time.deltaTime;
        if (currenTime >= notifLifeTime)
        {
            Destroy(gameObject);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyIn : MonoBehaviour
{
    [SerializeField] private float timeBeforeDestroy;
    private float currentTime;


    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;

        if(currentTime > timeBeforeDestroy)
        {
            Destroy(gameObject);
        }
    }
}
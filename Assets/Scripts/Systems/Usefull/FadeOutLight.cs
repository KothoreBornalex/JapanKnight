using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FadeOutLight : MonoBehaviour
{
    [SerializeField, Range(0, 10.0f)] private float fadeOutSpeed;
    private Light2D _light;
    // Start is called before the first frame update
    void Start()
    {
        _light = GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _light.intensity = Mathf.Lerp(_light.intensity, 0, fadeOutSpeed  * Time.deltaTime);
    }
}

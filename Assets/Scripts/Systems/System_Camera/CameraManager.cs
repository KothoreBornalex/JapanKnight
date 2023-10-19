using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    // Global Camera Manager Fields
    private PlayerStateMachine playerStateMachine;

    [SerializeField] private bool _isPlayerCamera;

    [SerializeField] public Camera _camera;
    [SerializeField] public Transform _cameraTransform;

    float _baseFOV;



    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        if (_isPlayerCamera)
        {
            playerStateMachine = PlayerStateMachine.instance;

            _baseFOV = _camera.fieldOfView;
        }
    }

    private void Update()
    {


    }



    public void SwitchingFOV()
    {
        
    }
}

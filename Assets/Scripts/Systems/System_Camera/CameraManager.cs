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
    [SerializeField, Range(0.0f, 10.0f)] public float _cameraSmoothing = 3.0f;

    private Transform _playerTransform;
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
        if (_isPlayerCamera)
        {
            if (_playerTransform != null)
            {
                Vector3 targetPosition = _playerTransform.position;
                targetPosition.z = transform.position.z;
                transform.position = Vector3.Lerp(transform.position, targetPosition, _cameraSmoothing * Time.deltaTime);
            }
            else if (PlayerStateMachine.instance != null)
            {
                _playerTransform = PlayerStateMachine.instance.transform;
            }
        }
        
    }



    public void SwitchingFOV()
    {
        
    }
}

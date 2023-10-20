using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;
using static DroppedItem;
using static IObjects;
using static IStatistics;

public class PlayerStateMachine : MonoBehaviour, IStatistics
{
    public static PlayerStateMachine instance;

    #region Declaring Inspector Buttons
    [Button("Reset")] void LaunchReset() => Reset();
    [Button("Receive Damage")] void Attack() => LoseLP();

    #endregion

    #region Declaring Player State Fields
    public enum PlayerState
    {
        Idle,
        Moving,
        Dead
    }
    [SerializeField] private PlayerState _playerState;
    private float _deathTimer;


    [Header("Weapon Fields")]
    [SerializeField] private Items _playerWeaponName;
    [SerializeField] private int _playerWeaponIndex;
    private IWeapon weaponScript;
    private Transform _playerWeaponTransform;
    private Vector3 _weaponBasePosition;
    private Vector3 _weaponTargetPosition;
    private bool canRotate;
    private bool isAttacking;
    private float _attackTimer;

    #endregion
    [Space(25)]

    #region Declaring Stats Fields
    List<Statistics> _playerStatistics = new List<Statistics>();
    #endregion 

    #region UI Stats For Designers
    [Header("Player Stats Barr")]

    [ProgressBar("Current Health", "_maxHealth", EColor.Red)]
    [SerializeField] float _health;
    float _maxHealth;

    [ProgressBar("Current Stamina", "_maxStamina", EColor.Blue)]
    [SerializeField] float _stamina;
    float _maxStamina;
    #endregion
    [Space(25)]

    #region References Fields
    private Rigidbody2D _rigidbody;
    private CapsuleCollider2D _capsuleCollider;
    private Animator _animator;
    #endregion

    #region HUD Fields
    [Header("HUD Fields")]
    [SerializeField] private Slider _lifeSlider;
    [SerializeField] private Slider _staminaSlider;

    #endregion
    [Space(25)]

    #region Movements Fields

    private float _smoothVertical;
    private float _smoothHorizontal;

    #endregion

    [Header("SO Fields")]
    [SerializeField, Expandable] PlayerDataScriptableObject _playerDataScriptableObject;
    [SerializeField, Expandable] PlayerKeyBindsScriptableObject _keyBindsMap;
    [SerializeField, Expandable] WeaponsScriptableObject _weaponsList;

    private Vector3 _weaponBoneTarget;
    [SerializeField] private Transform _weaponBone;
    [SerializeField] private Transform _weaponHolder;
    [SerializeField] private SpriteRenderer _playerSprite;

    public WeaponsScriptableObject WeaponsList { get => _weaponsList;}
    public Transform WeaponBone { get => _weaponBone;}

    #region OnValidate & Reset Functions
    private void OnValidate()
    {
        foreach (Statistics statistics in _playerStatistics)
        {
            Debug.Log("The Stat is: " + statistics._statName.ToString());
            switch (statistics._statName)
            {
                case StatName.Health:
                    _health = statistics._statCurrentValue;
                    _maxHealth = statistics._statMaxValue;
                    break;

                case StatName.Stamina:
                    _stamina = statistics._statCurrentValue;
                    _maxStamina = statistics._statMaxValue;
                    break;
            }

        }
    }

    private void Reset()
    {
        Debug.Log("Player Has Been Reset");
        _playerStatistics = _playerDataScriptableObject.PlayerStatistics;
    }


    private void LoseLP()
    {
        DecreaseStat(StatName.Health, (int)UnityEngine.Random.Range(1, 3));
    }
    #endregion

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Start()
    {
        // Set Up Cursor
        Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;

        _rigidbody = GetComponent<Rigidbody2D>();
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
        _weaponBasePosition = Vector3.zero;

        Debug.Log("Player Has Been Reset");

        InitializeStats();

    }


    private void Update()
    {
        HandlePlayerStateMachine();
        ActualizedHUD();

        if (_playerState == PlayerState.Dead)
        {
            return;
        }
        else
        {
            _playerSprite.color = Vector4.Lerp(_playerSprite.color, Color.white, Time.deltaTime * 3.0f);

            HandlePlayerAttack();
            HandleInteraction();

            if (canRotate)
            {
                PlayerBaseAim();
            }

            _attackTimer += Time.deltaTime;

            _weaponBone.localPosition = Vector3.Lerp(_weaponBone.localPosition, _weaponBoneTarget, Time.deltaTime * 4.0f);

            if (_playerWeaponTransform != null)
            {
                if (!isAttacking)
                {
                    _playerWeaponTransform.localPosition = Vector3.Lerp(_playerWeaponTransform.localPosition, _weaponBasePosition, Time.deltaTime * _weaponsList.WeaponsList[_playerWeaponIndex].smoothOutSpeed);
                }
                else
                {
                    if(_attackTimer > _weaponsList.WeaponsList[_playerWeaponIndex].attackTiming)
                    {
                        isAttacking = false;
                        canRotate = true;
                        weaponScript.StopAttack();
                    }



                    _playerWeaponTransform.localPosition = Vector3.Lerp(_playerWeaponTransform.localPosition, _weaponTargetPosition, Time.deltaTime * _weaponsList.WeaponsList[_playerWeaponIndex].smoothInSpeed);
                }
            }
            
        }
    }


    #region Player State Machines Bases Functions
    private void CheckChangeStateCondition(PlayerState currentState)
    {
       

        if (currentState == PlayerState.Idle)
        {
            if (GetStat(StatName.Health) <= 0)
            {
                _playerState = PlayerState.Dead;
                StartDeadBehavior();
                return;
            }

            if (Input.GetKey(_keyBindsMap.Up) || Input.GetKey(_keyBindsMap.Down) || Input.GetKey(_keyBindsMap.Left) || Input.GetKey(_keyBindsMap.Right))
            {
                _playerState = PlayerState.Moving;
                StartMovingBehavior();
                return;
            }
        }


        if(currentState == PlayerState.Moving)
        {
            if (GetStat(StatName.Health) <= 0)
            {
                _playerState = PlayerState.Dead;
                StartDeadBehavior();
                return;
            }

            if (_smoothHorizontal == 0 && _smoothVertical == 0)
            {
                _playerState = PlayerState.Idle;
                StartIdleBehavior();
                return;
            }
        }



        if(currentState == PlayerState.Dead)
        {

        }

    }

    private void HandlePlayerStateMachine()
    {
        switch(_playerState)
        {
            case PlayerState.Idle:
                IdleBehavior();
                break;


            case PlayerState.Moving:
                MovingBehavior();
                break;


            case PlayerState.Dead:
                DeadBehavior();
                break;
        }
    }

    #endregion



    #region Global Player Systems
    public void PlayerBaseAim()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - _weaponBone.position;
        direction.z = 0;
        direction.Normalize();

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        _weaponBone.rotation = Quaternion.Slerp(_weaponBone.rotation, rotation, _playerDataScriptableObject.LookSpeed * Time.deltaTime);
    }

    public void ActualizedHUD()
    {
        _lifeSlider.value = GetStat(StatName.Health);
        _staminaSlider.value = GetStat(StatName.Stamina);
    }
    
    public void PlayerDeath()
    {
        LevelManager.instance.LoadScene("GameScene");
    }


    public void HandleInteraction()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _playerDataScriptableObject.InteractionRange);

        /*
        foreach (Collider collider in colliders)
        {
            if (TryGetComponent<>())
            {

            }
        }
        */
    }


    public void SetWeapon(Items weaponEnum)
    {
        AudioManager.instance.PlayOneShot_GlobalSound(FMODEvents.instance.Player_WeaponEquiped);

        if (_playerWeaponTransform != null)
        {
            Destroy(_playerWeaponTransform.gameObject);

            Instantiate<GameObject>(_weaponsList.WeaponsList[_playerWeaponIndex].weaponDropped, transform.position + new Vector3(1.5f, 0, 0), transform.rotation);
        }


        _playerWeaponIndex = GetWeaponIndex(weaponEnum);
        _playerWeaponName = weaponEnum;


        _playerWeaponTransform = Instantiate<GameObject>(_weaponsList.WeaponsList[_playerWeaponIndex].weaponPrefab, _weaponHolder).transform;
        weaponScript = _playerWeaponTransform.GetComponent<IWeapon>();
    }

    public int GetWeaponIndex(Items weaponEnum)
    {
        for (int i = 0; i < _weaponsList.WeaponsList.Count; i++)
        {
            if (_weaponsList.WeaponsList[i].weaponName == weaponEnum)
            {
                return i;
            }
        }

        return 0;
    }
    #endregion

    #region Player Attacks Functions


    public void HandlePlayerAttack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            switch (_playerWeaponName)
            {
                case Items.Katana:
                    if (_attackTimer >= _weaponsList.WeaponsList[_playerWeaponIndex].weaponCoolDown)
                        HandleKatanaAttack();
                    break;

                case Items.Lance:
                    if (_attackTimer >= _weaponsList.WeaponsList[_playerWeaponIndex].weaponCoolDown)
                        HandleLanceAttack();
                    break;

                case Items.Pistolet:
                    if (_attackTimer >= _weaponsList.WeaponsList[_playerWeaponIndex].weaponCoolDown)
                        HandlePistolAttack();
                    break;

                case Items.Fusil:
                    if (_attackTimer >= _weaponsList.WeaponsList[_playerWeaponIndex].weaponCoolDown)
                        HandleFusilAttack();
                    break;

            }
        }

    }


    public void HandleKatanaAttack()
    {
        AudioManager.instance.PlayOneShot_GlobalSound(FMODEvents.instance.Weapons_KatanaSlash);

        isAttacking = true;
        weaponScript.StartAttack();

        _weaponTargetPosition = new Vector3(_weaponsList.WeaponsList[_playerWeaponIndex].weaponRecoil, 0, 0);
        _attackTimer = 0;
    }

    public void HandleLanceAttack()
    {
        AudioManager.instance.PlayOneShot_GlobalSound(FMODEvents.instance.Weapons_LanceSlash);

        isAttacking = true;
        canRotate = false;
        weaponScript.StartAttack();

        _weaponTargetPosition = new Vector3(_weaponsList.WeaponsList[_playerWeaponIndex].weaponRecoil, 0, 0);
        _attackTimer = 0;
    }


    public void HandlePistolAttack()
    {
        AudioManager.instance.PlayOneShot_GlobalSound(FMODEvents.instance.Weapons_PistolShot);

        isAttacking = true;
        weaponScript.StartAttack();
        _weaponTargetPosition = new Vector3(_weaponsList.WeaponsList[_playerWeaponIndex].weaponRecoil, 0, 0);
        _attackTimer = 0;
    }


    public void HandleFusilAttack()
    {
        AudioManager.instance.PlayOneShot_GlobalSound(FMODEvents.instance.Weapons_FusilShot);

        isAttacking = true;
        weaponScript.StartAttack();
        _weaponTargetPosition = new Vector3(_weaponsList.WeaponsList[_playerWeaponIndex].weaponRecoil, 0, 0);
        _attackTimer = 0;
    }


    #endregion

    #region Idle State Functions
    private void StartIdleBehavior()
    {

    }
    private void IdleBehavior()
    {
        //Debug.Log("Idle Working properly");
        IncreaseStat(StatName.Stamina, _playerDataScriptableObject.StaminaRegenerationRate * Time.deltaTime);

        CheckChangeStateCondition(_playerState);
    }
    #endregion

    #region Moving State Functions

    private void StartMovingBehavior()
    {

    }
    private void MovingBehavior()
    {
        float verticalInput = 0.0f;
        float horizontalInput = 0.0f;

        // Check for player input
        if (Input.GetKey(_keyBindsMap.Up))
        {
            verticalInput += 1.0f;
        }

        if (Input.GetKey(_keyBindsMap.Down))
        {
            verticalInput -= 1.0f;
        }


        if (Input.GetKey(_keyBindsMap.Right))
        {
            horizontalInput += 1.0f;
        }

        if (Input.GetKey(_keyBindsMap.Left))
        {
            horizontalInput -= 1.0f;
        }


        if (horizontalInput > 0)
        {
            _playerSprite.flipX = true;

            //Switch weaponSide
            Vector3 flipVector = _weaponBone.localPosition;
            flipVector.x = 0.25f;

            _weaponBoneTarget = flipVector;
        }
        else if (horizontalInput < 0)
        {
            _playerSprite.flipX = false;

            //Switch weaponSide
            Vector3 flipVector = _weaponBone.localPosition;
            flipVector.x = -0.25f;

            _weaponBoneTarget = flipVector;
        }

        _smoothVertical = Mathf.MoveTowards(_smoothVertical, verticalInput, Time.deltaTime * _playerDataScriptableObject.InputsSmoothing);

        _smoothHorizontal = Mathf.MoveTowards(_smoothHorizontal, horizontalInput, Time.deltaTime * _playerDataScriptableObject.InputsSmoothing);



        // Sprinting
        if (Input.GetKey(_keyBindsMap.Run) && GetStat(StatName.Stamina) > 0)
        {
            SetStat(StatName.CurrentSpeed, GetStat(StatName.RunSpeed));
            DecreaseStat(StatName.Stamina, _playerDataScriptableObject.StaminaConsumptionRate * Time.deltaTime);
        }
        else
        {
            SetStat(StatName.CurrentSpeed, GetStat(StatName.WalkSpeed));
            IncreaseStat(StatName.Stamina, _playerDataScriptableObject.StaminaRegenerationRate * Time.deltaTime);
        }





        // Calculate movement vector
        Vector2 movement = new Vector2(_smoothHorizontal, _smoothVertical) * GetStat(StatName.CurrentSpeed);

        // Apply movement to the Rigidbody2D
        _rigidbody.velocity = movement;


        CheckChangeStateCondition(_playerState);
    }
    #endregion

    #region Dead State Functions

    private void StartDeadBehavior()
    {

    }
    private void DeadBehavior()
    {


        CheckChangeStateCondition(_playerState);
    }
    #endregion


  
    #region IStatistics Functions

    public void InitializeStats()
    {
        foreach (Statistics statistics in _playerDataScriptableObject.PlayerStatistics)
        {
            _playerStatistics.Add(new Statistics(statistics._statName, statistics._statCurrentValue, statistics._statMaxValue));
        }
    }


    public void SetStat(StatName statName, float statValue)
    {

        foreach (Statistics stats in _playerStatistics)
        {
            if (stats._statName == statName)
            {
                stats._statCurrentValue = statValue;
            }
        }

    }

    public void DecreaseStat(StatName statName, float decreasingValue)
    {

        foreach (Statistics stats in _playerStatistics)
        {
            if (stats._statName == statName)
            {
                if (stats._statName == StatName.Health)
                {
                    AudioManager.instance.PlayOneShot_GlobalSound(FMODEvents.instance.Player_Hurt);
                    _playerSprite.color = Color.red;
                }

                stats._statCurrentValue -= decreasingValue;
                stats._statCurrentValue = Mathf.Clamp(stats._statCurrentValue, 0, stats._statMaxValue);
                return;
            }
        }

    }

    public void IncreaseStat(StatName statName, float increasingValue)
    {

        foreach (Statistics stats in _playerStatistics)
        {
            if (stats._statName == statName)
            {
                if (stats._statName == StatName.Health)
                {
                    AudioManager.instance.PlayOneShot_GlobalSound(FMODEvents.instance.Player_Healed);
                    _playerSprite.color = Color.green;
                }

                stats._statCurrentValue += increasingValue;
                stats._statCurrentValue = Mathf.Clamp(stats._statCurrentValue, 0, stats._statMaxValue);
                return;
            }
        }

    }



    public void ResetStat(StatName statName)
    {
        foreach (Statistics stat in _playerStatistics)
        {
            if (stat._statName == statName)
            {
                stat._statCurrentValue = stat._statMaxValue;
                return;
            }
        }
    }

    public float GetStat(StatName statName)
    {
        foreach (Statistics stats in _playerStatistics)
        {
            if (stats._statName == statName)
            {
                return stats._statCurrentValue;
            }
        }
        
        return 0.0f;
    }


    public float GetMaxStat(StatName statName)
    {
        foreach (Statistics stats in _playerStatistics)
        {
            if (stats._statName == statName)
            {
                return stats._statMaxValue;
            }
        }

        return 0.0f;
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _playerDataScriptableObject.InteractionRange);
    }
}

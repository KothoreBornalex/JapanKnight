using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static IObjects;
using static IStatistics;

public class PlayerStateMachine : MonoBehaviour, IStatistics
{
    public static PlayerStateMachine instance;

    #region Declaring Inspector Buttons
    [Button("Reset")] void LaunchReset() => Reset();

    #endregion

    #region Declaring Player State Fields
    public enum PlayerState
    {
        Idle,
        Moving,
        Dead
    }
    [SerializeField] private PlayerState _playerState;
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

    [ProgressBar("Current Armor", "_maxArmor", EColor.Blue)]
    [SerializeField] float _armor;
    float _maxArmor;
    #endregion
    [Space(25)]

    #region References Fields
    private Rigidbody2D _rigidbody;
    private CapsuleCollider2D _capsuleCollider;
    private Animator _animator;
    #endregion

    #region Movements Fields

    private float _smoothVertical;
    private float _smoothHorizontal;

    #endregion


    [SerializeField, Expandable] PlayerDataScriptableObject _playerDataScriptableObject;
    [SerializeField, Expandable] PlayerKeyBindsScriptableObject _keyBindsMap;

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

                case StatName.Armor:
                    _armor = statistics._statCurrentValue;
                    _maxArmor = statistics._statMaxValue;
                    break;
            }

        }
    }

    private void Reset()
    {
        Debug.Log("Player Has Been Reset");
        _playerStatistics = _playerDataScriptableObject.PlayerStatistics;
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
        Cursor.lockState = CursorLockMode.Locked;

        _rigidbody = GetComponent<Rigidbody2D>();
        _capsuleCollider = GetComponent<CapsuleCollider2D>();

        Debug.Log("Player Has Been Reset");

        InitializeStats();

    }


    private void Update()
    {
        HandlePlayerStateMachine();
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




    #region Idle State Functions
    private void StartIdleBehavior()
    {

    }
    private void IdleBehavior()
    {
        Debug.Log("Idle Working properly");

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

        _smoothVertical = Mathf.MoveTowards(_smoothVertical, verticalInput, Time.deltaTime * _playerDataScriptableObject.InputsSmoothing);

        _smoothHorizontal = Mathf.MoveTowards(_smoothHorizontal, horizontalInput, Time.deltaTime * _playerDataScriptableObject.InputsSmoothing);



        // Sprinting
        if (Input.GetKey(_keyBindsMap.Run))
        {
            SetStat(StatName.CurrentSpeed, GetStat(StatName.RunSpeed));
        }
        else
        {
            SetStat(StatName.CurrentSpeed, GetStat(StatName.WalkSpeed));
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
                stats._statCurrentValue -= decreasingValue;
                stats._statCurrentValue = Mathf.Clamp(stats._statCurrentValue, 0, stats._statMaxValue);

            }
        }

    }

    public void IncreaseStat(StatName statName, float increasingValue)
    {

        foreach (Statistics stats in _playerStatistics)
        {
            if (stats._statName == statName)
            {
                stats._statCurrentValue += increasingValue;
                stats._statCurrentValue = Mathf.Clamp(stats._statCurrentValue, 0, stats._statMaxValue);

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

    #endregion
}

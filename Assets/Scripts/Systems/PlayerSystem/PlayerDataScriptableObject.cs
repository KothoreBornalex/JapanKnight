using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static IStatistics;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 1)]
public class PlayerDataScriptableObject : ScriptableObject
{
    [SerializeField] private List<Statistics> _playerStatistics = new List<Statistics>();


    [Space(25)]
    [Header("Player Parameters")]
    [SerializeField, Range(0, 20)] float _inputsSmoothing = 2.5f;
    [SerializeField, Range(0.5f, 10)] float _interactionRange = 1.0f;

    [SerializeField, Range(0, 20)] float _jumpSpeed = 6.0f;
    [SerializeField, Range(0.5f, 10)] float _lookSpeed = 2.0f;


    [Space(20)]
    [Header("Advance")]
    [SerializeField] float _runningFOV = 65.0f;
    [SerializeField] float _speedToFOV = 4.0f;
    [SerializeField] float _timeToRunning = 2.0f;


    #region Getters & Setters
    public float JumpSpeed { get => _jumpSpeed; }
    public float LookSpeed { get => _lookSpeed; }
    public float RunningFOV { get => _runningFOV; }
    public float SpeedToFOV { get => _speedToFOV; }
    public float TimeToRunning { get => _timeToRunning; }

    public List<Statistics> PlayerStatistics { get => _playerStatistics; }
    public float InputsSmoothing { get => _inputsSmoothing;}
    public float InteractionRange { get => _interactionRange;}

    #endregion

}

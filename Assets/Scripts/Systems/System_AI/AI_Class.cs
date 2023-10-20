using NaughtyAttributes;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DroppedItem;
using static IStatistics;
using static UnityEditor.Progress;
using static WeaponContactTrigger;

public class AI_Class : MonoBehaviour, IStatistics
{
    public enum EnemyType
    {
        InfantryUnit,
        RangedUnit,
        LongRangedUnit
    }
    [Button("Receive Damage")] void Attack() => LoseLP();
    private void LoseLP()
    {
        DecreaseStat(StatName.Health, (int)UnityEngine.Random.Range(1, 3));
    }

    [Header("Global AI Fields")]
    [SerializeField] private Items _AI_Weapon;
    [SerializeField] private SpriteRenderer _aiSprite;
    [SerializeField, Expandable] private AI_Data _ai_Data;
    [SerializeField] private bool _isAlerted;
    [SerializeField] private bool _setChase;

    [SerializeField] private WeaponsScriptableObject _weaponsList;
    private List<Statistics> _aiStatistics = new List<Statistics>();

    [Header("Pathfinding Fields")]
    private AIDestinationSetter _destination;
    private Seeker _seeker;

    [Header("Patrols Fields")]
    [SerializeField] private Transform[] patrolWayPoints;
    private int currentPatrolPoint;


    [Header("Attack Fields")]
    private int _currentWeaponIndex;
    [SerializeField, Range(0, 5)] private float _attackFrequency;
    private float _attackTimer;

    private void Start()
    {
        _seeker = GetComponent<Seeker>();
        _destination = GetComponent<AIDestinationSetter>();
        _currentWeaponIndex = GetWeaponIndex(_AI_Weapon);

        InitializeStats();

    }


    private void Update()
    {
        _aiSprite.color = Vector4.Lerp(_aiSprite.color, Color.white, Time.deltaTime * 3.0f);

        if (!_isAlerted)
        {
            HandlePatrol();
        }
        else
        {
            HandleChase();
        }
    }


    #region Patrol & Chases Functions
    public void HandleChase()
    {
        if (!_setChase)
        {
            _destination.target = PlayerStateMachine.instance.transform;
            _setChase = true;
        }

        if (Vector3.Distance(transform.position, PlayerStateMachine.instance.transform.position) > _ai_Data.AttackRange && _destination.target != PlayerStateMachine.instance.transform)
        {
            _destination.target = PlayerStateMachine.instance.transform;
        }
        else
        {
            _destination.target = null;

            _attackTimer += Time.deltaTime;
            HandleAIAttack();
        }
    }

    public void HandlePatrol()
    {
        if(patrolWayPoints.Length == 0)
        {
            return;
        }

        if (_destination.target == null)
        {
            _destination.target = patrolWayPoints[currentPatrolPoint].transform;
        }


        if (Mathf.Round(transform.position.x) == Mathf.Round(patrolWayPoints[currentPatrolPoint].position.x) && Mathf.Round(transform.position.y) == Mathf.Round(patrolWayPoints[currentPatrolPoint].position.y) && currentPatrolPoint != patrolWayPoints.Length)
        {
            currentPatrolPoint++;
            _destination.target = patrolWayPoints[currentPatrolPoint].transform;

        }
        else if(patrolWayPoints.Length == currentPatrolPoint)
        {
            currentPatrolPoint = 0;
            _destination.target = patrolWayPoints[currentPatrolPoint].transform;
        }
    }

    #endregion



    public void Death()
    {
        Instantiate<GameObject>(_ai_Data.DeathObject, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }



    #region Global Attacks Functions
    public void HandleAIAttack()
    {

        switch (_AI_Weapon)
        {
            case Items.Katana:
                if (_attackTimer >= _weaponsList.WeaponsList[_currentWeaponIndex].weaponCoolDown)
                    HandleKatanaAttack();
                break;


            case Items.Pistolet:
                if (_attackTimer >= _weaponsList.WeaponsList[_currentWeaponIndex].weaponCoolDown)
                    HandlePistolAttack();
                break;

            case Items.Fusil:
                if (_attackTimer >= _weaponsList.WeaponsList[_currentWeaponIndex].weaponCoolDown)
                    Debug.Log("Fusil Attack !!");
                //HandleFusilAttack();
                break;

        }

    }


    public void HandleKatanaAttack()
    {
        Debug.Log("Katana Attack !!");
        AudioManager.instance.PlayOneShot_GlobalSound(FMODEvents.instance.Weapons_KatanaSlash);
        PlayerStateMachine.instance.DecreaseStat(StatName.Health, _weaponsList.WeaponsList[_currentWeaponIndex].weaonDamage);
        _attackTimer = 0;
    }


    public void HandlePistolAttack()
    {
        int currentWeapon = GetWeaponIndex(_AI_Weapon);

        // Calculate the rotation of the pistol in degrees
        float pistolRotation = transform.eulerAngles.z;
        float pistolRotationRad = pistolRotation * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(Mathf.Cos(pistolRotationRad), Mathf.Sin(pistolRotationRad));

        //Instantiation Projectile
        WeaponContactTrigger projectile = Instantiate<GameObject>(PlayerStateMachine.instance.WeaponsList.WeaponsList[currentWeapon].attackProjectile, transform.position, Quaternion.identity).GetComponent<WeaponContactTrigger>();
        projectile.direction = direction;
        projectile.TargetedFaction = Factions.Player;
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



    #region IStatistics Functions

    public void InitializeStats()
    {
        foreach (Statistics statistics in _ai_Data.AiStatistics)
        {
            _aiStatistics.Add(new Statistics(statistics._statName, statistics._statCurrentValue, statistics._statMaxValue));
        }
    }


    public void SetStat(StatName statName, float statValue)
    {

        foreach (Statistics stats in _aiStatistics)
        {
            if (stats._statName == statName)
            {
                stats._statCurrentValue = statValue;
            }
        }

    }

    public void DecreaseStat(StatName statName, float decreasingValue)
    {

        foreach (Statistics stats in _aiStatistics)
        {
            if (stats._statName == statName)
            {
                if (stats._statName == StatName.Health)
                {
                    AudioManager.instance.PlayOneShot_GlobalSound(FMODEvents.instance.Player_Hurt);
                    _aiSprite.color = Color.red;


                    if(stats._statCurrentValue <= 0)
                    {
                        Death();
                    }
                }

                stats._statCurrentValue -= decreasingValue;
                stats._statCurrentValue = Mathf.Clamp(stats._statCurrentValue, 0, stats._statMaxValue);
                return;
            }
        }

    }

    public void IncreaseStat(StatName statName, float increasingValue)
    {

        foreach (Statistics stats in _aiStatistics)
        {
            if (stats._statName == statName)
            {
                if (stats._statName == StatName.Health)
                {
                    AudioManager.instance.PlayOneShot_GlobalSound(FMODEvents.instance.Player_Healed);
                    _aiSprite.color = Color.green;
                }

                stats._statCurrentValue += increasingValue;
                stats._statCurrentValue = Mathf.Clamp(stats._statCurrentValue, 0, stats._statMaxValue);
                return;
            }
        }

    }



    public void ResetStat(StatName statName)
    {
        foreach (Statistics stat in _aiStatistics)
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
        foreach (Statistics stats in _aiStatistics)
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
        foreach (Statistics stats in _aiStatistics)
        {
            if (stats._statName == statName)
            {
                return stats._statMaxValue;
            }
        }

        return 0.0f;
    }
    #endregion
}

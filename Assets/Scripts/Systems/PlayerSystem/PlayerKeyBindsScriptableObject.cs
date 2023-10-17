using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "PlayerKeyBinds", menuName = "ScriptableObjects/PlayerKeyBinds", order = 1)]
public class PlayerKeyBindsScriptableObject : ScriptableObject
{
    [Header("Movements Keys")]
  [SerializeField] private KeyCode _up;
  [SerializeField] private KeyCode _down;
  [SerializeField] private KeyCode _left;
  [SerializeField] private KeyCode _right;

  [SerializeField] private KeyCode _run;


    [Header("Actions Keys")]
  [SerializeField] private KeyCode _attack;
  [SerializeField] private KeyCode _interact;

    public KeyCode Up { get => _up;}
    public KeyCode Down { get => _down;}
    public KeyCode Left { get => _left;}
    public KeyCode Right { get => _right;}
    public KeyCode Run { get => _run; }

    public KeyCode Attack { get => _attack;}
    public KeyCode Interact { get => _interact;}
}

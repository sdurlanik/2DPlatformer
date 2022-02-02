using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    public static Ability instance; 
    private void Awake() => instance = this;


    [SerializeField] private bool _canDash = false;
    [SerializeField] private bool _wallMovement = false;
    [SerializeField] private bool _canShoot = false;
    [SerializeField] private int _extraJumps = 0;
    

    public bool CanDash => _canDash;
    public bool WallMovement => _wallMovement;
    public int ExtraJumps => _extraJumps;

    public bool CanShoot
    {
        get { return _canShoot; }
        set { _canShoot = value; }
    }
}

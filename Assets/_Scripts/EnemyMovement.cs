using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Transform[] _waypoints;
    [SerializeField] private float _speed;
    private int _currentWaypoint = 0;
    private float _rotationSpeed;
    private float _waypointRadius = 1;
    private Animator enemyAnimator;

    private void Start()
    {
        enemyAnimator = GetComponent<Animator>();
    }

    void Update()
    {

        MoveTowards();

    }

    private void MoveTowards()
    {
        // Waypointe ne kadar yaklaştığımızı algılayıp ona göre içerideki işlemleri yapar
        if (Vector2.Distance(_waypoints[_currentWaypoint].position, transform.position) < _waypointRadius)
        {
            // Karakteri döndürüp soradaki waypoint noktasına geçiş yapar
            Flip();
            _currentWaypoint++;
            
            // Eğer daha fazla waypoint kalmadıysa ilk waypointe döner
            if (_currentWaypoint >= _waypoints.Length)
            {
                _currentWaypoint = 0;


            }
        }
        
        
        // Sadece x eksenine doğru hareket etmesini istediğimiz için geçici bir vektör oluşturup onun x değerini karaktere veriyoruz

        Vector2 temp = Vector2.MoveTowards(transform.position, _waypoints[_currentWaypoint].position,
            _speed * Time.deltaTime);

        transform.position = new Vector2(temp.x, transform.position.y);
        enemyAnimator.SetBool("isWalking", true);
    }

    
    // Düşman karakteri tersine döndürür
    private void Flip()
    {
        transform.Rotate(0f, 180f, 0f);
    }
}

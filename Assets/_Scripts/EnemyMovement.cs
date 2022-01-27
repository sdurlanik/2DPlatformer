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
    
    void Update()
    {
        if (Vector2.Distance(_waypoints[_currentWaypoint].position, transform.position) < _waypointRadius)
        {
            Flip();
            _currentWaypoint++;
            if (_currentWaypoint >= _waypoints.Length)
            {
                _currentWaypoint = 0;
                

            }
        }

        Vector2 temp = Vector2.MoveTowards(transform.position, _waypoints[_currentWaypoint].position,
            _speed * Time.deltaTime);

        transform.position = new Vector2(temp.x, transform.position.y);
        
    }

    private void Flip()
    {
        transform.Rotate(0f, 180f, 0f);
    }
}

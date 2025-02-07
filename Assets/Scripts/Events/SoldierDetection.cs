using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoldierDetection : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private LayerMask _detectionMask;
    [SerializeField] private float _timeUntilDetection = 2;
    [SerializeField] private float _range = 50;

    private float _currentDangerTime;

    private void Update()
    {
        LookForPlayer();
        CheckDangerTimer();
    }

    private void LookForPlayer()
    {
        Vector3 direction = _player.position - transform.position;
        if (Physics.Raycast(transform.position, direction,out RaycastHit hit,_range,_detectionMask))
        {
            if (hit.transform.CompareTag("Player"))
            {
                _currentDangerTime += Time.deltaTime;
                return;
            }
        }

        if(_currentDangerTime > 0)
        {
            _currentDangerTime -= Time.deltaTime;
        }
    }

    private void CheckDangerTimer()
    {
        if(_currentDangerTime >= _timeUntilDetection)
        {
            GameLossManager.Instance.Lose();
            Destroy(gameObject);
        }
    }
}

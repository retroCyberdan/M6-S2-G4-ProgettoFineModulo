using System.Collections;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform[] _waypoints;
    [SerializeField] private float _speed;
    [SerializeField] private float _minDistance = .001f;
    [SerializeField] private int _waitingTime = 3;
    private int _currentWaypointIndex;
    private bool _isWaiting;

    private void Update()
    {
        if (!_isWaiting) MovePlatform();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player")) collider.transform.SetParent(gameObject.transform);
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player")) collider.transform.SetParent(null);
    }

    private void MovePlatform()
    {
        Vector3 target = _waypoints[_currentWaypointIndex].position;
        transform.position = Vector3.MoveTowards(transform.position, target, _speed * Time.deltaTime);

        if (Vector3.SqrMagnitude(target - transform.position) < _minDistance * _minDistance)
        {
            _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
            _isWaiting = true;
            StartCoroutine(WaitingTime());
        }
    }

    IEnumerator WaitingTime()
    {
        yield return new WaitForSeconds(_waitingTime);
        _isWaiting = false;
    }
}

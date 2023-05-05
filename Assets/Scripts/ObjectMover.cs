using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    [SerializeField]
    private Transform _playerTransform;
    [SerializeField]
    private float _moveSpeed = 1f;
    [SerializeField]
    private float _scaleSpeed = 0.1f;
    [SerializeField]
    private Vector3 _targetPosition;
    [SerializeField]
    private Vector3 _targetScale = Vector3.one;
    [SerializeField]
    private float _moveTimer = 0f;
    [SerializeField]
    private float _moveInterval = 30f;

    void Start()
    {
        transform.localScale = Vector3.one * Vector3.Distance(transform.position, _playerTransform.position);
    }

    private void FixedUpdate()
    {

        // Update timer
        _moveTimer += Time.fixedDeltaTime;

        // Move towards player position every 30 seconds
        if (_moveTimer >= _moveInterval)
        {
            _moveTimer = 0f;
            _targetPosition = _playerTransform.position - _playerTransform.forward * 0.1f;
        }

        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _moveSpeed * Time.fixedDeltaTime);

        // Scale down towards 1 meter size
        _targetScale = Vector3.one * Vector3.Distance(transform.position, _playerTransform.position);
        transform.localScale = Vector3.Lerp(transform.localScale, _targetScale, _scaleSpeed * Time.fixedDeltaTime);

        // Stop 10cm before player position
        if (Vector3.Distance(transform.position, _targetPosition) <= 0.1f)
        {
            transform.position = _targetPosition;
            transform.localScale = Vector3.one;
            enabled = false;
        }
    }
}

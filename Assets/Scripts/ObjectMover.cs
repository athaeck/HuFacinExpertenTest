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
    [SerializeField]
    private bool _startMovement = false;
    [SerializeField]
    private float _startDelay = 30f*10;

    void Start()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);
        float scale = distanceToPlayer/10;
        _targetScale = (Vector3.one * scale) ;
        //transform.localScale = Vector3.Lerp(transform.localScale, _targetScale, _scaleSpeed * Time.fixedDeltaTime);
        transform.localScale = _targetScale;

        StartCoroutine(StartMovement());
    }

    private void FixedUpdate()
    {
        if (!_startMovement) return;

        // Update timer
        _moveTimer += Time.fixedDeltaTime;

        // Move towards player position every 30 seconds
        if (_moveTimer >= _moveInterval)
        {
            _moveTimer = 0f;
            _targetPosition = _playerTransform.position - _playerTransform.forward * 0.01f;
        }

        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _moveSpeed * Time.fixedDeltaTime);

        // Scale down towards 1 meter size
        // _targetScale = Vector3.one * Vector3.Distance(transform.position, _playerTransform.position);
        // transform.localScale = Vector3.Lerp(transform.localScale, _targetScale, _scaleSpeed * Time.fixedDeltaTime);

        // Scale down towards 1 meter size
        float distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);
        float scale = distanceToPlayer/10;
        _targetScale = (Vector3.one * scale);
        transform.localScale = _targetScale;


        // Stop 10cm before player position
        if (Vector3.Distance(transform.position, _targetPosition) <= 0.1f)
        {
            transform.position = _targetPosition;
            transform.localScale = Vector3.one;
            enabled = false;
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StopAllCoroutines();
            _startMovement = true;
        }
    }

    IEnumerator StartMovement()
    {
        yield return new WaitForSeconds(_startDelay);
        _startMovement = true;
    }
}

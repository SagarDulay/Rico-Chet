using UnityEngine;

public class MovingPlatform : MonoBehaviour, IButtonReaction, IResettable
{
    [Header("Settings")]
    [SerializeField] private Transform targetPoint;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private bool returnAfterReaching = false;

    private Vector3 _startPosition;
    private bool _isMoving = false;
    private bool _returning = false;
    private MovingPlatform _currentPlatform;
    private Vector3 _lastPlatformPosition;

    private void Awake()
    {
        _startPosition = transform.position;
    }

    public void OnButtonActivated()
    {
        _isMoving = true;
        _returning = false;
    }

    private void Update()
    {
        if (!_isMoving) return;

        Vector3 destination = _returning ? _startPosition : targetPoint.position;

        Vector3 newPosition = Vector3.MoveTowards(
            transform.position,
            destination,
            moveSpeed * Time.deltaTime
        );

        transform.position = newPosition;

        if (transform.position == destination)
        {
            if (returnAfterReaching && !_returning)
                _returning = true;
            else
                _isMoving = false;
        }
    }

    public void ResetState()
    {
        _isMoving = false;
        _returning = false;
        transform.position = _startPosition;
    }
}
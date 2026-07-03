using UnityEngine;

public class Door : MonoBehaviour, IButtonReaction, IResettable
{
    [Header("Settings")]
    [SerializeField] private Vector3 openOffset = new Vector3(0, 3f, 0);
    [SerializeField] private float slideSpeed = 2f;

    private Vector3 _closedPosition;
    private Vector3 _openPosition;
    private bool _isOpening = false;

    private void Awake()
    {
        _closedPosition = transform.position;
        _openPosition = transform.position + openOffset;
    }

    public void OnButtonActivated()
    {
        _isOpening = true;

        AudioManager.Instance.PlayDoorOpen();
    }

    private void Update()
    {
        if (!_isOpening) return;

        transform.position = Vector3.MoveTowards(transform.position, _openPosition, slideSpeed * Time.deltaTime);

        if (transform.position == _openPosition)
            _isOpening = false;
    }

    public void ResetState()
    {
        _isOpening = false;
        transform.position = _closedPosition;
    }
}
using UnityEngine;
using UnityEngine.Events;

public class FuseTimer : MonoBehaviour, IButtonReaction, IResettable
{
    [Header("Settings")]
    [SerializeField] private float fuseLength = 3f;

    [Header("On Fuse Complete")]
    public UnityEvent onFuseComplete;

    private bool _isRunning = false;
    private float _timer = 0f;

    public void OnButtonActivated()
    {
        if (_isRunning) return;
        _isRunning = true;
        _timer = fuseLength;
    }

    private void Update()
    {
        if (!_isRunning) return;

        _timer -= Time.deltaTime;

        if (_timer <= 0f)
        {
            _isRunning = false;
            onFuseComplete.Invoke();
        }
    }

    public void ResetState()
    {
        _isRunning = false;
        _timer = 0f;
    }
}
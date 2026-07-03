using UnityEngine;
using UnityEngine.Events;

public class PuzzleButton : MonoBehaviour, IActivatable, IResettable
{
    [Header("Visuals")]
    [SerializeField] private Color pressedColor = Color.red;

    [Header("Settings")]
    [SerializeField] private bool oneShot = true;

    [Header("On Activated")]
    public UnityEvent onActivated;

    private bool _hasBeenPressed = false;
    private Renderer _renderer;
    private Color _originalColor;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        if (_renderer != null)
            _originalColor = _renderer.material.color;
    }

    public void Activate()
    {
        if (oneShot && _hasBeenPressed) return;

        _hasBeenPressed = true;

        if (_renderer != null)
            _renderer.material.color = pressedColor;

        onActivated.Invoke();

        AudioManager.Instance.PlayButtonPress();
    }

    public void ResetState()
    {
        _hasBeenPressed = false;
        if (_renderer != null)
            _renderer.material.color = _originalColor;
    }
}
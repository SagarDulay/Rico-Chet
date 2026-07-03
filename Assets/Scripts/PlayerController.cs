using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 12f;

    [Header("Gravity - Mario feel")]
    [SerializeField] private float gravityUp = -20f;
    [SerializeField] private float gravityDown = -40f;

    [Header("Polish")]
    [SerializeField] private float coyoteTimeDuration = 0.12f;
    [SerializeField] private float mouseSensitivity = 2f;

    [Header("References")]
    [SerializeField] private Transform cameraTransform;

    
    private CharacterController _cc;
    private float _verticalVelocity;
    private float _cameraPitch;
    private bool _isGrounded;
    private float _coyoteTimer;

    
    private MovingPlatform _currentPlatform;
    private Vector3 _lastPlatformPosition;

    private void Start()
    {
        _cc = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleMouseLook();
        CheckGrounded();
        HandleCoyoteTime();
        HandleJump();
        ApplyGravity();
        HandleMove();
        StickToPlatform();
    }

    
    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        _cameraPitch -= mouseY;
        _cameraPitch = Mathf.Clamp(_cameraPitch, -80f, 80f);
        cameraTransform.localRotation = Quaternion.Euler(_cameraPitch, 0f, 0f);
    }

   
    private void CheckGrounded()
    {
        float rayLength = 0.15f;
        _isGrounded = Physics.Raycast(
            transform.position,
            Vector3.down,
            _cc.height / 2f + rayLength
        );
    }

    
    private void HandleCoyoteTime()
    {
        if (_isGrounded)
            _coyoteTimer = coyoteTimeDuration;
        else
            _coyoteTimer -= Time.deltaTime;
    }

    
    private float _jumpSoundDelay = 0.01f;  
    private float _jumpSoundTimer = 0f;    

    private void HandleJump()
    {
        
        if (_jumpSoundTimer < _jumpSoundDelay)
        {
            _jumpSoundTimer += Time.deltaTime;
        }

        bool canJump = _coyoteTimer > 0f;

        if (Input.GetButtonDown("Jump") && canJump)
        {
            _verticalVelocity = jumpForce;
            _coyoteTimer = 0f;

            
            if (_jumpSoundTimer >= _jumpSoundDelay)
            {
                AudioManager.Instance.PlayJump();
            }
        }
    }

    
    private void ApplyGravity()
    {
        if (_isGrounded && _verticalVelocity < 0f)
        {
            _verticalVelocity = -2f;
            return;
        }

        float gravity = _verticalVelocity > 0f ? gravityUp : gravityDown;
        _verticalVelocity += gravity * Time.deltaTime;
    }

    
    private void HandleMove()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = transform.right * h
                     + transform.forward * v;

        move = Vector3.ClampMagnitude(move, 1f);

        Vector3 velocity = move * moveSpeed
                         + Vector3.up * _verticalVelocity;

        _cc.Move(velocity * Time.deltaTime);
    }

    
    private void StickToPlatform()
    {
        RaycastHit hit;
        bool onSomething = Physics.Raycast(
            transform.position,
            Vector3.down,
            out hit,
            _cc.height / 2f + 0.2f
        );

        if (onSomething)
        {
            MovingPlatform platform = hit.collider.GetComponent<MovingPlatform>();

            if (platform != null)
            {
                if (_currentPlatform != platform)
                {
                    
                    _currentPlatform = platform;
                    _lastPlatformPosition = platform.transform.position;
                }
                else
                {
                    
                    Vector3 delta = platform.transform.position - _lastPlatformPosition;
                    _cc.Move(delta);
                    _lastPlatformPosition = platform.transform.position;
                }
            }
            else
            {
                _currentPlatform = null;
            }
        }
        else
        {
            _currentPlatform = null;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.GetComponent<LavaDeath>() != null)
        {
            CheckpointManager.Instance.RespawnPlayer(transform);
        }
    }
}
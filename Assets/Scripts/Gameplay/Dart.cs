using UnityEngine;


public class Dart : MonoBehaviour
{
    [SerializeField] private float speed = 30f;
    [SerializeField] private float lifetime = 5f;   

    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;    
        _rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    
    public void Launch(Vector3 direction)
    {
        _rb.linearVelocity = direction.normalized * speed;
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.TryGetComponent<IActivatable>(out var target))
        {
            target.Activate();
        }

        Destroy(gameObject);    
    }
}
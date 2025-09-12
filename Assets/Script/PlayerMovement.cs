using UnityEngine;

public class PersonajeScript : MonoBehaviour
{
    [SerializeField] float _speed = 5f;
    [SerializeField] float _rotationSpeed = 10f;

    Rigidbody rb;
    Animator _animator;
    Vector3 _input;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Tomamos input en 2D (plano XZ)
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        _input = new Vector3(horizontal, 0, vertical);

        // Animaciones (magnitud del movimiento)
        _animator.SetFloat("VelX", horizontal);
        _animator.SetFloat("VelY", vertical);
    }

    void FixedUpdate()
    {
        // Movimiento
        Vector3 moveDir = _input.normalized * _speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + moveDir);

        // Rotación (hacia donde se mueve)
        if (_input != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_input, Vector3.up);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);
        }
    }
}




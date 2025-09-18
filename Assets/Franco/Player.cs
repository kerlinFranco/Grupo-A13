using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]private float force= 10f, rotationSpeed = 10f;
    

    Rigidbody rb;
    PlayerInput playerInput;
    Vector2 moveInput;
    Vector3 move;
    [SerializeField]Animator animator;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = playerInput.actions["Move"].ReadValue<Vector2>();

        if (moveInput.x != 0 || moveInput.y != 0)
        {
            animator.SetFloat("IsWalk", 1);
        } else
        {
            animator.SetFloat("IsWalk", 0);
        }
    }

    private void FixedUpdate()
    {
        move = new Vector3(moveInput.x, 0, moveInput.y) * force;
        
        rb.AddForce(move);
        if (move.sqrMagnitude > 0.01f) 
        {
            Quaternion targetRotation = Quaternion.LookRotation(-move);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }

    
  
}

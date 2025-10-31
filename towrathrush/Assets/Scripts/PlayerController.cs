using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float forwardSpeed = 10f;
    public float lateralSpeed = 8f;
    public float jumpForce = 8f;
    public float laneDistance = 3f;
    
    [Header("Ground Check")]
    public LayerMask groundLayer;
    public float groundCheckDistance = 0.2f;
    
    private Rigidbody rb;
    private int currentLane = 1;
    private const int MIN_LANE = 0;
    private const int MAX_LANE = 2;
    private bool isGrounded;
    private Vector2 moveInput;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        Debug.Log("PlayerController initialized");
    }
    
    void Update()
    {
        CheckGrounded();
        HandleLaneSwitch();
    }
    
    void FixedUpdate()
    {
        if (GameManager.Instance != null && !GameManager.Instance.isGameActive)
            return;
        
        if (PhaseManager.Instance != null && PhaseManager.Instance.currentPhase == GamePhase.Transition)
            return;
            
        MoveForward();
        MoveLateral();
    }
    
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        Debug.Log($"OnMove called - Value: {moveInput}");
    }
    
    void OnJump(InputValue value)
    {
        Debug.Log($"OnJump called - Grounded: {isGrounded}");
        
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            Debug.Log("Jump executed!");
        }
    }
    
    void HandleLaneSwitch()
    {
        if (moveInput.x > 0.5f && currentLane < MAX_LANE)
        {
            currentLane++;
            moveInput.x = 0;
            Debug.Log($"Switched to lane {currentLane}");
        }
        else if (moveInput.x < -0.5f && currentLane > MIN_LANE)
        {
            currentLane--;
            moveInput.x = 0;
            Debug.Log($"Switched to lane {currentLane}");
        }
    }
    
    void MoveForward()
    {
        float currentSpeed = forwardSpeed;
        
        if (PhaseManager.Instance != null && PhaseManager.Instance.IsReversePhase())
        {
            currentSpeed = -currentSpeed;
        }
        
        Vector3 velocity = rb.linearVelocity;
        velocity.z = currentSpeed;
        rb.linearVelocity = velocity;
    }
    
    void MoveLateral()
    {
        float targetX = (currentLane - 1) * laneDistance;
        float currentX = transform.position.x;
        float deltaX = targetX - currentX;
        
        Vector3 velocity = rb.linearVelocity;
        velocity.x = deltaX * lateralSpeed;
        rb.linearVelocity = velocity;
    }
    
    void CheckGrounded()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance + 0.5f, groundLayer);
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            HealthSystem health = GetComponent<HealthSystem>();
            if (health != null)
            {
                health.TakeDamage(1);
            }
            else if (GameManager.Instance != null)
            {
                GameManager.Instance.GameOver();
            }
        }
    }
}
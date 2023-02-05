using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharController : MonoBehaviour
{
    public static event Action<CharController, int> OnHealthChanged;
    public static event Action OnPlayerGameOver; 

    public int currentHealth = 3;
    public int maxHealth = 3;
    
    //This controller uses the Rigidbody system for movement
    public Rigidbody2D theRB;

    //Variables for movement and jump force
    public float moveSpeed;
    public float jumpForce;
    public int maxJumps = 1;

    private int jumps;

    private Vector2 moveInput;

    // These build a raycast system for jumping
    public LayerMask whatIsGround;
    public bool isGrounded;

    //Animator reference 
    public Animator anim;

    //For accessing the SpriteRenderers X/Y flip
    public SpriteRenderer theSR;

    [SerializeField]
    private float jumpDelay;

    [SerializeField] 
    private float jumpDistanceCheck;

    public List<Vector2> Checkpoints = new();

    private PlayerInputActions playerInput;

    private void Awake()
    {
        playerInput = new PlayerInputActions();
        currentHealth = maxHealth;
        Checkpoints.Add(transform.position);
    }

    private void OnEnable()
    {
        playerInput.Player.Move.performed += MoveOnperformed;
        playerInput.Player.Move.canceled += MoveOncanceled;
        playerInput.Player.Jump.performed += JumpOnperformed;
        playerInput.Enable();
    }

    public void DecreaseHealth(int health)
    {
        currentHealth -= health;
        OnHealthChanged?.Invoke(this, -health);
        if (currentHealth <= 0)
        {
            OnPlayerGameOver?.Invoke();
        }
    }
    public void IncreaseHealth(int health)
    {
        currentHealth += health;
        OnHealthChanged?.Invoke(this, health);
    }

    private void JumpOnperformed(InputAction.CallbackContext input)
    {
        if (!isGrounded) { return;}
        if (jumps <= 0) { return; }
        if (jumps > 0)
        {     
            isGrounded = false;
            theRB.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            jumps--;
            anim.SetTrigger("Jump");
        }
        

    }

    private void MoveOncanceled(InputAction.CallbackContext input)
    {
        moveInput = Vector3.zero;
        anim.SetBool("isMoving", false);
    }

    private void MoveOnperformed(InputAction.CallbackContext input)
    {
        moveInput = input.ReadValue<Vector2>();
        anim.SetBool("isMoving", true);
    }

    private void OnDisable()
    {
        playerInput.Player.Move.performed -= MoveOnperformed;
        playerInput.Player.Move.canceled -= MoveOncanceled;
        playerInput.Player.Jump.performed -= JumpOnperformed;
        playerInput.Disable();
    }
    

    private void FixedUpdate()
    {
        theRB.velocity = new Vector2(moveInput.x * moveSpeed, theRB.velocity.y);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, jumpDistanceCheck, whatIsGround);

        if (hit.collider != null)
        {
            jumpDelay = hit.distance;
            isGrounded = true;
            jumps = maxJumps;
            
        }
        else
        {
            isGrounded = false;
            jumpDelay = 0f;
        }
        
        theSR.flipX = theSR.flipX switch
        {
            //This flips the sprite when moving
            false when moveInput.x < 0 => true,
            true when moveInput.x > 0 => false,
            _ => theSR.flipX
        };
    }

    public void Respawn()
    {
        transform.position = Checkpoints[Checkpoints.Count - 1];
    }
}
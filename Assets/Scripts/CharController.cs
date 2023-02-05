using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class CharController : MonoBehaviour
{
    //This controller uses the Rigidbody system for movement
    public Rigidbody theRB;

    //Variables for movement and jump force

    public float moveSpeed;
    public float jumpForce;
    public bool isCrouched;
    public int maxJumps = 1;

    private int jumps;

    private Vector3 moveInput;

    // These build a raycast system for jumping
    public LayerMask whatIsGround;
    public Transform groundPoint;
    public bool isGrounded;

    //Animator refrence 
    public Animator anim;

    //For accessing the SpriteRenderers X/Y flip
    public SpriteRenderer theSR;


    //public HealthBar healthBar;

    public float maxHealth = 100;
    [Range(0, 100)]
    public float currentHealth = 100;

    [SerializeField]
    private float jumpDelay;

    [SerializeField]
    private float justSwitchedFrames = 0;

    void OnTriggerEnter(Collider other)
    {
        if (other)
        {
            // Debug.Log("HIT");
        }
        if (other.gameObject.tag == "MossSlope")
        {
            Debug.Log("You got hit");
            currentHealth -= 200 * Time.deltaTime;
         //   healthBar.SetHealth(currentHealth);

        }
    }

    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
        {
            if(this.jumpDelay > .03f)
                this.Jump();
        }


        //Gets the inputs for movement
        moveInput.x = Input.GetAxis("Horizontal");
        //moveInput.y = Input.GetAxis("Vertical");

        //Stops any wierd directional movement speed increases
        moveInput.Normalize();

        // Applies the inputs to the Rigidbody 
        theRB.velocity = new Vector3(moveInput.x * moveSpeed, theRB.velocity.y, moveInput.y * moveSpeed);

        //Stores information if the raycast hits anything and detects the gorund when jumping
        RaycastHit hit;
        if (Physics.Raycast(groundPoint.position, Vector3.down, out hit, .7f, whatIsGround))
        {
            Debug.DrawRay(groundPoint.position, Vector3.down * hit.distance, Color.green);

            jumpDelay = hit.distance;
            //Debug.Log(hit.distance);
            isGrounded = true;
            jumps = maxJumps;
        }
        else
        {
            Debug.DrawRay(groundPoint.position, Vector3.down * .7f, Color.red);
            isGrounded = false;
            jumpDelay = 0f;
        }

        //This flips the sprite when moving
        if (!theSR.flipX && moveInput.x < 0)
        {
            theSR.flipX = true;
        }
        else if (theSR.flipX && moveInput.x > 0)
        {
            theSR.flipX = false;
        }
        anim.SetBool("onGround", isGrounded);
        if(Input.GetAxis("Horizontal") < 0)
        {
            anim.SetFloat("MoveSpeed", -Input.GetAxis("Horizontal"));
            justSwitchedFrames = -Input.GetAxis("Horizontal");
        }
        else
        {
            anim.SetFloat("MoveSpeed", Input.GetAxis("Horizontal"));
            justSwitchedFrames = Input.GetAxis("Horizontal");
        }
        //if(Input.GetAxis("Horizontal") == 0 && justSwitchedFrames != 0)
        //{
        //    anim.SetFloat("MoveSpeed", justSwitchedFrames);
        //}
        //    anim.SetFloat("MoveSpeed", 3);

    }
    
    private void Jump()
       {
        Debug.Log("Jumping");
        if (jumps > 0)
            {     
                isGrounded = false;
                gameObject.GetComponent<Rigidbody>().AddForce (new Vector3 (0, jumpForce), ForceMode.Impulse);
                jumps = jumps - 1;
            }
            if (jumps == 0)
            {
                return;
            }
        }

}
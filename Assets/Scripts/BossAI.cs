using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    [Tooltip("Where the rays will be drawn from")]
    public Transform eyes;

    [Tooltip("If enabled enemy will pace back and forth")]
    public bool pace = false;

    [Tooltip("Speed is randomly chosen between two points")]
    public List<int> speedRanges = new List<int>() { 1, 10 };

    [Tooltip("How likely you are to move")]
    public int howLikelyToMove = 10;
    public float howLongToMove = 4f;

    public bool allowJumping = false;

    private int direction = -1;

    private int decisiveness = 1;
    [SerializeField]
    private float howLongIDecided = 2f;
    [SerializeField]
    private bool moving = true;

    [SerializeField]
    private float buildingSpeed = 0f;
    [SerializeField]
    private float currentSpeed = 10f;

    private bool grounded = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision other)
    {
        grounded = true;
    }
    void OnCollisionExit(Collision collisionInfo)
    {
        grounded = false;
    }

    // See Order of Execution for Event Functions for information on FixedUpdate() and Update() related to physics queries
    void FixedUpdate()
    {

        Vector2 fwd = transform.TransformDirection(Vector2.left);
        RaycastHit2D hit = Physics2D.Raycast(eyes.position, fwd, 10);

        if (hit != false)
        {
            Debug.DrawRay(eyes.position, fwd * hit.distance, Color.red);
            //Debug.Log("I see " + hit.transform.gameObject.name);
            Debug.Log(hit.transform.gameObject.layer);
        }
        else
        {
            Debug.DrawRay(eyes.position, fwd * 10, Color.green);
        }
        howLongIDecided -= Time.deltaTime;
        if (howLongIDecided < 0)
        {
            int decision = (int)Random.Range(0, howLikelyToMove);
            Debug.Log("I decided this..." + decision);
            if (decision == decisiveness)
            {
                Debug.Log("Moving");
                moving = true;
                currentSpeed = Random.Range(speedRanges[0], speedRanges[1]);
            }
            else
            {
                Debug.Log("Not Moving");
                moving = false;
            }
            howLongIDecided = Random.Range(0, howLongToMove);
        }
        if (moving || pace)
        {
            Movement();
        }

        Vector3 fwdDown = transform.TransformDirection(new Vector3(-1, -.5f, 0));
        hit = Physics2D.Raycast(eyes.position + Vector3.up * 1, fwdDown, 5f);
        if (hit)
        {
            Debug.DrawRay(eyes.position + Vector3.up * 1, fwdDown * hit.distance, Color.red);
            //Debug.Log("I see " + hit.transform.gameObject.name);
            Debug.Log(hit.transform.gameObject.layer);
            if (hit.transform.gameObject.layer == 3)
            {
                if (hit.distance > 5)
                {
                    TurnAround();
                }
            }
            else
            {
            }
        }
        else
        {
            Debug.DrawRay(eyes.position + Vector3.up * 1, fwdDown * 3f, Color.green);
            TurnAround();
            
        }
    }

    void Movement()
    {
        if (grounded)
            this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        RaycastHit2D hit;
        Vector3 fwdDown = transform.TransformDirection(new Vector3(-1, -.4f, 0));

        hit = Physics2D.Raycast(eyes.position, fwdDown, 1.5f);
        if (hit)
        {
            Debug.DrawRay(eyes.position, fwdDown * hit.distance, Color.red);
            //Debug.Log("I see " + hit.transform.gameObject.name);
            Debug.Log(hit.transform.gameObject.layer);
            if (hit.transform.gameObject.layer == 3)
            {
                if (allowJumping)
                {
                    if (buildingSpeed < currentSpeed) this.buildingSpeed += this.currentSpeed / 10;
                    this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(-buildingSpeed * direction, 10, 0);
                }
                else
                {
                    if (buildingSpeed < currentSpeed) this.buildingSpeed += this.currentSpeed / 10;
                    this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(-buildingSpeed * direction, 0, 0);
                }
            }
            else
            {
                if (grounded)
                    this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                buildingSpeed = 0f;
            }
        }
        else
        {
            if (grounded)
                this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            Debug.DrawRay(transform.position, fwdDown * 1.5f, Color.green);
            TurnAround();
        }



    }


    private void TurnAround()
    {
        if (direction < 0) this.gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
        if (direction > 0) this.gameObject.transform.rotation = new Quaternion(0, 180, 0, 0);
        direction = -direction;
        buildingSpeed = 0f;
    }
}

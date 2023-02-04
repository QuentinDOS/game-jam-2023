using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    public Transform eyes;
    public bool pace = false;

    public int howLikelyToMove = 10;

    private int direction = -1;

    private int decisiveness = 2;
    private float howLongIDecided = 2f;
    private bool moving = true;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {    

    }

    // See Order of Execution for Event Functions for information on FixedUpdate() and Update() related to physics queries
    void FixedUpdate()
    {

        Vector3 fwd = transform.TransformDirection(Vector3.left);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, fwd, out hit, 10))
        {
            Debug.DrawRay(transform.position, fwd * hit.distance, Color.red);
            //Debug.Log("I see " + hit.transform.gameObject.name);
            Debug.Log(hit.transform.gameObject.layer);
        }
        else
        {
            Debug.DrawRay(transform.position, fwd * 10, Color.green);
        }
        howLongIDecided -= Time.deltaTime;
        if(howLongIDecided < 0)
        {
            int decision = (int)Random.Range(0, 5);
            Debug.Log("I decided this..." + decision);
            if(decision == decisiveness)
            {
                Debug.Log("Moving");
                moving = true;
            }
            else
            {
                Debug.Log("Not Moving");
                moving = false;
            }
            howLongIDecided = Random.Range(0, 2);
        }
        if(moving || pace)
        {
            Movement();
        }
    }

    void Movement()
    {
        RaycastHit hit;
        Vector3 fwdDown = transform.TransformDirection(new Vector3(-1, -.4f, 0));
        if (Physics.Raycast(transform.position, fwdDown, out hit, 1.5f))
        {
            Debug.DrawRay(transform.position, fwdDown * hit.distance, Color.red);
            //Debug.Log("I see " + hit.transform.gameObject.name);
            Debug.Log(hit.transform.gameObject.layer);
            if(hit.transform.gameObject.layer == 3)
            {
                this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(-10 * direction, 0, 0);
            }
            else
            {
                this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            }
        }
        else
        {
            this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            Debug.DrawRay(transform.position, fwdDown * 1.5f, Color.green);
            if(direction < 0) this.gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
            if(direction > 0) this.gameObject.transform.rotation = new Quaternion(0, 180 , 0, 0);
            direction = -direction;
        }
    }
}

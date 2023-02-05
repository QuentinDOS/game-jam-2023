using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyAI : MonoBehaviour
{
    private float timeRunning = 0;
    public LayerMask whatIsGround;
    public float viewDistance = 1f;

    public float speed = 2f;
    public float playerChaseSpeed = 2.5f;

    Rigidbody2D rb;
    private int direction = 1;
    // Start is called before the first frame update
    void Start()
    {
        if(!rb)
        {
            rb = this.GetComponent<Rigidbody2D>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void FixedUpdate()
    {

        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, Vector2.right * direction * viewDistance, 1f, whatIsGround);

        

        if (hit != false)
        {
            Debug.DrawRay(this.transform.position, Vector2.right * direction * viewDistance, Color.green);
            //Debug.Log("Turning around");
            direction = -direction;
            //if(direction > 0)
            //    this.transform.rotation = new Quaternion(0, 180, 0, 0);
            //else
            //    this.transform.rotation = new Quaternion(0, 0, 0, 0);
        }
        else
        {
            Debug.DrawRay(this.transform.position, Vector2.right * direction, Color.green);
        }

       
        rb.velocity = new Vector2(speed * direction * Time.deltaTime, 0);
    }
}

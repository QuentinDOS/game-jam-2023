using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BasicEnemyAI : MonoBehaviour
{
    public int damage = 1;
    private float timeRunning = 0;
    public LayerMask whatIsGround;
    public float viewDistance = 1f;
    public float collisionDistance = 2f;

    public float speed = 2f;
    public float playerChaseSpeed = 2.5f;

    Rigidbody2D rb;
    private int direction = 1;

    private Animator anim;
    
    // Start is called before the first frame update
    void Start()
    {
        if(!rb)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        if (!anim)
        {
            anim = GetComponent<Animator>();
        }
    }
    private RaycastHit2D CheckForGround(int direction)
    {
        Vector2 startPoint = transform.position;
        Vector2 endPoint = startPoint + new Vector2(viewDistance * direction, 0f);
        return Physics2D.Linecast(startPoint, endPoint, whatIsGround);
    }

    private void MoveToGround(RaycastHit2D rayHit)
    {
        float distance = Vector2.Distance(transform.position, rayHit.point);
        if (distance >= collisionDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, rayHit.point, speed * Time.deltaTime);
        }
        else
        {
            direction *= -1;
        }
    }

    private void Update()
    {
        RaycastHit2D hit = CheckForGround(direction);
        if (hit.collider != null)
        {
            MoveToGround(hit);
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving",false);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        var player = col.gameObject.GetComponent<CharController>();
        if (player != null)
        {
            player.DecreaseHealth(damage);
            player.Respawn();
        }
    }
}

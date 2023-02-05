using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineGrab : MonoBehaviour
{
    [SerializeField]
    GameObject otherObject;

    private float holdingTime = 0f;

    public float amountOfVel = .2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(this.gameObject.transform.rotation.z > .1) 
        {
            Debug.Log("right");
            if(this.otherObject.GetComponent<Rigidbody>().velocity.x > 0 && holdingTime < .1f)
            {
                this.gameObject.GetComponent<Rigidbody>().velocity += this.otherObject.GetComponent<Rigidbody>().velocity * amountOfVel;
                holdingTime += Time.deltaTime;
            }
            else if(!(holdingTime < .1f))
            {
                holdingTime -= Time.deltaTime;
            }
        
        }
        else if(this.gameObject.transform.rotation.z < -.1)
        {
            Debug.Log("left");
            if(this.otherObject.GetComponent<Rigidbody>().velocity.x < 0 && holdingTime > -.1f)
            {
                this.gameObject.GetComponent<Rigidbody>().velocity += this.otherObject.GetComponent<Rigidbody>().velocity * amountOfVel;
                holdingTime -= Time.deltaTime;
            }
            else if(!(holdingTime > -.1f))
            {
                holdingTime += Time.deltaTime;
            }
        
        }
        else
        {
            this.gameObject.GetComponent<Rigidbody>().velocity += this.otherObject.GetComponent<Rigidbody>().velocity * amountOfVel;
        }

        this.otherObject.GetComponent<Rigidbody>().velocity = new Vector3();
        this.otherObject.transform.position = this.gameObject.transform.position;

        
        

        if(Input.GetButtonDown("up"))
        {
            if(otherObject)
            {
                otherObject.gameObject.transform.SetParent(this.transform);
            }
        }
        if(this.gameObject.GetComponentInChildren<Transform>())
        {
            if(Input.GetButtonUp("up"))
            {
                this.gameObject.transform.DetachChildren();
            }

        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(!other.gameObject.transform.parent)
            {
                Debug.Log("Collider entered");
                Debug.Log(other.gameObject.name);
                otherObject = other.gameObject;
                otherObject.gameObject.transform.SetParent(this.transform);

                this.otherObject.GetComponent<Rigidbody>().useGravity = false;
            }
        }
    }
}

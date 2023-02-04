using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {    
        for (float i = 0; i < 360; i++)
        {
            var currentPointPosition = Quaternion.AngleAxis(i, transform.up) * transform.forward;
            Debug.DrawRay(this.gameObject.transform.position, currentPointPosition, Color.red, 1.0f);
        }
    }
}

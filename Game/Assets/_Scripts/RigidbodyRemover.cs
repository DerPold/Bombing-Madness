using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyRemover : MonoBehaviour {

    private Rigidbody rb;
    
    private float frameCounter;
	void Start () {
        rb = this.gameObject.GetComponent<Rigidbody>();
    }

    bool isGrounded()
    {
        return rb.velocity.magnitude < 0.5;
    }
	
	// Update is called once per frame
	void Update ()
    {
        frameCounter += Time.deltaTime;
        if(frameCounter > 5 && isGrounded())
        {
            Destroy(rb);
            Destroy(this);
        }
	}
}

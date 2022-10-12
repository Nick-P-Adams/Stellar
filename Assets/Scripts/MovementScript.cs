using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    private Vector3 movementVector;
    private Rigidbody playerBody;
    public float speed = 10.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        playerBody = gameObject.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 upVector = new Vector3(), downVector = new Vector3(), rightVector = new Vector3(), leftVector = new Vector3();

        if (Input.GetKey("w"))
        {
            upVector = new Vector3(transform.forward.x, 0.0f, transform.forward.z);
        }
        if (Input.GetKey("s"))
        {
            downVector = new Vector3(-transform.forward.x, 0.0f, -transform.forward.z);
        }
        if (Input.GetKey("d"))
        {
            rightVector = new Vector3(transform.right.x, 0.0f, transform.right.z);
        }
        if (Input.GetKey("a"))
        {
            leftVector = new Vector3(-transform.right.x, 0.0f, -transform.right.z);
        }
        movementVector = upVector + downVector + rightVector + leftVector;

        playerBody.MovePosition(transform.position + movementVector * Time.fixedDeltaTime * speed);
    }
}

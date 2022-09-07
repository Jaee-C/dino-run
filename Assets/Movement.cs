using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody rigidBody;
    private GeneratePlane planeGenerator;
    private bool isColliding = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        planeGenerator = FindObjectOfType<GeneratePlane>();
        rigidBody.velocity = new Vector3(0, 0, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        //rigidBody.MovePosition(this.transform.position + new Vector3(0, 0, 0.2f));
        //rigidBody.AddForce(new Vector3(0, 0, .2f), mode: ForceMode.VelocityChange

        bool isMoving = false;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (isColliding)
            {
                rigidBody.velocity = new Vector3(-10f, 0, 0);
            }
            else
            {
                rigidBody.velocity = new Vector3(-10f, 0, 10f);
            }
            isMoving = true;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (isColliding)
            {
                rigidBody.velocity = new Vector3(10f, 0, 0);
            }
            else
            {
                rigidBody.velocity = new Vector3(10f, 0, 10f);
            }
            isMoving = true;
        }

        if (isColliding)
        {
            if(rigidBody.velocity.z != 0 && rigidBody.velocity.z > -0.1f) isColliding = false;
        }
        else if (!isMoving)
        {
            rigidBody.velocity = new Vector3(0, 0, 10f);    
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collider collider = collision.collider;
        if (collider.name == "End")
        {
            planeGenerator.spawnPlane();
            collider.gameObject.GetComponent<BoxCollider>().enabled = false;
            Destroy(collider.transform.parent.gameObject, 1f);
        }else if (collider.name.StartsWith("Obstacle"))
        {
            rigidBody.velocity = Vector3.zero;
            rigidBody.AddForce(new Vector3(0, 0, -200f));
            isColliding = true;
        }
    }
}

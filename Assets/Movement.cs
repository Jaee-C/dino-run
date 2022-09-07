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
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position.z > planeGenerator.getLastPlane().transform.position.z + GeneratePlane.PLANE_SIZE / 2)
        {
            planeGenerator.spawnPlane();
            planeGenerator.destroy();
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if(rigidBody.velocity.x > -10)
            {
                rigidBody.AddForce(new Vector3(-10f, 0, 0));
            }
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (rigidBody.velocity.x < 10)
            {
                rigidBody.AddForce(new Vector3(10f, 0, 0));
            }
        }

        if (isColliding)
        {
            if(Mathf.Abs(rigidBody.velocity.z) < 0.3f) isColliding = false;
        }
        else
        {
            if(rigidBody.velocity.z < 10) rigidBody.AddForce(new Vector3(0, 0, 100f));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collider collider = collision.collider;
        if (collider.name.StartsWith("Obstacle"))
        {
            rigidBody.AddForce(-rigidBody.velocity * 70);
            isColliding = true;
        }
    }
}

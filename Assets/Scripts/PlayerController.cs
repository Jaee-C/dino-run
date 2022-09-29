using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 3.0f;
    [SerializeField] private float speedIncrease = 1.0f;
    [SerializeField] private float dodgeSpeed = 1.0f;
    [SerializeField] private FollowPlayer cameraMovement;

    private GeneratePlane planeGenerator;


    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        planeGenerator = FindObjectOfType<GeneratePlane>();
    }

    // Update is called once per frame
    void Update()
    {
        // Move the player
        float xMove = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector3(xMove, rb.velocity.y, speed) * dodgeSpeed;

        speed += speedIncrease * Time.deltaTime; // Increases speed over time
        
        if (this.transform.position.z - cameraMovement.zOffset > planeGenerator.getLastPlane().transform.position.z + GeneratePlane.PLANE_SIZE / 2)
        {
            planeGenerator.spawnPlane();
            planeGenerator.destroy();
        }
    }

    private void OnTriggerEnter(Collider other) {
        Destroy(other.gameObject);
    }
}

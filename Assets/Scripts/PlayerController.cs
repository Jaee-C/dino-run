using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 3.0f;
    [SerializeField] private float speedIncrease = 1.0f;
    [SerializeField] private float dodgeSpeed = 1.0f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Move the player
        float xMove = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector3(xMove, rb.velocity.y, speed) * dodgeSpeed;

        speed += speedIncrease * Time.deltaTime; // Increases speed over time
    }

    private void OnTriggerEnter(Collider other) {
        Destroy(other.gameObject);
    }
}

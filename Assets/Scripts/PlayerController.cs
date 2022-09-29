using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 3.0f;
    [SerializeField] private float speedIncrease = 1.0f;
    [SerializeField] private float dodgeSpeed = 1.0f;
    public Slider healthBar;
    public float health = 100.0f;
    public float maxHealth = 100.0f;
    public float healthDecay = 0.1f;

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

        health -= healthDecay * Time.deltaTime;
        speed += speedIncrease * Time.deltaTime; // Increases speed over time
        healthBar.value = health;
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
        health -= 20;
        healthBar.value = health;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 3.0f;
    [SerializeField] private float speedIncrease = 1.0f;
    [SerializeField] private float dodgeSpeed = 1.0f;
    [SerializeField] private float health = 100.0f;
    [SerializeField] private float maxHealth = 100.0f;
    [SerializeField] private float healthDecay = 0.1f;
    [SerializeField] private float slowdownRate = 0.8f;
    [SerializeField]
    [Range(0, 80)]
    private float slowdownThreshold = 20.0f;
    public Slider healthBar;

    private bool slowedSpeed;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        slowedSpeed = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Move the player
        float xMove = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector3(xMove, rb.velocity.y, speed) * dodgeSpeed;

        // Decelerate health decay after reaching slowdownThreshold
        if (health < slowdownThreshold && slowedSpeed == false)
        {
            slowedSpeed = true;
            speed *= slowdownRate;
        }
        // Reaccelerate health decay when above slowdownThreshold
        else if (health >= slowdownThreshold)
        {
            slowedSpeed = false;
            speed += speedIncrease * Time.deltaTime;
        }


        health -= healthDecay * Time.deltaTime;
        healthBar.value = health/maxHealth * 100;
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
        health -= 20;
        healthBar.value = health;
    }
}

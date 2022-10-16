using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 3.0f;
    [SerializeField] private float speedIncrease = 1.0f;
    [SerializeField] private float dodgeSpeed = 1.0f;
    [SerializeField] private FollowPlayer cameraMovement;
    [SerializeField] private bool enableSpeedLimit = true;
    [SerializeField] private float speedLimit = 10f;

    private GeneratePlane planeGenerator;

    [SerializeField] private float health = 100.0f;
    [SerializeField] private float maxHealth = 100.0f;
    [SerializeField] private float healthDecay = 0.1f;
    [SerializeField] private float slowdownRate = 0.8f;
    [SerializeField]
    [Range(0, 80)]
    private float slowdownThreshold = 20.0f;
    [SerializeField] private float obstacleDamage = 10.0f;
    [SerializeField] private float foodHeal = 10.0f;
    [SerializeField] private GameObject restartButton;
    public Slider healthBar;

    private bool slowedSpeed;

    private Rigidbody rb;

    [SerializeField] private Animation anim;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        planeGenerator = FindObjectOfType<GeneratePlane>();
        slowedSpeed = false;
        restartButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (health <= 0)
        {
            Time.timeScale = 0;
            restartButton.SetActive(true);
        }

        // Move the player
        float xMove = Input.GetAxisRaw("Horizontal");

        rb.velocity = new Vector3(xMove * dodgeSpeed, rb.velocity.y, speed);

        // Player can't leave game area
        if (Mathf.Abs(this.transform.position.x) > GeneratePlane.PLANE_SIZE / 2.0f)
        {
            this.transform.position = new Vector3(Mathf.Sign(this.transform.position.x) * GeneratePlane.PLANE_SIZE / 2.0f, this.transform.position.y, this.transform.position.z);
        }

        if (!enableSpeedLimit || enableSpeedLimit && speed < speedLimit)
        {
            speed += speedIncrease * Time.deltaTime;
        }

        // Plane is destroyed when player passes it
        if (this.transform.position.z - cameraMovement.zOffset > planeGenerator.getLastPlane().transform.position.z + GeneratePlane.PLANE_SIZE / 2)
        {
            planeGenerator.spawnPlane();
            planeGenerator.destroy();
        }

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
        healthBar.value = health / maxHealth * 100;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Player health change on collision
        if (other.gameObject.tag == "Obstacle")
        {
            health -= obstacleDamage;
            other.gameObject.GetComponent<ObstacleController>().Kill();
        }
        else if (other.gameObject.tag == "Food")
        {
            health += foodHeal;
        }
        Destroy(other.gameObject);
        healthBar.value = health;
    }

    public void test(string s)
    {
        Debug.Log(s);
    }
}

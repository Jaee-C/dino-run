using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public float speed = 1.0f;
    [SerializeField] private float dodgeSpeed = 1.0f;

    // Update is called once per frame
    void Update()
    {
        // Move the player forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Player movement
        if (Input.GetKey(KeyCode.LeftArrow))
            transform.Translate(Vector3.left * (this.dodgeSpeed * Time.deltaTime));
        if (Input.GetKey(KeyCode.RightArrow))
            transform.Translate(Vector3.right * (this.dodgeSpeed * Time.deltaTime));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] public Transform player;
    [SerializeField] private float yOffset = 2.0f;
    [SerializeField] private float xOffset = 0.0f;
    [SerializeField] public float zOffset = 7.0f;
    [SerializeField] public bool followLeftRight = true;

    // Shake Parameters
    [SerializeField] public float shakeDuration = 0.5f;
    [SerializeField] public float shakeAmount = 0.05f;

    private bool canShake = false;  // set to true when the camera is shaking
    private float _shakeTimer;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        float playerX = player.transform.position.x;
        float playerY = player.transform.position.y;
        float playerZ = player.transform.position.z;
        if (followLeftRight)
        {
            transform.position = new Vector3(playerX + xOffset, playerY + yOffset, playerZ - zOffset);
        }
        else
        {
            transform.position = new Vector3(xOffset, playerY + yOffset, playerZ - zOffset);
        }

        // Check if the camera is supposed to be shaking at the moment
        if (canShake)
        {
            StartCameraShakeEffect();
        }
    }

    // Camera Shake functions
    public void ShakeCamera()
    {
        canShake = true;
        _shakeTimer = shakeDuration;
    }

    /** Changes camera position to shakes the camera when the timer is not up 
    or stops the shaking when the camera is up.
    */
    public void StartCameraShakeEffect()
    {
        if (_shakeTimer > 0)
        {
            // Shakes the camera
            transform.localPosition = transform.position + Random.insideUnitSphere * shakeAmount;
            _shakeTimer -= Time.deltaTime;
        }
        else
        {
            // Stop shaking the character when the shakeTimer is up
            _shakeTimer = 0f;
            transform.position = transform.position;
            canShake = false;
        }
    }

}
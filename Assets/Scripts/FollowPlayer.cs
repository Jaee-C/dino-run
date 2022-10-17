using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] public Transform player;
    [SerializeField] public float zOffset = 7.0f;
    [SerializeField] private float yOffset = 2.0f;

    // Shake Parameters
    [SerializeField] public float shakeDuration = 0.5f;
    [SerializeField] public float shakeAmount = 0.05f;

    private bool canShake = false;
    private float _shakeTimer;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + new Vector3(0, yOffset, - zOffset);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShakeCamera();
        }

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

    public void StartCameraShakeEffect()
    {
        if (_shakeTimer > 0)
        {
            transform.localPosition = transform.position + Random.insideUnitSphere * shakeAmount;
            _shakeTimer -= Time.deltaTime;
        }
        else
        {
            _shakeTimer = 0f;
            transform.position = transform.position;
            canShake = false;
        }
    }
    
}
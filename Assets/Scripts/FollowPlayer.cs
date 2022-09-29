using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] public Transform player;
    [SerializeField] private float zOffset = 7.0f;
    [SerializeField] private float yOffset = 2.0f;

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + new Vector3(0, yOffset, -zOffset);
    }
}

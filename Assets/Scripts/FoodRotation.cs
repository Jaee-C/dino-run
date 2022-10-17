using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodRotation : MonoBehaviour
{
    [SerializeField] private float spinSpeed = 100f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var angle = this.spinSpeed * Time.deltaTime;
        var axis = new Vector3(0.0f, 1.0f, 0.0f);
        transform.localRotation *= Quaternion.AngleAxis(angle, axis);
    }
}
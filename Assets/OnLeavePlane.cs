using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnLeavePlane : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.name == "Capsule" && this.transform.parent.gameObject.name != "Plane")
        {
            GeneratePlane.spawnPlane();
            Destroy(this.transform.parent.gameObject);
        }
    }
}

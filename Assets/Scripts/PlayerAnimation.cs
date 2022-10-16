using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    ParticleManager particleGenerator;
    // Start is called before the first frame update
    void Start()
    {
        this.particleGenerator = GameObject.FindObjectOfType<ParticleManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void generateParticle()
    {
        this.particleGenerator.generateParticles(this.transform.position + new Vector3(0, 0.2f, 0),
            Vector3.up, 10, 200, 10);
    }
}

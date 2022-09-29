using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    private int frameCount = 0;
    private bool startGen = false;
    private int particleCount = 0;
    private List<ParticleLifetime> particles = new List<ParticleLifetime>();

    private int MAX_PARTICLES = 10;

    struct ParticleLifetime
    {
        public GameObject particle;
        public int createdFrame;
    }

    // Start is called before the first frame update
    void Start()
    {
        float radius = 1f;
        Vector3 N = new Vector3(0.5f, 0.5f, 0.5f);
        N = N.normalized;

        Vector3 v1 = new Vector3(1, 1, 0);
        v1.z = (-N.y - N.x) / N.z;
        v1 = v1.normalized;
        Vector3 v2 = Vector3.Cross(N, v1);
        v2 = v2.normalized;
        
        Debug.DrawLine(Vector3.zero, N * 100, Color.red, 1000f);
        Debug.DrawLine(Vector3.zero + N * 1, v1 * 100, Color.blue, 1000f);
        Debug.DrawLine(Vector3.zero + N * 1, v2 * 100, Color.blue, 1000f);
    }

    void generateParticle(Vector3 initialPosition, Vector3 direction, float radius, float force)
    {
        GameObject particle = GameObject.CreatePrimitive(PrimitiveType.Cube);
        particles.Add(new ParticleLifetime() { particle = particle, createdFrame = frameCount });
        particle.transform.position = initialPosition;
        particle.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        Vector3 dir = direction;

        if (direction.x == 0) dir.x = Random.Range(-radius, radius);
        if (direction.y == 0) dir.y = Random.Range(-radius, radius);
        if (direction.z == 0) dir.z = Random.Range(-radius, radius);

        dir = dir.normalized;

        particle.AddComponent<Rigidbody>().mass = 0.5f;
        particle.GetComponent<Rigidbody>().AddForceAtPosition(dir * force, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        frameCount++;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            startGen = true;
        }
        
        if(frameCount % 2 == 0)
        {
            if (startGen && particleCount < MAX_PARTICLES)
            {
                generateParticle(Vector3.zero, new Vector3(0, 0, 3), 1f, 300f);
                particleCount++;
            }
            else
            {
                if(particles.Count > 0)
                {
                    for(int i = 0; i < particles.Count;)
                    {
                        ParticleLifetime particleInfo = particles[i];
                        if(frameCount - particleInfo.createdFrame > 100)
                        {
                            Destroy(particleInfo.particle);
                            particles.RemoveAt(i);
                        }
                        else
                        {
                            i++;
                        }
                    }
                }
                particleCount = 0;
                startGen = false;
            }
        }
        */
    }
}

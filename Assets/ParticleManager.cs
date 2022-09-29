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
        /*
        float radius = 0.5f;
        Vector3 dir = new Vector3(-0.2f, -0.3f, 0);
        float gapAngle = Mathf.Acos((2 - radius * radius) / 2);

        dir = dir.normalized;
        float xyAngle = Mathf.Acos(Vector3.Dot(dir, new Vector3(1, 0, 0)));
        float leftXYAngle = xyAngle - gapAngle;
        float rightXYAngle = xyAngle + gapAngle;
        Vector3 leftXYdir = new Vector3(Mathf.Cos(leftXYAngle), Mathf.Sin(leftXYAngle), 0);
        Vector3 rightXYdir = new Vector3(Mathf.Cos(rightXYAngle), Mathf.Sin(rightXYAngle), 0);

        Debug.DrawLine(Vector3.zero, dir * 100, Color.red, 1000f);
        Debug.DrawLine(Vector3.zero, leftXYdir * 100, Color.blue, 1000f);
        Debug.DrawLine(Vector3.zero, rightXYdir * 100, Color.blue, 1000f);
        */
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
    }
}

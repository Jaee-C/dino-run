using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] private float particleTimeout = 2f;
    // [SerializeField] private GameObject particleObject;
    [SerializeField] private Material particleMaterial; 
    private List<ParticleLifetime> particles = new List<ParticleLifetime>();
    private static System.Random rnd = new System.Random();

    struct ParticleLifetime
    {
        public GameObject particle;
        public double createdTime;
    }

    public void generateParticles (Vector3 initialPos, Vector3 direction, float radius, float force, int numParticles)
    {
        // Adding offset in case direction vector is 0
        if (direction.x == 0)
        {
            direction.x = .0001f;
        }
        if (direction.y == 0)
        {
            direction.y = .0001f;
        }
        if (direction.z == 0)
        {
            direction.z = .0001f;
        }

        // Create a list of directions for each particle
        List<Vector3> dirs = new List<Vector3>();

        // Calculate square on plane given by the direction vector
        Vector3 N = direction.normalized;
        Vector3 v1 = new Vector3(1, 1, 0);
        v1.z = (-N.y - N.x) / N.z;
        v1 = v1.normalized;
        Vector3 v2 = Vector3.Cross(N, v1);
        v2 = v2.normalized;

        // Start position of particles
        Vector3 center = initialPos + direction;
        Vector3 v1Start = center - v1 * radius;
        Vector3 v2Start = center - v2 * radius;

        // Step size of each sample point
        float sideLength = Mathf.Sqrt(2 * numParticles);

        // Calculating the direction of each particle
        for(int i = 0; i < sideLength; i++)
        {
            Vector3 currV1 = v1Start + v1 * radius * 2 / sideLength * i;
            for(int j = 0; j < sideLength; j++)
            {
                Vector3 currV2 = v2Start + v2 * radius * 2 / sideLength * j;
                Vector3 currPos = currV2 + (currV1 - center);
                Vector3 dir = currPos - initialPos;
                dir = dir.normalized;
                dirs.Add(dir);
            }
        }

        // List of randomly selected sample points to generate particles
        IEnumerable<Vector3> sampleDirs = dirs.OrderBy(x => rnd.Next()).Take(numParticles);

        // Creating Particles
        foreach(Vector3 dir in sampleDirs)
        {
            GameObject particle = GameObject.CreatePrimitive(PrimitiveType.Cube);
            particle.transform.position = initialPos;
            particle.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            particle.GetComponent<Renderer>().material = particleMaterial;

            particle.AddComponent<Rigidbody>().mass = 0.5f;
            particle.GetComponent<Collider>().enabled = false;
            particle.GetComponent<Rigidbody>().AddForceAtPosition(dir * force, transform.position);

            particles.Add(new ParticleLifetime() { particle = particle, createdTime = Time.realtimeSinceStartupAsDouble });
        }
    }

    // Update is called once per frame
    void Update()
    {
        /* 
         * Testing code
        if (Input.GetKeyDown(KeyCode.Space))
        {
            generateParticles(Vector3.one, new Vector3(0, 1, 0), 100, 100, 300);
        }
        */
        
        // Go through every particle and check if they should be removed
        for (int i = 0; i < particles.Count; i++)
        {
            ParticleLifetime particleInfo = particles[i];

            if (Time.realtimeSinceStartupAsDouble - particleInfo.createdTime > this.particleTimeout)
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratePlane : MonoBehaviour
{
    [SerializeField] private GameObject planeObject;
    [SerializeField] private GameObject obstacleObject;

    [SerializeField]
    [Range(0, 10)]
    private float obstacleChance = 9.8f;

    private Queue<GameObject> planes = new Queue<GameObject>();
    private GameObject firstPlane;
    public static int PLANE_SIZE = 10;

    public GameObject getLastPlane()
    {
        return this.planes.Peek();
    }

    public void destroy()
    {
        GameObject lastPlane = planes.Dequeue();
        Destroy(lastPlane);
    }

    // Randomly generate obstacles for the plane
    public void generateObstacles()
    {
        // Obstacle is generated when the random value is higher than a obstacleChance
        for (float x = firstPlane.transform.position.x - 5; x < firstPlane.transform.position.x + 5; x++)
        {
            for (float z = firstPlane.transform.position.z - 5; z < firstPlane.transform.position.z + 5; z++)
            {
                float height = Random.value * 10f;
                if (height >= obstacleChance)
                {
                    GameObject generatedObstacle = Instantiate(obstacleObject, new Vector3(0, 0, 0), Quaternion.identity);
                    
                    // Size and position of the obstacle. TO BE REMOVED WHEN ACTUAL OBSTACLES IS DONE
                    generatedObstacle.transform.parent = firstPlane.transform;
                    generatedObstacle.transform.localScale = new Vector3(1, height - 7f, 1);
                    generatedObstacle.transform.position = new Vector3(x, 0 + (height - 7f) * 0.5f, z);
                }
            }
        }
    }

    // Spawn a new plane
    public void spawnPlane()
    {
        Vector3 newPosition = firstPlane.transform.position + new Vector3(0, 0, PLANE_SIZE);
        GameObject temp = Instantiate(planeObject, newPosition, Quaternion.identity);
        this.firstPlane = temp;
        generateObstacles();
        this.planes.Enqueue(temp);
    }
    // Start is called before the first frame update
    void Start()
    {
        GameObject plane = Instantiate(planeObject, new Vector3(0, 0, 0), Quaternion.identity);
        planes.Enqueue(plane);
        firstPlane = plane;

        for (int i = 0; i < 15; i++)
        {
            spawnPlane();
        }
    }
}

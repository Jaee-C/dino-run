using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratePlane : MonoBehaviour
{
    [SerializeField] private GameObject planeObject;
    [SerializeField] private GameObject obstacleObject;
    [SerializeField] private GameObject foodObject;
    [SerializeField]
    [Range(0, 1)]
    private float obstacleChance = 0.5f;
    [SerializeField]
    [Range(0, 1)]
    private float foodChance = 0.5f;

    private Queue<GameObject> planes = new Queue<GameObject>();
    private Queue<GameObject> sideTerrains = new Queue<GameObject> ();
    private GameObject firstPlane;
    public static int PLANE_SIZE = 20;

    TerrainGeneration generator;

    struct ObstacleInfo
    {
        public Vector3 position;
        public int radius;
    }

    public GameObject getLastPlane()
    {
        return this.planes.Peek();
    }

    public void destroy()
    {
        GameObject lastPlane = planes.Dequeue();
        Destroy(lastPlane);

        Destroy(sideTerrains.Dequeue());
        Destroy(sideTerrains.Dequeue());
    }

    public double euclideanDistance(Vector3 v1, Vector3 v2)
    {
        return Mathf.Sqrt(Mathf.Pow(v1.x - v2.x, 2) + Mathf.Pow(v1.y - v2.y, 2) + Mathf.Pow(v1.z - v2.z, 2));
    }

    private bool inRadius(Vector3 center, int radius, Vector3 point)
    {
        if(euclideanDistance(center, point) <= radius)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // Randomly generate obstacles for the plane
    public void generateObstacles()
    {
        List<ObstacleInfo> obstacleList = new List<ObstacleInfo>();
        var count = 0;

        // Obstacle is generated when the random value is higher than a obstacleChance
        for (float x = firstPlane.transform.position.x - PLANE_SIZE/2 + 1.5f; x < firstPlane.transform.position.x + PLANE_SIZE/2 - 1.5f; x++)
        {
            for (float z = firstPlane.transform.position.z - 4; z < firstPlane.transform.position.z + 4; z++)
            {
                count ++;
                // Generate an obstacle
                if (Random.value < count / 190f * obstacleChance)
                {
                    bool isValidPos = true;
                    foreach(ObstacleInfo info in obstacleList)
                    {
                        if(inRadius(info.position, info.radius, new Vector3(x, 0, z)))
                        {
                            isValidPos = false;
                            break;
                        }
                    }

                    if (isValidPos)
                    {
                        float rnd = Random.value;
                        // Randomly choose size
                        int size;
                        if (rnd < 0.15f)
                        {
                            size = 1;
                        }
                        else if (rnd < 0.4f)
                        {
                            size = 2;
                        }
                        else
                        {
                            size = 3;
                        }

                        GameObject generatedObject;
                        // Randomly insert food or obstacle
                        if (Random.value < foodChance)
                        {
                            generatedObject = Instantiate(foodObject);
                            // set food's y to 1 to give it the 'hovering' look
                            generatedObject.transform.position = new Vector3(x, 1, z);
                        }
                        else
                        {
                            generatedObject = Instantiate(obstacleObject);
                            generatedObject.transform.position = new Vector3(x, 0, z);
                        }

                        generatedObject.transform.parent = firstPlane.transform;
                        generatedObject.GetComponent<Renderer>().material.color = new Color(1, 0, 0);

                        obstacleList.Add(new ObstacleInfo() { position = new Vector3(x, 0, z), radius = 3*size });
                    }
                }
            }
        }
    }

    // Spawn a new plane
    public void spawnPlane()
    {
        Vector3 newPosition = firstPlane.transform.position + new Vector3(0, 0, 10);
        GameObject temp = Instantiate(planeObject, newPosition, Quaternion.identity);
        this.firstPlane = temp;
        generateObstacles();
        this.planes.Enqueue(temp);

        //this.generator.addPerlin(temp, .7f, .2f, false, false);

        GameObject right = this.generator.generateTerrain(firstPlane.transform.position, true);
        GameObject left = this.generator.generateTerrain(firstPlane.transform.position, false);
        sideTerrains.Enqueue(left);
        sideTerrains.Enqueue(right);
    }
    // Start is called before the first frame update
    void Start()
    {
        this.generator = GameObject.FindObjectOfType<TerrainGeneration>();

        GameObject plane = Instantiate(planeObject, new Vector3(0, 0, -10), Quaternion.identity);
        planes.Enqueue(plane);
        firstPlane = plane;

        for (int i = 0; i < 15; i++)
        {
            spawnPlane();
        }
    }
}

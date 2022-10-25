using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratePlane : MonoBehaviour
{
    [Header("Level 1")]
    [SerializeField] private GameObject plane1;
    [SerializeField] private Material terrainMat1;
    [SerializeField] private GameObject obstacle1;
    [SerializeField] private GameObject food1;

    [Header("Level 1 - 2 Transition")]
    [SerializeField] private GameObject transition1;

    [Header("Level 2")]
    [SerializeField] private GameObject plane2;
    [SerializeField] private Material terrainMat2;
    [SerializeField] private GameObject obstacle2;
    [SerializeField] private GameObject food2;

    [Header("Level 2 - 3 Transition")]
    [SerializeField] private GameObject transition2;

    [Header("Level 3")]
    [SerializeField] private GameObject plane3;
    [SerializeField] private Material terrainMat3;
    [SerializeField] private GameObject obstacle3;
    [SerializeField] private GameObject food3;

    [Header("Level 3 - 4 Transition")]
    [SerializeField] private GameObject transition3;

    [Header("Level 4")]
    [SerializeField] private GameObject plane4;
    [SerializeField] private Material terrainMat4;
    [SerializeField] private GameObject obstacle4;
    [SerializeField] private GameObject food4;

    public struct LevelObjects
    {
        public GameObject plane;
        public Material terrainMat;
        public GameObject obstacle;
        public GameObject food;
    }

    [Header("Obstacles and Food Chances")]
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
    PlayerController player;
    private int currLevel = 1;

    public LevelObjects curr;
    public LevelObjects next;
    private bool isTransition = false;

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
                        int size = 3;

                        GameObject spawnedFood;
                        GameObject spawnedObstacle;

                        if (this.isTransition)
                        {
                            Debug.Log("Is Transition");

                            if(Random.value < 0.5f)
                            {
                                spawnedFood = curr.food;
                                spawnedObstacle = curr.obstacle;
                            }
                            else
                            {
                                spawnedFood = next.food;
                                spawnedObstacle = next.obstacle;
                            }
                        }
                        else
                        {
                            spawnedFood = curr.food;
                            spawnedObstacle = curr.obstacle;
                        }

                        GameObject generatedObject;
                        // Randomly insert food or obstacle
                        if (Random.value < foodChance)
                        {
                            generatedObject = Instantiate(curr.food);
                            generatedObject.transform.position = new Vector3(x, 1, z);
                        }
                        else
                        {
                            generatedObject = Instantiate(curr.obstacle);
                            generatedObject.transform.position = new Vector3(x, 0, z);
                        }

                        generatedObject.transform.parent = firstPlane.transform;
                        //generatedObject.GetComponent<Renderer>().material.color = new Color(1, 0, 0);

                        obstacleList.Add(new ObstacleInfo() { position = new Vector3(x, 0, z), radius = 3*size });
                    }
                }
            }
        }

        if (this.isTransition)
        {
            this.curr = this.next;
            this.isTransition = false;
        }
    }

    // Spawn a new plane
    public void spawnPlane()
    {
        Vector3 newPosition = firstPlane.transform.position + new Vector3(0, 0, 10);
        GameObject temp = Instantiate(curr.plane, newPosition, Quaternion.identity);
        this.firstPlane = temp;
        generateObstacles();
        this.planes.Enqueue(temp);

        //this.generator.addPerlin(temp, .7f, .2f, false, false);

        GameObject right = this.generator.generateTerrain(firstPlane.transform.position, true, curr.terrainMat);
        GameObject left = this.generator.generateTerrain(firstPlane.transform.position, false, curr.terrainMat);
        sideTerrains.Enqueue(left);
        sideTerrains.Enqueue(right);
    }
    // Start is called before the first frame update
    void Start()
    {
        this.generator = GameObject.FindObjectOfType<TerrainGeneration>();
        this.player = GameObject.FindObjectOfType<PlayerController>();
        this.curr = new LevelObjects() { plane = plane1, obstacle = obstacle1, food = food1, terrainMat = terrainMat1 };

        GameObject plane = Instantiate(plane1, new Vector3(0, 0, -10), Quaternion.identity);
        planes.Enqueue(plane);
        firstPlane = plane;

        for (int i = 0; i < 15; i++)
        {
            spawnPlane();
        }
    }

    void Update()
    {
        if (player.level != currLevel)
        {
            Debug.Log("Level switch");
            // Only 4 levels right now
            switch (player.level)
            {
                case 2:
                    this.curr.plane = transition1;
                    this.next = new LevelObjects { plane = plane2, obstacle = obstacle2, food = food2, terrainMat = terrainMat2 };
                    break;
                case 3:
                    this.curr.plane = transition2;
                    this.next = new LevelObjects { plane = plane3, obstacle = obstacle3, food = food3, terrainMat = terrainMat3 };
                    break;
                case 4:
                    this.curr.plane = transition3;
                    this.next = new LevelObjects { plane = plane4, obstacle = obstacle4, food = food4, terrainMat = terrainMat4 };
                    break;
            }
            
            this.isTransition = true;
            currLevel = player.level;
        }
    }
}

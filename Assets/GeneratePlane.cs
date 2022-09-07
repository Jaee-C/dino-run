using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratePlane : MonoBehaviour
{
    private GameObject savedPlane;
    private GameObject firstPlane;
    private GameObject obstacle;
    private Queue<GameObject> planes = new Queue<GameObject>();
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

    public void generateObstacles()
    {
        for (float x = firstPlane.transform.position.x - 5; x < firstPlane.transform.position.x + 5; x++)
        {
            for(float z = firstPlane.transform.position.z - 5; z < firstPlane.transform.position.z + 5; z++)
            {
                float height = Random.value * 10f;
                if(height >= 9.8f)
                {
                    GameObject generatedObstacle = Instantiate(obstacle, new Vector3(0, 0, 0), Quaternion.identity);
                    generatedObstacle.transform.parent = firstPlane.transform;
                    generatedObstacle.transform.localScale = new Vector3(1, height - 7f, 1);
                    generatedObstacle.transform.position = new Vector3(x, 0 + (height - 7f) * 0.5f, z);
                }
            }
        }
    }
    
    public void spawnPlane()
    {
        Vector3 newPosition = firstPlane.transform.position + new Vector3(0, 0, PLANE_SIZE);
        GameObject temp = Instantiate(savedPlane, newPosition, Quaternion.identity);
        this.firstPlane = temp;
        generateObstacles();
        this.planes.Enqueue(temp);
    }
    // Start is called before the first frame update
    void Start()
    {
        savedPlane = GameObject.Find("Plane");
        GameObject plane = Instantiate(savedPlane, new Vector3(0, 0, 0), Quaternion.identity);
        planes.Enqueue(plane);
        firstPlane = plane;
        obstacle = GameObject.Find("Obstacle");

        for (int i = 0; i < 15; i++)
        {
            spawnPlane();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

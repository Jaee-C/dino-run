using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratePlane : MonoBehaviour
{
    static GameObject savedPlane;
    public static GameObject plane;
    static GameObject obstacle;
    private static Vector3 endPosition;
    private static Vector3 offset;

    static void generateObstacles()
    {
        int numGen = 0;
        for (float x = plane.transform.position.x - 5; x < plane.transform.position.x + 5; x++)
        {
            for(float z = plane.transform.position.z - 5; z < plane.transform.position.z + 5; z++)
            {
                float height = Random.value * 10f;
                if(height >= 9.8f)
                {
                    numGen++;
                    GameObject generatedObstacle = Instantiate(obstacle, new Vector3(0, 0, 0), Quaternion.identity);
                    generatedObstacle.transform.parent = plane.transform;
                    generatedObstacle.transform.localScale = new Vector3(1, height - 7f, 1);
                    generatedObstacle.transform.position = new Vector3(x, 0 + (height - 7f) * 0.5f, z);
                }
            }
        }
        Debug.Log(numGen);
    }

    public static void spawnPlane()
    {
        GameObject temp = Instantiate(savedPlane, endPosition, Quaternion.identity);
        endPosition = plane.transform.GetChild(0).transform.position + offset;
        plane = temp;
        generateObstacles();
    }
    // Start is called before the first frame update
    void Start()
    {
        savedPlane = GameObject.Find("Plane");
        plane = Instantiate(savedPlane, savedPlane.transform.position, Quaternion.identity);
        offset = savedPlane.transform.GetChild(0).transform.position - savedPlane.transform.position;

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

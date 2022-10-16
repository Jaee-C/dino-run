using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGeneration : MonoBehaviour
{
    [SerializeField] float sampleSize = 5f;
    [SerializeField] float multiplier = 3f;
    // Start is called before the first frame update
    void Start()
    {
        //generateTerrain(Vector3.zero);
    }

    public GameObject generateTerrain(Vector3 pos, bool isRight)
    {
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.transform.position = pos;

        Vector3[] vertices = plane.GetComponent<MeshFilter>().mesh.vertices;
        float iStart = Random.value * 10;
        float jStart = Random.value * 10;

        int count = 0;
        for (float i = iStart; i < iStart + sampleSize; i += sampleSize / 11f)
        {
            for (float j = jStart; j < jStart + sampleSize; j += sampleSize / 11f)
            {
                if (count < 121)
                {
                    if((count + 1) % 11 == 0 && isRight)
                    {
                        vertices[count].y = -10f;
                        count++;
                    }
                    else if(count % 11 == 0 && !isRight)
                    {
                        vertices[count].y = -10f;
                        count++;
                    }
                    else
                    {
                        vertices[count].y = Mathf.PerlinNoise(i, j) * multiplier;
                        count++;
                    }
                    
                }
            }
        }

        foreach (Vector3 vertex in vertices)
        {
            Debug.Log(vertex);
        }

        plane.GetComponent<MeshFilter>().mesh.vertices = vertices;
        plane.GetComponent<MeshFilter>().mesh.RecalculateBounds();
        plane.GetComponent<MeshFilter>().mesh.RecalculateNormals();

        plane.transform.localScale = new Vector3(2, 1, 2);

        return plane;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

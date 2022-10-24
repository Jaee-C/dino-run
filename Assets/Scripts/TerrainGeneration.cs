using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGeneration : MonoBehaviour
{
    [SerializeField] float sampleSize = 5f;
    [SerializeField] float multiplier = 3f;

    // Position Offsets
    [SerializeField] private float height = -10f;
    private float X_OFFSET = 30f;
    private float Y_OFFSET = 2f;



    // Start is called before the first frame update
    void Start()
    {
        //generateTerrain(Vector3.zero);
    }

    public void addPerlin(GameObject plane, float heightScale, float heightOffset, bool addOffset, bool isRight)
    {
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
                    if ((count + 1) % 11 == 0 && isRight && addOffset)
                    {
                        vertices[count].y = height;
                        count++;
                    }
                    else if (count % 11 == 0 && !isRight && addOffset)
                    {
                        vertices[count].y = height;
                        count++;
                    }
                    else
                    {
                        vertices[count].y = Mathf.PerlinNoise(i, j) * heightScale - heightOffset;
                        count++;
                    }

                }
            }
        }

        plane.GetComponent<MeshFilter>().mesh.vertices = vertices;
        plane.GetComponent<MeshFilter>().mesh.RecalculateBounds();
        plane.GetComponent<MeshFilter>().mesh.RecalculateNormals();
    }

    public GameObject generateTerrain(Vector3 pos, bool isRight, Material terrainMaterial)
    {
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        if (isRight) {
            plane.transform.position = pos + new Vector3(X_OFFSET, Y_OFFSET, 0);
        } else {
            plane.transform.position = pos + new Vector3(-X_OFFSET, Y_OFFSET, 0);
        }
        plane.GetComponent<Renderer>().material = terrainMaterial;

        addPerlin(plane, multiplier, 0f, true, isRight);

        plane.transform.localScale = new Vector3(4, 1, 2);

        return plane;
    }

    // Update is called once per frame
    void Update()
    {

    }
}

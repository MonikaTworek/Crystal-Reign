using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.MapGeneration
{
    public class CubeGenerator : MonoBehaviour
    {

        public int cubesAmount = 1000;
        public bool inGrid = false; //lol
        public float gridUnit = 0.8f;
        public float gridRange = 80;

        private bool[,,] gridCubes;

        // Use this for initialization
        void Start()
        {
            if (inGrid)
            {
                gridCubes = new bool[(int)(gridRange / gridUnit), (int)(gridRange / gridUnit), (int)(gridRange / gridUnit)];
                for (int i = 0; i < (int)(gridRange / gridUnit); i++)
                    for (int j = 0; j < (int)(gridRange / gridUnit); j++)
                        for (int k = 0; k < (int)(gridRange / gridUnit); k++)
                            gridCubes[i, j, k] = false;
            }

            for (int i = 0; i < cubesAmount; i++)
            {
                float distance = Mathf.Pow(Random.Range(0, 6f), 2);
                float angleX = Random.Range(0, Mathf.PI * 2);
                float angleY = Random.Range(0, Mathf.PI);
                Vector3 position = new Vector3(distance * Mathf.Sin(angleY) * Mathf.Cos(angleX) + 40, distance * Mathf.Sin(angleY) * Mathf.Sin(angleX) + 40,
                    distance * Mathf.Cos(angleY) + 40);
                float scale = Random.Range(0, 0.5f) * (36 - distance);

                if (inGrid)
                    createGridCube(position, scale);
                else
                    createCube(position, scale);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        void createCube(Vector3 position, float scale)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = position;
            cube.transform.localScale = new Vector3(scale, scale, scale);
        }

        void createGridCube(Vector3 position, float scale)
        {
            for (int i = (int)((position.x - scale / 2) / gridUnit); i < (position.x + scale / 2) / gridUnit; i++)
                for (int j = (int)((position.y - scale / 2) / gridUnit); j < (position.y + scale / 2) / gridUnit; j++)
                    for (int k = (int)((position.z - scale / 2) / gridUnit); k < (position.z + scale / 2) / gridUnit; k++)
                    {
                        if (!gridCubes[i, j, k])
                        {
                            gridCubes[i, j, k] = true;
                            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            cube.transform.position = new Vector3(i * gridUnit + gridUnit / 2, j * gridUnit + gridUnit / 2, k * gridUnit + gridUnit / 2);
                            cube.transform.localScale = new Vector3(gridUnit * 0.9f, gridUnit * 0.9f, gridUnit * 0.9f);
                        }
                    }
        }
    }
}

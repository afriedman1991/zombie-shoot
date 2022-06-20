using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{

    public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    public int columns = 10;
    public int rows = 10;
    public int level = 1;

    public Count wallCount = new Count(7, 11);


    // Environmental objects
    public GameObject[] floorObjects;
    public GameObject[] wallObjects;
    public GameObject[] outerWallObjects;
    public GameObject[] enemyObjects;
    public GameObject board;

    private int typeId;
    private Transform boardHolder;
    private Transform outerWallsAndFloors;
    private Transform innerWalls;

    private List<Vector3> gridPositions = new List<Vector3>();

    public int GetAgentTypeIdByName(string agentTypeName)
    {
        int count = NavMesh.GetSettingsCount();
        string[] agentTypeNames = new string[count + 2];
        for (var i = 0; i < count; i++)
        {
            int id = NavMesh.GetSettingsByIndex(i).agentTypeID;
            string name = NavMesh.GetSettingsNameFromID(id);
            if (name == agentTypeName)
            {
                return id;
            }
        }
        return -1;
    }

    void InitialiseList()
    {
        gridPositions.Clear();

        for (int x = 1; x < columns - 1; x++)
        {
            for (int z = 1; z < rows - 1; z++)
            {
                gridPositions.Add(new Vector3(x, 0f, z));
            }
        }
    }

    void BoardSetup()
    {
        int agentId = GetAgentTypeIdByName("Enemy");

        board.GetComponent<NavMeshSurface>().agentTypeID = agentId;
        boardHolder = board.transform;

        outerWallsAndFloors = new GameObject("outerWallsAndFloors").transform;

        for (int x = -1; x < columns + 1; x++)
        {
            for (int z = -1; z < rows + 1; z++)
            {
                GameObject toInstantiate = floorObjects[Random.Range(0, floorObjects.Length)];
                if (x == -1 || x == columns || z == -1 || z == rows)
                {
                    toInstantiate = outerWallObjects[Random.Range(0, outerWallObjects.Length)];
                }

                GameObject instance = Instantiate(toInstantiate, new Vector3(x, 0f, z), Quaternion.identity) as GameObject;

                instance.transform.SetParent(outerWallsAndFloors);
                outerWallsAndFloors.transform.SetParent(boardHolder);
            }
        }
    }

    Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }

    void LayoutObjectAtRandom(GameObject[] objectArray, int minimum, int maximum)
    {
        innerWalls = new GameObject("innerWalls").transform;
        boardHolder = board.transform;
        int objectCount = Random.Range(minimum, maximum + 1);

        for (int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();
            GameObject objectChoice = objectArray[Random.Range(0, objectArray.Length)];

            GameObject instance = Instantiate(objectChoice, randomPosition, Quaternion.identity);

            instance.transform.SetParent(innerWalls);
            innerWalls.transform.SetParent(boardHolder);
        }
    }

    public void BakeNavMesh()
    {
        board.GetComponent<NavMeshSurface>().BuildNavMesh();
    }

    public void SetupScene(int level)
    {
        BoardSetup();
        InitialiseList();
        LayoutObjectAtRandom(wallObjects, wallCount.minimum, wallCount.maximum);

        BakeNavMesh();

        int enemyCount = (int)Math.Ceiling(Mathf.Log(level, 2f));

        LayoutObjectAtRandom(enemyObjects, enemyCount, 4);
    }
}

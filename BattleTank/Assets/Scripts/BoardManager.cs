using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{

    [Serializable]
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

    public int rows = 15;
    public int columns = 15;

    public GameObject[] floorTiles;
    public GameObject[] borderWallTiles;

    public int destructibleCount = 100;
    public GameObject[] destructibleTiles;

    //public int lavaFireObj = 20;
    //public GameObject[] lavaFireTiles;

    public GameObject heartObject;
    public GameObject nuclearPickup;

    public int grassFieldSize = 9;
    public GameObject grassObject;


    //public int lavaFieldSize = 10;
    //public GameObject lavaObject;

    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();

    void InitializeList()
    {
        gridPositions.Clear();

        for (int x = 0; x < columns; x++)
        {
            for (int y = 1; y < rows - 1; y++)
            {
                if (!((x > columns / 2 - 1 && x < columns / 2 + 2) && (y == 2 || y == rows - 2)))
                {
                    gridPositions.Add(new Vector3(x, y, 0f));
                }
            }
        }
    }

    void BoardSetup()
    {
        boardHolder = new GameObject("Board").transform;

        for (int x = -1; x < columns + 1; x++)
        {
            for (int y = -1; y < rows + 1; y++)
            {
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                if (x == -1 || x == columns || y == -1 || y == rows)
                {
                    toInstantiate = borderWallTiles[Random.Range(0, borderWallTiles.Length)];
                }

                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                instance.transform.SetParent(boardHolder);
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


    void LayoutObjectAtRandom(GameObject[] tileArray, Count range)
    {
        int objectCount = Random.Range(range.minimum, range.maximum + 1);

        for (int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            GameObject instance = Instantiate(tileChoice, randomPosition, Quaternion.identity);
            //GameObject tileChoicelava = tileArray[Random.Range(0, tileArray.Length)];
            //GameObject instanceLava = Instantiate(tileChoicelava, randomPosition, Quaternion.identity);
           // instanceLava.transform.SetParent(boardHolder);
            instance.transform.SetParent(boardHolder);
                //do not touch this part !!!
         
        }
    }

    void AddRandomGrassField()
    {

        int randomX = Random.Range(1, columns - grassFieldSize - 1);
        int randomY = Random.Range(1, rows - grassFieldSize - 1);


        for (int x = randomX; x < (randomX + grassFieldSize); x++)
        {
            for (int y = randomY; y < (randomY + grassFieldSize); y++)
            {
                int randomSeed = Random.Range(0, 3);
                if (randomSeed < 2)
                {
                    GameObject grass = Instantiate(grassObject, new Vector3(x, y, 0.0f), Quaternion.identity);
                    grass.transform.SetParent(boardHolder);

                    Predicate<Vector3> vectorPred = (Vector3 v) => {
                        return (v.x == (float)x && v.y == (float)y);
                    };
                    int index = gridPositions.FindIndex(vectorPred);
                    if (index >= 0 && index < gridPositions.Count)
                    {
                        gridPositions.RemoveAt(index);
                    }
                }
            }
        }
    }



    public void SetupScene()
    {
        ClearBoard();
        BoardSetup();
        InitializeList();
        AddRandomGrassField();
       // AddRandomLavaObject();
        LayoutObjectAtRandom(destructibleTiles, new Count(destructibleCount / 2, destructibleCount));
       // LayoutObjectAtRandom(lavaFireTiles, new Count(lavaFireObj / 2, lavaFireObj));
    }

    void ClearBoard()
    {
        if (boardHolder)
        {
            Destroy(boardHolder.gameObject);
        }
    }

    public void AddRandomHeart()
    {
        Vector3 randomPosition = RandomPosition();
        GameObject instance = Instantiate(heartObject, randomPosition, Quaternion.identity);
        instance.transform.SetParent(boardHolder);
    }

    public void AddRandomNuclearPickup()
    {
        Vector3 randomPosition = RandomPosition();
        GameObject instance = Instantiate(nuclearPickup, randomPosition, Quaternion.identity);
        instance.transform.SetParent(boardHolder);
    }

}

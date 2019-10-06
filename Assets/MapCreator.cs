using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class MapCreator : MonoBehaviour
{
    public Tile Tile;
    public int width = 100;
    public int height = 100;
    public int deathLimit = 3;
    public int birthLimit = 4;
    public int numberOfSteps = 6;
    public float chanceToStartAlive = 0.32f;
    public GameObject PlayerObject;
    public GameObject Camera;

    private bool[,] initialiseMap(bool[,] map) {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (Random.Range(0f, 1f) < chanceToStartAlive) {
                    map[x, y] = true;
                }
            }
        }
        return map;
    }
    //Returns the number of cells in a ring around (x,y) that are alive.
    private int countAliveNeighbours(bool[,] map, int x, int y) {
        int count = 0;
        for (int i = -1; i < 2; i++) {
            for (int j = -1; j < 2; j++) {
                int neighbour_x = x + i;
                int neighbour_y = y + j;
                //If we're looking at the middle point
                if (i == 0 && j == 0) {
                    //Do nothing, we don't want to add ourselves in!
                }
                //In case the index we're looking at it off the edge of the map
                else if (neighbour_x < 0 || neighbour_y < 0 || neighbour_x >= width || neighbour_y >= height) {
                    count = count + 1;
                }
                //Otherwise, a normal check of the neighbour
                else if (map[neighbour_x, neighbour_y]) {
                    count = count + 1;
                }
            }
        }
        return count;
    }

    private bool[,] doSimulationStep(bool[,] oldMap) {
        bool[,] newMap = new bool[width,height];
        //Loop over each row and column of the map
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                int nbs = countAliveNeighbours(oldMap, x, y);
                //The new value is based on our simulation rules
                //First, if a cell is alive but has too few neighbours, kill it.
                if (oldMap[x, y]) {
                    if (nbs < deathLimit) {
                        newMap[x, y] = false;
                    } else {
                        newMap[x, y] = true;
                    }
                } //Otherwise, if the cell is dead now, check if it has the right number of neighbours to be 'born'
                else {
                    if (nbs > birthLimit) {
                        newMap[x, y] = true;
                    } else {
                        newMap[x, y] = false;
                    }
                }
            }
        }
        return newMap;
    }

    private bool[,] fillEdges(bool[,] oldMap) {
        for (int x = 0; x < width; x++) {
            oldMap[x, 0] = true;
            oldMap[x, height-1] = true;
        }
        for (int y = 0; y < height; y++) {
            oldMap[0, y] = true;
            oldMap[width-1, y] = true;
        }
        return oldMap;
    }

    private void spawnPlayer(bool[,] world) {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (!world[x, y]) {
                    GameObject projectile = Instantiate(PlayerObject, new Vector3(x*.6f, y*.6f, 0), Quaternion.identity) as GameObject;
                    GameObject camera = Instantiate(Camera, new Vector3(x * .6f, y * .6f, -10), Quaternion.identity) as GameObject;
                    return;
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        bool[,] cellmap = new bool[width, height];
        cellmap = initialiseMap(cellmap);
        for (int i = 0; i < numberOfSteps; i++) {
            cellmap = doSimulationStep(cellmap);
        }
        cellmap = fillEdges(cellmap);
        Tilemap tm = GetComponent<Tilemap>();
        for (var i = 0; i < width; i++) {
            for (var j = 0; j < height; j++) {
                Vector3Int v = new Vector3Int(i, j, 0);
                if (cellmap[i, j]) {
                    tm.SetTile(v, Tile);
                }
            }
        }
        spawnPlayer(cellmap);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

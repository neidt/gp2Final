using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControls : MonoBehaviour
{

    public GameObject blackMoku, whiteMoku;

    public bool isBlacksTurn = true;

    public GameObject[,] stones = new GameObject[19, 19];

    public List<Stone> blackStones = new List<Stone>();
    public List<Stone> whiteStones = new List<Stone>();
    public List<Stone> stonesList = new List<Stone>();

    // Use this for initialization
    void Start()
    {
        GameObject[] existingStones = GameObject.FindGameObjectsWithTag("Moku");
        for (int i = 0; i < existingStones.Length; i++)
        {
            Vector3 existingStoneLocation = existingStones[i].transform.position;
            stones[Mathf.RoundToInt(existingStoneLocation.x), Mathf.RoundToInt(existingStoneLocation.y)] = existingStones[i];
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int bx = Mathf.RoundToInt(mousePosition.x), by = Mathf.RoundToInt(mousePosition.y);

            if (bx >= 0 && bx <= 18 && by >= 0 && by <= 18 && stones[bx, by] == null)
            {
                GameObject newMoku = GameObject.Instantiate(isBlacksTurn ? blackMoku : whiteMoku);
                newMoku.transform.position = new Vector3(bx, by, 0);
                Stone madeMoku = new Stone(newMoku.transform.position);

                stonesList.Add(madeMoku);

                if (isBlacksTurn)
                {
                    blackStones.Add(madeMoku);
                }
                else
                {
                    whiteStones.Add(madeMoku);
                }

                stones[bx, by] = newMoku;

                RemoveCapturedStones();

                isBlacksTurn = !isBlacksTurn;
            }
        }
    }

    private void RemoveCapturedStones()
    {
        // TODO: Fill out this function.
        // Any stones that are now surrounded because of the placement of the new stone
        // should be removed.

        //black stones
        if (isBlacksTurn)
        {
            //check if there are any surrounding stones for black stones
            foreach (Stone bstone in blackStones)
            {
                if (IsStoneThere(new Vector3(bstone.stonePosition.x, bstone.stonePosition.y + 1, 0)))
                {
                    bstone.isConnectedUp = true;
                    bstone.ConnectedSpaces.Add(new Connection(Vector2Int.up, bstone));
                }
                if (IsStoneThere(new Vector3(bstone.stonePosition.x + 1, bstone.stonePosition.y, 0)))
                {
                    bstone.isConnectedRight = true;
                    bstone.ConnectedSpaces.Add(new Connection(Vector2Int.right, bstone));
                }
                if (IsStoneThere(new Vector3(bstone.stonePosition.x, bstone.stonePosition.y - 1, 0)))
                {
                    bstone.isConnectedDown = true;
                    bstone.ConnectedSpaces.Add(new Connection(Vector2Int.down, bstone));
                }
                if (IsStoneThere(new Vector3(bstone.stonePosition.x - 1, bstone.stonePosition.y, 0)))
                {
                    bstone.isConnectedLeft = true;
                    bstone.ConnectedSpaces.Add(new Connection(Vector2Int.left, bstone));
                }
            }
        }

        //white stones
        if (!isBlacksTurn)
        {
            foreach (Stone wstone in whiteStones)
            {
                if (IsStoneThere(new Vector3(wstone.stonePosition.x, wstone.stonePosition.y + 1, 0)))
                {
                    wstone.isConnectedUp = true;
                    wstone.ConnectedSpaces.Add(new Connection(Vector2Int.up, wstone));
                }
                if (IsStoneThere(new Vector3(wstone.stonePosition.x + 1, wstone.stonePosition.y, 0)))
                {
                    wstone.isConnectedRight = true;
                    wstone.ConnectedSpaces.Add(new Connection(Vector2Int.right, wstone));
                }
                if (IsStoneThere(new Vector3(wstone.stonePosition.x, wstone.stonePosition.y - 1, 0)))
                {
                    wstone.isConnectedDown = true;
                    wstone.ConnectedSpaces.Add(new Connection(Vector2Int.down, wstone));
                }
                if (IsStoneThere(new Vector3(wstone.stonePosition.x - 1, wstone.stonePosition.y, 0)))
                {
                    wstone.isConnectedLeft = true;
                    wstone.ConnectedSpaces.Add(new Connection(Vector2Int.left, wstone));
                }
            }
        }
        
        for (int i = 0; i < 19; i++)
        {
            for (int j = 0; j < 19; j++)
            {
                if (stones[i, j] == blackMoku &&
                    stones[i + 1, j] == whiteMoku && stones[i, j + 1] == whiteMoku &&
                    stones[i - 1, j] == whiteMoku && stones[i, j - 1] == whiteMoku)
                    Destroy(stones[i, j]);
                //destroy(blackMoku);???
            }
        }

        // You do NOT need to worry about scoring - just remove any stones that are captured.
    }

    private bool IsStoneThere(Vector3 pos)
    {
        foreach (Stone stone in stonesList)
        {
            if (stone.stonePosition == pos)
            {
                return true;
            }
        }
        return false;
    }
}



public class Stone
{
    public bool isConnectedUp;
    public bool isConnectedDown;
    public bool isConnectedRight;
    public bool isConnectedLeft;

    public GameObject stoneObj;
    public Vector3 stonePosition;
    public Transform stoneTransform;
    public List<Connection> ConnectedSpaces = new List<Connection>();

    public Stone()
    {
        isConnectedUp = false;
        isConnectedRight = false;
        isConnectedDown = false;
        isConnectedLeft = false;
    }
    public Stone(Vector3 position)
    {
        stonePosition = position;
        isConnectedUp = false;
        isConnectedRight = false;
        isConnectedDown = false;
        isConnectedLeft = false;
    }

    public bool isSurrounded()
    {
        if (isConnectedUp == false && isConnectedRight == false &&
        isConnectedDown == false && isConnectedLeft == false && ConnectedSpaces.Count == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

public class Connection
{
    public Vector2Int dirFrom;
    public Stone stoneConnectedTo;

    public Connection(Vector2Int dirConnected, Stone connectedStone)
    {
        dirFrom = dirConnected;
        stoneConnectedTo = connectedStone;
    }
}

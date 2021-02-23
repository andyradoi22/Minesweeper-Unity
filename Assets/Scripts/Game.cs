using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public Tile[,] grid = new Tile[9, 9];
    public List<Tile> tilesToCheck = new List<Tile>();

    // Start is called before the first frame update
    void Start()
    {
        for (int index = 0; index < 10; index++)
        {
            PlaceMines();
        }

        PlaceClues();

        PlaceBlanks();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            int coord1 = Mathf.RoundToInt(mousePosition.x);
            int coord2  = Mathf.RoundToInt(mousePosition.y);

            Tile tile = grid[coord1, coord2];

            if (tile.tileState == Tile.TileState.Normal)
            {
                if (tile.isCovered)
                {
                    if (tile.tileKind == Tile.TileKind.Mine)
                    {
                        GameOver(tile);
                    }
                    else
                    {
                        tile.SetIsCovered(false);
                    }

                    if (tile.tileKind == Tile.TileKind.Blank)
                    {
                        RevealAdjacentTileForTileAt(coord1, coord2);
                    }
                }  
            }
        }
    }

    private void GameOver(Tile tile)
    {
        tile.SetClickedMine();
        GameObject[] gameObject = GameObject.FindGameObjectsWithTag("Mine");

        foreach(GameObject go in gameObject)
        {
            Tile t = go.GetComponent<Tile>();
            if (t != tile)
            {
                if (t.tileState == Tile.TileState.Normal)
                {
                    t.SetIsCovered(false);
                }
            }
        }

        for (int coord2 = 0; coord2 < 9; coord2++)
        {
            for (int coord1 = 0; coord1 < 9; coord1++)
            {
                Tile t = grid[coord1, coord2];

                if (t.tileState == Tile.TileState.Flagged)
                {
                    if (t.tileKind != Tile.TileKind.Mine)
                    {
                        t.SetNotAMineFlagged();
                    }
                }
            }
        }
    }

    void PlaceMines()
    {
        int coord1 = UnityEngine.Random.Range(0, 9);
        int coord2 = UnityEngine.Random.Range(0, 9);

        if (grid[coord1, coord2] == null)
        {
            Tile mineTile = Instantiate(Resources.Load("Prefabs/mine", typeof(Tile)), new Vector3 (coord1, coord2, 0), Quaternion.identity) as Tile;

            grid[coord1, coord2] = mineTile;

            Debug.Log("(" + coord1 + ", " + coord2 + ")");
        }
        else
        {
            PlaceMines();
        }
    }

    void PlaceClues()
    {
        for (int coord2 = 0; coord2 < 9; coord2++)
        {
            for (int coord1 = 0; coord1 < 9; coord1++)
            {
                if (grid[coord1, coord2] == null)
                {
                    //Nothing is here; can 't be a mine
                    int numMines = 0;

                    //North
                    if (coord2 + 1 < 9)
                    {
                        if (grid[coord1, coord2 + 1] != null && grid[coord1, coord2 + 1].tileKind == Tile.TileKind.Mine)
                        {
                        numMines++;
                        }
                    }

                    //East
                    if (coord1 + 1 < 9)
                    {
                        if (grid[coord1 + 1, coord2] != null && grid[coord1 + 1, coord2].tileKind == Tile.TileKind.Mine)
                        {
                            numMines++;
                        }
                    }

                    //South
                    if (coord2 - 1 >= 0)
                    {
                        if(grid[coord1, coord2 - 1] != null && grid[coord1, coord2 - 1].tileKind == Tile.TileKind.Mine)
                        {
                            numMines++;
                        }
                    }

                    //West
                    if (coord1 - 1 >= 0)
                    {
                        if (grid[coord1 - 1, coord2] != null && grid[coord1 - 1, coord2].tileKind == Tile.TileKind.Mine)
                        {
                            numMines++;
                        }
                    }

                    //NordEast
                    if (coord1 + 1 < 9 && coord2 + 1 < 9)
                    {
                        if(grid[coord1 + 1, coord2 + 1] != null && grid[coord1 + 1, coord2 + 1].tileKind == Tile.TileKind.Mine)
                        {
                            numMines++;
                        }
                    }

                    //NorthWest
                    if (coord1 - 1 >= 0 && coord2 + 1 < 9)
                    {
                        if(grid[coord1 - 1, coord2 + 1] != null && grid[coord1 - 1, coord2 + 1].tileKind == Tile.TileKind.Mine)
                        {
                            numMines++;
                        }
                    }

                    //SouthEast
                    if (coord1 + 1 < 9 && coord2 - 1 >= 0)
                    {
                        if (grid[coord1 + 1, coord2 - 1] != null && grid[coord1 + 1, coord2 - 1].tileKind == Tile.TileKind.Mine)
                        {
                            numMines++;
                        }
                    }

                    //SouthWest
                    if (coord1 - 1 >= 0 && coord2 - 1 >= 0)
                    {
                        if (grid[coord1 - 1, coord2 - 1] != null && grid[coord1 - 1, coord2 - 1].tileKind == Tile.TileKind.Mine)
                        {
                            numMines++;
                        }
                    }

                    if (numMines > 0)
                    {
                        Tile clueTile = Instantiate(Resources.Load("Prefabs/" + numMines, typeof(Tile)), new Vector3 (coord1, coord2, 0), Quaternion.identity) as Tile;

                        grid[coord1, coord2] = clueTile;
                    }
                }
            }
        }
    }

    void PlaceBlanks()
    {
        for (int coord2 = 0; coord2 < 9; coord2++)
        {
            for (int coord1 = 0; coord1 < 9; coord1++)
            {
                if (grid[coord1, coord2] == null)
                {
                    Tile blankTile = Instantiate(Resources.Load("Prefabs/blank", typeof(Tile)), new Vector3(coord1, coord2, 0), Quaternion.identity) as Tile;

                    grid[coord1, coord2] = blankTile;
                }
            }
        }
    }

    void RevealAdjacentTileForTileAt(int coord1, int coord2)
    {
        //North
        if ((coord2 + 1) < 9)
        {
            CheckTileAt(coord1, coord2 + 1);
        }

        //East
        if ((coord1 + 1) < 9)
        {
            CheckTileAt(coord1 + 1, coord2);
        }

        //South
        if((coord2 - 1) >= 0)
        {
            CheckTileAt(coord1, coord2 - 1);
        }

        //West
        if ((coord1 - 1) >= 0)
        {
            CheckTileAt(coord1 - 1, coord2);
        }

        //NorthEast
        if ((coord2 + 1) < 9 && (coord1 + 1) < 9)
        {
            CheckTileAt(coord1 + 1, coord2 + 1);
        }

        //NorthWest
        if ((coord2 + 1) < 9 && (coord1 - 1) >= 0)
        {
            CheckTileAt(coord1 - 1, coord2 + 1);
        }

        //SouthEast
        if ((coord2 - 1) >= 0 && (coord1 + 1) < 9)
        {
            CheckTileAt(coord1 + 1, coord2 - 1);
        }

        //SouthWest
        if ((coord2 - 1) >= 0 && (coord1 - 1) >= 0)
        {
            CheckTileAt(coord1 - 1, coord2 - 1);
        }

        for (int index = tilesToCheck.Count - 1; index >=0; index--)
        {
            if (tilesToCheck[index].didCheck)
            {
                tilesToCheck.RemoveAt(index);
            }
        }

        if (tilesToCheck.Count > 0)
        {
            RevealAdjacentTileForTiles();
        }
    }

    private void RevealAdjacentTileForTiles()
    {
        for (int index = 0; index < tilesToCheck.Count; index++)
        {
            Tile tile = tilesToCheck[index];
            int coord1 = (int)tile.gameObject.transform.localPosition.x;
            int coord2 = (int)tile.gameObject.transform.localPosition.y;

            tile.didCheck = true;

            if (tile.tileState != Tile.TileState.Flagged)
            {
                tile.SetIsCovered(false);
            }

            //we know all the tilesToCheck list are all blanks so we don't have to check if
            //the tile is a blanktile before calling RevealAdjacentTilesForTileAt(coord1, coord2)
            //RevealAdjacentTilesForTileAt(coord1, coord2);
            RevealAdjacentTileForTileAt(coord1, coord2);
        }
    }

    private void CheckTileAt(int coord1, int coord2)
    {
        Tile tile = grid[coord1, coord2];

        if (tile.tileKind == Tile.TileKind.Blank)
        {
            tilesToCheck.Add(tile);
            Debug.Log("Tile @ (" + coord1 + ", " + coord2 + ") is a Blank Tile");
        }
        else if (tile.tileKind == Tile.TileKind.Clue)
        {
            tile.SetIsCovered(false);
            Debug.Log("Tile @ (" + coord1 + ", " + coord2 + ") is a Clue Tile");
        }
        else if (tile.tileKind == Tile.TileKind.Mine)
        {
            Debug.Log("Tile @ (" + coord1 + ", " + coord2 + ") is a Mine Tile");
        }
    }
}

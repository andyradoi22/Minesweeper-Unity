using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public enum TileKind
    {
        Blank,
        Mine,
        Clue
    }

    public enum TileState
    {
        Normal,
        Flagged
    }

    public bool isCovered = true;
    public bool didCheck = false;

    public Sprite coveredSprite;
    public Sprite flagSprite;
    public Sprite mineClicked;
    public Sprite flagNotAMineSprite;

    public TileKind tileKind = TileKind.Blank;
    public TileState tileState = TileState.Normal;

    private Sprite defaultSprite;

    private void Start()
    {
        defaultSprite = GetComponent<SpriteRenderer>().sprite;

        GetComponent<SpriteRenderer>().sprite = coveredSprite;
    }

    public void SetIsCovered(bool covered)
    {
        isCovered = false;
        GetComponent<SpriteRenderer>().sprite = defaultSprite;
    }

    public void SetClickedMine()
    {
        GetComponent<SpriteRenderer>().sprite = mineClicked;
    }

    public void SetNotAMineFlagged()
    {
        GetComponent<SpriteRenderer>().sprite = flagNotAMineSprite;
    }

    private void OnMouseOver()
    {
        if (isCovered)
        {
            if (Input.GetMouseButtonUp(1))
            {
                if (tileState == TileState.Normal)
                {
                    tileState = TileState.Flagged;
                    GetComponent<SpriteRenderer>().sprite = flagSprite;
                }
                else
                {
                    tileState = TileState.Normal;
                    GetComponent<SpriteRenderer>().sprite = coveredSprite;
                }
            }
        }
    }
}

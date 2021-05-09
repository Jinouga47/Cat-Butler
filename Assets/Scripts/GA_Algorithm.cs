using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GA_Algorithm : MonoBehaviour
{
    public Tile tile;
    public Tilemap tilemap;
    public Vector3Int currentPos;
    public int pos_ = 0;
    public int limit_ = 0;
    private List<Block> blocks = new List<Block>();//List of block objects
    private List<List<Block>> rows = new List<List<Block>>();//List of block lists
    private List<List<List<Block>>> levels = new List<List<List<Block>>>();//List of row lists
    public float timer = 1f;
    // Start is called before the first frame update
    public struct Block
    {
        public int x, y;
    };
    void Start()
    {
        setBorder();
    }

    // Update is called once per frame
    void Update()
    {
        if(timer < 0)
        {
            if (levels.Count != 0) reset();
            //Clear the rows/blocks so new ones can be added
            while (limit_ < 8)
            //While the limit hasn't been reached...
            {
                if (pos_ < 9)
                //...if the current block isn't the 10th block to be placed in the row...
                {
                    placeObjects();
                    //...place the block

                    pos_++;
                    //Increment the pos_ variable

                    currentPos.x += 1;
                    //Increment the current x coordinate for the next placement
                }
                else
                //If the current block is the 10th block...
                {
                    if (rowCheck() && rowDupeCheck())
                    //...check the row to make sure it's not completely full of blocks or a duplicate
                    {
                        currentPos.y += 1;
                        //Increment to the next row

                        limit_++;
                        //Increment limit_
                        rows.Add(new List<Block>(blocks));
                        //Add the current row to the rows variable
                    }

                    currentPos.x = -4;
                    //Reset the x coordinate

                    pos_ = 0;
                    //Reset the pos_ variable

                    blocks.Clear();
                }
            }

            if (limit_ == 8)
            {
                if (levels.Count != 0) selectionCrossover();
                mutation();
                levels.Add(new List<List<Block>>(rows));
            }
            timer = 0.5f;
        }
        timer -= Time.deltaTime;


        //Basic idea
        //Run through each row and check to see if that space has block surrounding it. If the space has
        //no blocks surrounding it, place a block there. If the entire row ends up being blocks, redo the entire row.
        //If the next row has too many stacking blocks redo the entire row. This effectively makes two seperate
        //fitness functions, meaning that this needs to be outlined in the documentation.
        //The chances of a level being created that can't be completed using this method should be quite low but should
        //still be taken into account afterwards. Maybe running the program in the background with screen capture running
        //for a few minutes should help with finding out failure rates. This would be done by having the level generator cycle
        //on loop and capture the footage, examining it frame by frame to determine which levels can
        //be completed or not.
    }

    private void reset()
    {
        for (int i = 0; i < 8; i++)
        {
            for(int ii = 0; ii < 9; ii++)
            {
                tilemap.SetTile(new Vector3Int(ii - 4, i - 4, 0), null);
            }
        }
        setBorder();
        blocks.Clear();
        rows.Clear();
        pos_ = 0;
        limit_ = 0;
        currentPos.y = -4;
    }

    void setBorder()//Set the initial border of the level
    {
        currentPos = new Vector3Int(-5, -5, 0);
        //Bottom Border
        for (int i = 0; i < 10; i++) tilemap.SetTile(currentPos + new Vector3Int(i, 0, 0), tile);

        //Left Border
        for (int i = 0; i < 10; i++) tilemap.SetTile(currentPos + new Vector3Int(0, i, 0), tile);

        currentPos.x = 5;

        //Right Border
        for (int i = 0; i < 10; i++) tilemap.SetTile(currentPos + new Vector3Int(0, i, 0), tile);

        currentPos.y = 4;

        //Top Border
        for (int i = 0; i < 10; i++) tilemap.SetTile(currentPos - new Vector3Int(i, 0, 0), tile);

        currentPos = new Vector3Int(-4, -4, 0);
    }

    void addBlocks()
    {
        Block temp = new Block();
        List <Block> temp2 = new List<Block>();
        foreach (Block block in blocks)
        {
            temp.x = block.x;
            temp.y = block.y;
            temp2.Add(temp);
        }
        rows.Add(temp2);
    }

    void placeObjects()//Place the tiles on the tilemap
    {
        if (Random.Range(-1, 2) == 1)//If the random chance triggers...
        {
            tilemap.SetTile(currentPos, tile);//...set the tile in the current position.

            Block t = new Block();//Create new block object

            t.x = currentPos.x;//Assign 'x' coordinate

            t.y = currentPos.y;//Assign 'y' coordinate

            blocks.Add(t);//Add new block to list

            if (proximityCheck(0))//If the returned value from the called function makes the statement true...
            {
                tilemap.SetTile(currentPos, null);//Remove the tile from the grid

                blocks.RemoveAt(blocks.Count - 1);//Remove the block from the list
            }
            
        }
    }

    bool rowCheck()//Part of the fitness function
    {
        return blocks.Count != 9;
    }

    bool rowDupeCheck()//Part of the fitness function
    {
        int clones = 0;

        //Iterate through the list of rows
        for(int i = 0; i < rows.Count; i++)
        {

            //If the current row has the same amount of blocks as the iterated row...
            if (blocks.Count == rows[i].Count)
            {

                //Iterate through the current row.
                for (int ii = 0; ii < blocks.Count; ii++)
                {

                    //If the current selected block has the same x coordinate as the iterated block,
                    //increase clones by 1.
                    if (blocks[ii].x == rows[i][ii].x) clones++;
                }
            }

            //If clones is the same as the amount of blocks in the current row, return true.
            if (clones == blocks.Count) return false;
        }

        //Return true if there the row isn't a duplicate
        return true;
    }

    bool proximityCheck(int i)//Part of the fitness function
    {
        int x;
        int y;
        if (levels.Count == 0/*i == 0*/)
        {
            x = blocks[blocks.Count - 1].x;
            y = blocks[blocks.Count - 1].y;
        }
        else
        {
            x = blocks[i].x;
            y = blocks[i].y;
        }

        bool bottom_left, bottom_left_plus, down, down_plus;

        bottom_left = tilemap.GetTile(new Vector3Int(x - 1, y - 1, 0));
        bottom_left_plus = tilemap.GetTile(new Vector3Int(x - 2, y - 2, 0));
        down = tilemap.GetTile(new Vector3Int(x, y - 1, 0));

        down_plus = tilemap.GetTile(new Vector3Int(x, y - 2, 0));

        return down && down_plus;// && bottom_left && bottom_left_plus;

    }

    void mutation()//Random chance of triggering
    {
        if(Random.Range(0, 101) == 51)//If the random chance is triggered...
        {

            int rowCounter = Random.Range(0, rows.Count);//...get a random row from the entire list.

            int y = rows[rowCounter][0].y;//Set the 'y' coordinate of the selected row

            List<Block> row = new List<Block>(rows[rowCounter]);//Add the selected row to a variable to make it easier to access

            foreach (Block block in row) tilemap.SetTile(new Vector3Int(block.x, y, 0), null);//Remove all the placed blocks in that row from the scene

            for (int i = 0; i < 9; i++)//Iterate 9 times, one for every available space in the row
            {
                if(Random.Range(-1, 2) == 1)//If the random chance is triggered...
                {
                    tilemap.SetTile(new Vector3Int(i - 4, y, 0), tile);//...place a tile in the iterated space.

                    Block t = new Block();//Create a new block object

                    t.x = i - 4;//Assign 'x' coordinate

                    t.y = y;//Assign 'y' cooridinate

                    blocks.Add(t);//Add the block to the list
                }
            }
            rows[rowCounter] = new List<Block>(blocks);//Overwrite the old row list with the new row list
            
        }
    }

    void selectionCrossover()
    {
        List<List<Block>> level = new List<List<Block>>(levels[levels.Count - 1]);//Stores an entire level in a list. Essentially 'rows' with a different name

        for(int i = 0; i < rows.Count; i++)//Loop through the current levels rows
        {
            Debug.Log(rows[i].Count);
            if(Random.Range(-1, 2) == 1)//If the random chance is triggered...
            {
                blocks = new List<Block>(rows[i]);
                List<Block> oldSet = new List<Block>(blocks);//...store the set of blocks in a variable in case the row from the previous level is a duplicate
                
                blocks = new List<Block>(level[i]);//Sets the current set of blocks (current row) to the iterated row of the previous level
                
                if (!rowDupeCheck()) blocks = oldSet;//If the row from the previous level ends up being a duplicate, restore the original row
                
                int tt = blocks.Count;

                for(int ii = 0; ii < tt; ii++)//Loop through the current row
                {

                    if (proximityCheck(ii))//If the currently iterated block doesn't fit the criteria...
                    {
                        tilemap.SetTile(new Vector3Int(blocks[ii].x, blocks[ii].y, 0), null);//...remove the tile from the grid,
                        
                        blocks.RemoveAt(ii);//and remove the block from the list.
                    }
                }
                rows[i] = new List<Block>(blocks);//Overwrites the old row with the new row 
            }
        }
    }
}

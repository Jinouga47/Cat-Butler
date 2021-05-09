using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    GameObject player, mouseHole, gm, spawner;
    private Transform leftCheck, rightCheck, headCheck, floorCheck;
    public float checkRadius;
    public LayerMask boundary, playerMask, ground, mouseHoleMask;
    private bool leftMove, rightMove, leftBoundary, rightBoundary, hasKey, grounded, thief;
    public int proximity = 2;
    public SpriteRenderer foodSprite;

    public float deadCounter = 0.2f;
    public bool dead = false, flip, landed = false;

    float playerPos_x, enemyPos_x, playerPos_y, enemyPos_y, holePos_x, holePos_y, distance_x, distance_y, distance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Spawn(bool thief)
    {
        player = GameObject.Find("Player");
        mouseHole = GameObject.Find("MouseHole");
        gm = GameObject.Find("Main Camera");
        spawner = GameObject.Find("Spawner");
        this.thief = thief;

        leftCheck = transform.Find("LeftCheck");
        rightCheck = transform.Find("RightCheck");
        headCheck = transform.Find("HeadCheck");
        floorCheck = transform.Find("GroundCheck");

        foodSprite = transform.Find("Cake").GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!DeathCheck())
        {
            if (thief) Run();
            else LeftRightMove();

            Steal();
        }

        gameObject.GetComponent<SpriteRenderer>().flipX = flip;
        foodSprite.enabled = hasKey;

        if (dead && deadCounter < 0)
        {
            gm.GetComponent<GameManager>().score++;
            spawner.GetComponent<EnemySpawner>().enemyCount--;
            Destroy(gameObject);
        }
    }

    void Run()
    {
        playerPos_x = player.transform.position.x;
        enemyPos_x = transform.position.x;
        playerPos_y = player.transform.position.y;
        enemyPos_y = transform.position.y;
        holePos_x = mouseHole.transform.position.x;
        holePos_y = mouseHole.transform.position.y;

        grounded = Physics2D.OverlapCircle(floorCheck.position, checkRadius, ground);
        leftBoundary = Physics2D.OverlapCircle(leftCheck.position, checkRadius, boundary);
        rightBoundary = Physics2D.OverlapCircle(rightCheck.position, checkRadius, boundary);
        if (grounded)
        {
            distance_x = enemyPos_x - playerPos_x < 0 ? (enemyPos_x - playerPos_x) * -1 : enemyPos_x - playerPos_x;

            distance_y = enemyPos_y - playerPos_y < 0 ? (enemyPos_y - playerPos_y) * -1 : enemyPos_y - playerPos_y;

            distance = Mathf.Sqrt(Mathf.Pow(distance_x, 2) + Mathf.Pow(distance_y, 2));

            if (leftBoundary) gameObject.transform.position += new Vector3(0.01f, 0, 0);
            else if (rightBoundary) gameObject.transform.position -= new Vector3(0.01f, 0, 0);
            if (hasKey)
            {
                if(WallCheck() == (int)Direction.Neutral)
                {
                    if (enemyPos_x > holePos_x)
                    {
                        flip = false;
                        gameObject.transform.position -= new Vector3(1, 0, 0) * Time.deltaTime * 4;
                    }
                    //else if (playerPos_x > gameObject.transform.position.x)
                    //{
                    //    flip = true;
                    //    gameObject.transform.position -= new Vector3(1, 0, 0) * Time.deltaTime * 4;
                    //}
                    //if (enemyPos_y < holePos_y + 1 && enemyPos_y > holePos_y - 1 && WallCheck() == (int)Direction.Neutral)
                    //{
                    //    flip = true;
                    //    gameObject.transform.position += new Vector3(1, 0, 0) * Time.deltaTime * 4;
                    //}
                    //else
                    //{
                    //    flip = false;
                    //    gameObject.transform.position -= new Vector3(1, 0, 0) * Time.deltaTime * 4;
                    //}
                    else
                    {
                        flip = true;
                        gameObject.transform.position += new Vector3(1, 0, 0) * Time.deltaTime * 4;
                    }
                }
            }
            else
            {
                if (distance < proximity && !leftBoundary && !rightBoundary)
                {
                    if (enemyPos_x > playerPos_x)
                    {
                        if (!hasKey)
                        {
                            flip = false;
                            gameObject.transform.position -= new Vector3(1, 0, 0) * Time.deltaTime * 4;
                        }
                        else if (WallCheck() == (int)Direction.Neutral)
                        {
                            flip = true;
                            gameObject.transform.position += new Vector3(1, 0, 0) * Time.deltaTime * 4;
                        }
                    }
                    else
                    {
                        if (!hasKey)
                        {
                            flip = true;
                            gameObject.transform.position += new Vector3(1, 0, 0) * Time.deltaTime * 4;
                        }
                        else if (WallCheck() == (int)Direction.Neutral)
                        {
                            flip = false;
                            gameObject.transform.position -= new Vector3(1, 0, 0) * Time.deltaTime * 4;
                        }
                    }
                }
            }
        }
        else
        {
            if(!(rightBoundary || leftBoundary))
            {
                if (flip) gameObject.transform.position += new Vector3(1, 0, 0) * Time.deltaTime * 1;
                else gameObject.transform.position -= new Vector3(1, 0, 0) * Time.deltaTime * 1;
            }
            gameObject.transform.position -= new Vector3(0, 1, 0) * Time.deltaTime * 4;
        }
    }

    void LeftRightMove()
    {
        leftBoundary = Physics2D.OverlapCircle(leftCheck.position, checkRadius, boundary);
        rightBoundary = Physics2D.OverlapCircle(rightCheck.position, checkRadius, boundary);
        grounded = Physics2D.OverlapCircle(floorCheck.position, checkRadius, ground);
        if (leftBoundary)
        {
            leftMove = false;
            rightMove = true;
        }
        else if (rightBoundary)
        {
            leftMove = true;
            rightMove = false;
        }

        if (grounded)
        {
            landed = true;
            if (!leftMove && !rightMove)
            {
                gameObject.transform.position -= new Vector3(1, 0, 0) * Time.deltaTime * 4;
            }

            if (leftMove)
            {
                flip = false;
                gameObject.transform.position -= new Vector3(1, 0, 0) * Time.deltaTime * 4;
            }
            if (rightMove)
            {
                flip = true;
                gameObject.transform.position += new Vector3(1, 0, 0) * Time.deltaTime * 4;
            }
        }
        else
        {
            if (leftMove || (!leftMove && !rightMove))
            {
                leftMove = false;
                rightMove = true;
                gameObject.transform.position += new Vector3(0.1f, 0, 0);
            }
            else if (rightMove)
            {
                leftMove = true;
                rightMove = false;
                gameObject.transform.position -= new Vector3(0.1f, 0, 0);
            }
            if (!landed) gameObject.transform.position -= new Vector3(0, 0.1f, 0);
        }

    }

    void Steal()
    {
        leftBoundary = Physics2D.OverlapCircle(leftCheck.position, checkRadius, playerMask);
        rightBoundary = Physics2D.OverlapCircle(rightCheck.position, checkRadius, playerMask);
        if (leftBoundary || rightBoundary)
        {
            if (player.GetComponent<PlayerControl>().hasKey && thief)
            {
                hasKey = true;
                player.GetComponent<PlayerControl>().hasKey = false;
                player.GetComponent<PlayerControl>().KnockBack(playerPos_x > enemyPos_x ? 1 : -1);
                gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 120, 120);
            }
        }
        if (Physics2D.OverlapCircle(transform.position, checkRadius, mouseHoleMask) && hasKey)
        {
            gm.GetComponent<GameManager>().ActivateKey();
            spawner.GetComponent<EnemySpawner>().enemyCount--;
            Destroy(gameObject);
        }
    }

    bool DeathCheck()
    {
        if (Physics2D.OverlapCircle(headCheck.position, checkRadius, playerMask))
        {
            dead = true;
            if (hasKey) player.GetComponent<PlayerControl>().hasKey = true;
            return true;
        }

        if (dead && deadCounter > 0) deadCounter -= Time.deltaTime;
        return dead;
    }

    int WallCheck()
    {
        bool leftBoundary, rightBoundary;
        leftBoundary = Physics2D.OverlapCircle(leftCheck.position, checkRadius, boundary);
        rightBoundary = Physics2D.OverlapCircle(rightCheck.position, checkRadius, boundary);

        leftMove = rightBoundary;
        rightMove = leftBoundary;

        if (leftBoundary) return (int)Direction.Right;
        else if (rightBoundary) return (int)Direction.Left;
        else return (int)Direction.Neutral;
    }

    public enum Direction
    {
        Left,
        Right,
        Neutral
    }
}


using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
    public float speed = 5;
    public float jumpSpeed = 12;
    private Rigidbody2D rigidbody2D;
    public SpriteRenderer foodSprite;

    private bool grounded, enemyHead;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask ground, enemy;

    private float jumpTimer, stunTimer = 0.5f;
    public float airtime, goombaJumpInputWindowTimer = 0.5f, goombaJumpInputWindow;
    private bool airborne, flip = true, goombaJump = false, windowClose = true, stunned = false;

    public bool hasKey;

    // Start is called before the first frame update
    void Start()
    {
        foodSprite = GameObject.Find("Cake").GetComponent<SpriteRenderer>();
    }

    private void Awake()
    {
        rigidbody2D = transform.GetComponent<Rigidbody2D>();
        rigidbody2D.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, ground);
        enemyHead = Physics2D.OverlapCircle(groundCheck.position, checkRadius, enemy);
        foodSprite.enabled = hasKey;
        if (stunned)
        {
            if (stunTimer > 0)
            {
                stunTimer -= Time.deltaTime;
                gameObject.layer = LayerMask.NameToLayer("Enemy");

            }
            else
            {
                stunTimer = 0.5f;
                stunned = false;
                gameObject.layer = LayerMask.NameToLayer("Player");
            }
        }
        else
        {
            if (((Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && grounded))
            {
                airborne = true;
                goombaJump = false;
                jumpTimer = airtime;
                rigidbody2D.velocity = Vector2.up * jumpSpeed;
            }

            if (enemyHead)
            {
                airborne = true;
                goombaJump = true;
                jumpTimer = airtime;
                goombaJumpInputWindow = goombaJumpInputWindowTimer;
                rigidbody2D.velocity = Vector2.up * jumpSpeed;
            }
            InputWindow();
            if ((Input.GetButton("Jump") || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && airborne && !goombaJump)
            {
                if (jumpTimer > 0)
                {
                    rigidbody2D.velocity = Vector2.up * jumpSpeed;
                    jumpTimer -= Time.deltaTime;
                }
                else
                {
                    airborne = false;
                    goombaJump = false;
                }
            }
            else if ((Input.GetButton("Jump") || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && airborne && !windowClose)
            {
                if (jumpTimer > 0)
                {
                    rigidbody2D.velocity = Vector2.up * jumpSpeed;
                    jumpTimer -= Time.deltaTime;
                }
                else
                {
                    airborne = false;
                    goombaJump = false;
                }
            }

            if ((Input.GetButtonUp("Jump") || Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W)))
            {
                airborne = false;
            }

            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) flip = true;
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) flip = false;
            gameObject.GetComponent<SpriteRenderer>().flipX = flip;
        }
    }

    private void FixedUpdate()
    {
        if (!stunned)
        {
            var movement = Input.GetAxisRaw("Horizontal");

            rigidbody2D.transform.position += new Vector3(movement, 0, 0) * Time.deltaTime * speed;
        }
    }

    private void InputWindow()
    {
        if (goombaJump)
        {
            if (goombaJumpInputWindow > 0)
            {
                goombaJumpInputWindow -= Time.deltaTime;
                if(Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
                    windowClose = false;
            }
            else
            {
                airborne = false;
                goombaJump = false;
                windowClose = true;
            }
        }
    }

    public void KnockBack(int dir)
    {
        stunned = true;
        rigidbody2D.transform.position += new Vector3(dir/2, 0.5f, 0);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.name == "Key" || other.name == "Key 1") && !hasKey && other.GetComponent<KeyScript>().active)
        {
            hasKey = true;
            other.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
            other.GetComponent<KeyScript>().active = false;
        }
    }
}

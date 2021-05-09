using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorScript : MonoBehaviour
{
    public Transform playerCheck;
    public float checkRadius;
    public LayerMask playerMask;
    public GameObject player;
    public bool open;
    public bool winState;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics2D.OverlapCircle(playerCheck.position, checkRadius, playerMask) && open)
        {
            winState = true;
        }

    }

    
}

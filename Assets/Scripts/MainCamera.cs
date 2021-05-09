using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        //gameObject.transform.position = new Vector3(0,0,15);
    }

    // Update is called once per frame
    void Update()
    {
        //gameObject.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -15);
    }
}

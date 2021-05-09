using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    private Color baseColour;
    public GameObject player, gm;
    public bool active = true;
    // Start is called before the first frame update
    void Start()
    {
        baseColour = GetComponent<SpriteRenderer>().color;
        gm = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        if (active) GetComponent<SpriteRenderer>().color = baseColour;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player" && active)
        {
            //GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
            //active = false;
        }
    }
}

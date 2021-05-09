using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextScene()
    {
        switch(SceneManager.GetActiveScene().name)
        {
            case "Level 1":
                SceneManager.LoadScene("Level 2");
                break;
            case "Level 2":
                SceneManager.LoadScene("Level 3");
                break;
            case "Level 3":
                SceneManager.LoadScene("Level 4");
                break;
            case "Level 4":
                SceneManager.LoadScene("Level 5");
                break;
            case "Main Menu":
                SceneManager.LoadScene("Level 1");
                break;
        }
    }
}
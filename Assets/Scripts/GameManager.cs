using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject key, player, door;
    public Text objective, scoreText;
    private SpriteRenderer renderer;
    public int score;
    public bool stolen;
    public Vector3 keyPos;
    public Canvas play, win;
    public Sprite win1, win2, win3;
    public Image stars;
    // Start is called before the first frame update
    void Start()
    {
        win.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<PlayerControl>().hasKey)
        {
            objective.text = "Current Objective: Get To The Door!";
            door.GetComponent<DoorScript>().open = true;
        }
        else
        {
            objective.text = "Current Objective: Get The Food!";
            door.GetComponent<DoorScript>().open = false;
        }

        scoreText.text = score.ToString();

        if (door.GetComponent<DoorScript>().winState == true)
        {
            play.gameObject.SetActive(false);
            win.gameObject.SetActive(true);


            if (score < 5)
                stars.sprite = win1;
            else if (score >= 5 && score <= 15)
                stars.sprite = win2;
            else if (score > 15)
                stars.sprite = win3;

        }
        else
        {
            play.gameObject.SetActive(true);
            win.gameObject.SetActive(false);
        }
    }

    public void ActivateKey()
    {
        key.GetComponent<KeyScript>().active = true;
    }

    public void Scenes()
    {
        switch (SceneManager.GetActiveScene().name)
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

    public void QuitGame()
    {
        Application.Quit();
    }
}
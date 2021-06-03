using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public Text title;
    private int titleCount;
    private bool adding;

    //the drawing animations
    public GameObject drawing1;
    public GameObject drawing2;
    public GameObject drawing3;

    // Start is called before the first frame update
    void Start()
    {
        titleCount = 0;
        adding = true;
        //InvokeRepeating("UpdateText", 0f, 0.3f);
        //decides which drawing animation

        int rand = Random.Range(0, 3);
        if(rand == 0)
        {
            drawing1.SetActive(true);
        } else if(rand == 1)
        {
            drawing2.SetActive(true);

        }
        else
        {
            drawing3.SetActive(true);
       
        }

        //now sets up player prefs

        if(!PlayerPrefs.HasKey("highscore"))
        {
            PlayerPrefs.SetInt("highscore", 0);
            PlayerPrefs.SetString("pencil", "255,0,0");

        }
    }

    private void UpdateText()
    {
        if (adding) {
            titleCount++;
        } else
        {
            titleCount--;
        }

        string text = "SMARTDRAW";

        if(titleCount <= 5)
        {
            title.text = text.Substring(0, titleCount); 
        } else
        {
            title.text = text.Substring(0, 5) + "\n" + text.Substring(5, titleCount - 5);
        }

        if(titleCount == 9)
        {
            adding = false;
        } else if(titleCount == 0)
        {
            adding = true;
        }

    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Help()
    {
        SceneManager.LoadScene(2);
    }

}

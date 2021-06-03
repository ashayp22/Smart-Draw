using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HelpManager : MonoBehaviour
{

    public Text statsText;
    private Vector2 firstPressPos;
    private Vector2 currentSwipe;

    public Slider red;
    public Slider green;
    public Slider blue;

    public Button colorButton;

    // Start is called before the first frame update
    void Start()
    {
        statsText.text = "STATS\n\n" + "Best Score: " + PlayerPrefs.GetInt("highscore");

        //sets sliders to pencil color value

        string color = PlayerPrefs.GetString("pencil");

        int r = int.Parse(color.Substring(0, color.IndexOf(",")));

        color = color.Substring(color.IndexOf(",") + 1);

        int g = int.Parse(color.Substring(0, color.IndexOf(",")));
        color = color.Substring(color.IndexOf(",") + 1);

        int b = int.Parse(color);

        red.value = r;
        green.value = g;
        blue.value = b;

        setColorButton(r, g, b);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) //tapped
        {
            firstPressPos = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            currentSwipe = (Vector2)Input.mousePosition - firstPressPos;
            if (currentSwipe.y >= 300)//swiped up
            {
                SceneManager.LoadScene(0);
            }
            
        }
    }

    public void UpdatePencilColor()
    {
        PlayerPrefs.SetString("pencil", (int)red.value + "," + (int)green.value + "," + (int)blue.value);
        setColorButton((int)red.value, (int)green.value, (int)blue.value);
    }

    public void setColorButton(int r, int g, int b)
    {
        colorButton.GetComponent<Image>().color = new Color32((byte)r, (byte)g, (byte)b, 255);
    }

}

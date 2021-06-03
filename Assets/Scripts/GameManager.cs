using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FreeDraw;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public Drawable drawable; //drawable
    private CSVParsing thetaParser; //parsing the thetas

    public GameObject canvas; //canvas, but gameobject
    public GameObject startPanel; //panel for the start with 321 go
    public Text panelText; //the text 
    public Text drawingText;
    public Text scoreText;

    private int timer; //timer
    private bool gamestart; //game start or not
    //possibilities for drawings
    private int drawing1;
    private int drawing2;
    private int drawing3;


    //for detecting swipes
    private Vector2 firstPressPos;
    private Vector2 currentSwipe;
    private bool pressed;

    private int score;

    public DrawingSettings drawingSettings;

    public AudioSource startingSound;

    // Start is called before the first frame update
    void Start()
    {

        //sets the pen

        string color = PlayerPrefs.GetString("pencil");

        int r = int.Parse(color.Substring(0, color.IndexOf(",")));

        color = color.Substring(color.IndexOf(",") + 1);

        int g = int.Parse(color.Substring(0, color.IndexOf(",")));
        color = color.Substring(color.IndexOf(",") + 1);

        int b = int.Parse(color);

        drawingSettings.penColor = new Color32((byte)r, (byte)g, (byte)b, 255);
        //drawingSettings.SetMarkerColor();

        //does rest
        thetaParser = new CSVParsing();
        timer = 0;
        gamestart = false;
        score = 0;
        drawing1 = Random.Range(0, 29);
        pressed = false;
        do
        {
            drawing2 = Random.Range(0, 30);
        } while (drawing1 == drawing2);

        do
        {
            drawing3 = Random.Range(0, 30);
        } while (drawing1 == drawing3 || drawing2 == drawing3);


        //now put on screen

        drawingText.text = "DRAW ONE OF\nTHE FOLLOWING\n\n" + GameManager.getDrawingName(drawing1) + "\n" + GameManager.getDrawingName(drawing2) + "\n" + GameManager.getDrawingName(drawing3);
        InvokeRepeating("UpdateTimer", 0f, 1f);
        InvokeRepeating("SetPen", 4.05f, 1f);
    }

    private void SetPen()
    {
        drawingSettings.SetMarkerColor();
        CancelInvoke("SetPen");
    }

    //updates the timer

    private void UpdateTimer()
    {
        timer += 1;

        //plays audio

        if(timer <= 3)
        {
            startingSound.Play();
            Debug.Log("Ye");
        }
    }

    // Update is called once per frame
    void Update()
    {

        if(!gamestart) //game isn't going on
        {
            if (timer <= 3) //show 3 2 1 on screen
            {
                panelText.text = (4 - timer) + "";
            }
            else if (timer == 4) //start the game
            {
                panelText.text = "GO";
            } else if(timer == 5)
            {
                startPanel.SetActive(false);
                canvas.SetActive(true);
                gamestart = true;
            }
            else //show the results
            {
                Debug.Log("here");
                //detect swipe
                if (Input.GetMouseButtonDown(0)) //tapped
                {
                    firstPressPos = Input.mousePosition;
                    pressed = true;
                    Debug.Log("collected--------------------------");
                }
                if (Input.GetMouseButtonUp(0) && pressed)
                {
                    currentSwipe = (Vector2)Input.mousePosition - firstPressPos;
                    if (currentSwipe.y >= 10) //swipped up
                    {
                        Debug.Log("correct");
                        score++;

                        //updates high score
                        if(PlayerPrefs.GetInt("highscore") < score)
                        {
                            PlayerPrefs.SetInt("highscore", score);
                        }

                        restart();
                    }
                    else if(currentSwipe.y <= -10)//swipped down
                    {
                        Debug.Log("wrong");
                        //updates high score
                        if (PlayerPrefs.GetInt("highscore") < score)
                        {
                            PlayerPrefs.SetInt("highscore", score);
                        }

                        score = 0;
                        restart();
                    }
                }


            }
        } else //game is going on
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                checkGuess();
            }
        }
     
    }

    public void done()
    {
        if(gamestart && timer > 10 && drawable.getColored() && !drawable.isClearCanvas())
        {
            checkGuess();
            Debug.Log("done checking guess");
        }  
    }

    private void checkGuess()
    {
        //guesses
        List<double> screen = drawable.getPixels();

        //checks to see if the prediction matches any of the possible drawings

        string matched = "none";

        //1

        int group = (drawing1 / 5) + 1;
        int num = drawing1 % 5;

        int guess = thetaParser.getPrediction(copyDoubleList(screen), group);

        if (num == guess)
        {
            matched = GameManager.getDrawingName(drawing1);
        }

        //2
        group = (drawing2 / 5) + 1;
        num = drawing2 % 5;

        guess = thetaParser.getPrediction(copyDoubleList(screen), group);

        if (num == guess)
        {
            matched = GameManager.getDrawingName(drawing2);
        }
        //3
        group = (drawing3 / 5) + 1;
        num = drawing3 % 5;

        guess = thetaParser.getPrediction(copyDoubleList(screen), group);

        if (num == guess)
        {
            matched = GameManager.getDrawingName(drawing3);
        }


        gamestart = false;

        if (matched.Equals("none"))
        {
            drawingText.text = "I PREDICT\nNOTHING\nSWIPE DOWN\nTO PLAY AGAIN";
        }
        else
        {
            drawingText.text = "I PREDICT\n" + matched + "\nSWIPE UP\nIF CORRECT\nELSE SWIPE DOWN";
        }
    }
     
    private void restart() //restarts the screen
    {
        scoreText.text = "Score: " + score;
        drawing1 = Random.Range(0, 29);
        pressed = false;
        do
        {
            drawing2 = Random.Range(0, 29);
        } while (drawing1 == drawing2);

        do
        {
            drawing3 = Random.Range(0, 29);
        } while (drawing1 == drawing3 || drawing2 == drawing3);


        //now put on screen

        drawingText.text = "DRAW ONE OF\nTHE FOLLOWING\n\n" + GameManager.getDrawingName(drawing1) + "\n" + GameManager.getDrawingName(drawing2) + "\n" + GameManager.getDrawingName(drawing3);

        timer = 5;
        gamestart = true;

        //clears the board
        drawable.clearCanvas();

        drawable.resetColored();

    }

    private List<double> copyDoubleList(List<double> arr)
    {
        List<double> copy = new List<double>();
        foreach(double d in arr)
        {
            copy.Add(d);
        }
        return copy;
    } 

    public static string getDrawingName(int i)
    {
        switch(i)
        {
            case 0:
                return "basketball";
            case 1:
                return "car";
            case 2:
                return "flower";
            case 3:
                return "pencil";
            case 4:
                return "smiley face";
            case 5:
                return "rainbow";
            case 6:
                return "skull";
            case 7:
                return "star";
            case 8:
                return "triangle";
            case 9:
                return "violin";
            case 10:
                return "house";
            case 11:
                return "mountain";
            case 12:
                return "pants";
            case 13:
                return "potato";
            case 14:
                return "square";
            case 15:
                return "book";
            case 16:
                return "calculator";
            case 17:
                return "cloud";
            case 18:
                return "eye";
            case 19:
                return "hammer";
            case 20:
                return "palm tree";
            case 21:
                return "pig";
            case 22:
                return "pizza";
            case 23:
                return "shark";
            case 24:
                return "tornado";
            case 25:
                return "knife";
            case 26:
                return "sailboat";
            case 27:
                return "snowman";
            case 28:
                return "sun";
            case 29:
                return "tree";
            default:
                return "";
        }
    }

    public void GoHome()
    {
        SceneManager.LoadScene(0);
    }

    //how does the game work

    //add - drawings from same group
    //- menu
    //- go back to menu
    //change color of pen
    //stats like longest good streak, longest bad streak, common drawing

}

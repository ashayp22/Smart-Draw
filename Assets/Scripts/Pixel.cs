using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pixel : MonoBehaviour
{

    private bool isClicked;
    public int row;
    public int column;

    // Start is called before the first frame update
    void Start()
    {
        isClicked = false;
    }

    public bool getClicked()
    {
        return isClicked;
    }

    public void Clear()
    {
        isClicked = false;
        this.gameObject.GetComponent<Renderer>().material.color = Color.white;
    }

    void OnMouseOver()
    {
        //if(mouseClicked)
        // {
        // Debug.Log(this.name);
        // this.gameObject.GetComponent<Renderer>().material.color = Color.black;

        //}
    }

    public void Fill()
    {
        this.gameObject.GetComponent<Renderer>().material.color = Color.black;
        isClicked = true;
    }
}

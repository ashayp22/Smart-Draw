using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CSVParsing
{

    //all the theta

    private List<List<double>> Theta1Grad1 = new List<List<double>>();
    private List<List<double>> Theta2Grad1 = new List<List<double>>();
    private List<List<double>> Theta1Grad2 = new List<List<double>>();
    private List<List<double>> Theta2Grad2 = new List<List<double>>();
    private List<List<double>> Theta1Grad3 = new List<List<double>>();
    private List<List<double>> Theta2Grad3 = new List<List<double>>();
    private List<List<double>> Theta1Grad4 = new List<List<double>>();
    private List<List<double>> Theta2Grad4 = new List<List<double>>();
    private List<List<double>> Theta1Grad5 = new List<List<double>>();
    private List<List<double>> Theta2Grad5 = new List<List<double>>();
    private List<List<double>> Theta1Grad6 = new List<List<double>>();
    private List<List<double>> Theta2Grad6 = new List<List<double>>();


    //does all the parsing and predicting

    public CSVParsing()
    {
        //now load all the data from the training
        Theta1Grad1 = this.readCSV("Assets/Data/Theta1Grad1.csv");
        Theta2Grad1 = this.readCSV("Assets/Data/Theta2Grad1.csv");

        Theta1Grad2 = this.readCSV("Assets/Data/Theta1Grad2.csv");
        Theta2Grad2 = this.readCSV("Assets/Data/Theta2Grad2.csv");

        Theta1Grad3 = this.readCSV("Assets/Data/Theta1Grad3.csv");
        Theta2Grad3 = this.readCSV("Assets/Data/Theta2Grad3.csv");

        Theta1Grad4 = this.readCSV("Assets/Data/Theta1Grad4.csv");
        Theta2Grad4 = this.readCSV("Assets/Data/Theta2Grad4.csv");

        Theta1Grad5 = this.readCSV("Assets/Data/Theta1Grad5.csv");
        Theta2Grad5 = this.readCSV("Assets/Data/Theta2Grad5.csv");

        Theta1Grad6 = this.readCSV("Assets/Data/Theta1Grad6.csv");
        Theta2Grad6 = this.readCSV("Assets/Data/Theta2Grad6.csv");
    }

    private List<List<double>> readCSV(string path)
    {
        StreamReader strReader = new StreamReader(path);
        bool endoffile = false;

        List<List<double>> theta = new List<List<double>>();

        while(!endoffile)
        {
            string data_string = strReader.ReadLine();
            if(data_string == null)
            {
                endoffile = true;
                break;
            }
            //storing to a variable
            string[] data_values = data_string.Split(',');

            if(data_values.Length == 1) //is an empty line 
            {
                continue;
            }

            List<double> t = new List<double>();

            foreach(string s in data_values)
            {
                t.Add(double.Parse(s));
            }

            theta.Add(t);

        }
        return theta;

    }


    private double Sigmoid(double x)
    {
        return 1 / (1 + Mathf.Exp((float)(-x)));
    }

    public int getPrediction(List<double> image, int i)
    {

        int max = -1;

        if(i == 1)
        {
            max = predictImage(image, Theta1Grad1, Theta2Grad1);
        } else if(i == 2)
        {
            max = predictImage(image, Theta1Grad2, Theta2Grad2);
        }
        else if (i == 3)
        {
            max = predictImage(image, Theta1Grad3, Theta2Grad3);
        }
        else if (i == 4)
        {
            max = predictImage(image, Theta1Grad4, Theta2Grad4);
        }
        else if (i == 5)
        {
            max = predictImage(image, Theta1Grad5, Theta2Grad5);
        }
        else if (i == 6)
        {
            max = predictImage(image, Theta1Grad6, Theta2Grad6);
        }

        return max;
    }

    //image prediction code

    private int predictImage(List<double> image, List<List<double>> t1, List<List<double>> t2)
    {
        //sizes: image is 785 
        //t1 is 250 * 785
        //t2 is 5 * 250

        image.Insert(0, 1); //add bias
        //get next layer

        List<double> layer2 = new List<double>();  //size is 250

        for (int i = 0; i < t1.Count; i++)
        {
            double sum = 0;
            for (int j = 0; j < image.Count; j++)
            {
                sum += image[j] * t1[i][j];
            }
            layer2.Add(Sigmoid(sum)); //can't forget the sigmoid
        }


        //add bias again

        layer2.Insert(0, 1);

        //now get the hypothesis
        List<double> hypothesis = new List<double>();

        for (int i = 0; i < t2.Count; i++)
        {
            double sum = 0;
            for (int j = 0; j < layer2.Count; j++)
            {
                sum += layer2[j] * t2[i][j];
            }
            hypothesis.Add(Sigmoid(sum));
        }

        //Debug.Log(hypothesis.Count);

        //gets the max d

        double maxD = hypothesis[0];
        int maxIndex = 0;

        for(int i = 1; i < hypothesis.Count; i++)
        {
            if(hypothesis[i] > maxD)
            {
                maxD = hypothesis[i];
                maxIndex = i;
            }
        }

        foreach (double d in hypothesis)
        {
            //Debug.Log(d);
        }

        return maxIndex;
    }

}

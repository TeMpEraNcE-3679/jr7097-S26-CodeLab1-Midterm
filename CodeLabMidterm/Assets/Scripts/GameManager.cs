using UnityEngine;
using System.IO;
using System;
using TMPro;
using System.Collections.Generic;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    //SET UP THE BUTTON TO START THE GAME. 
    
    private bool StartedGame = false;
    
    // SET UP VARIABLES LIKE NUMBER, DIVISOR, SUCCESS TIMES

    private int divisor = 3;
    
    //CONNECT THE TEXTMESHPRO WITH THE VARIABLES: already set in get/set
    public int Divisor
    {
        get
        {
            return divisor;
        }
        set
        {
            divisor = value;
            DivisorText.text = divisor.ToString();
        }
    }

    private int number = 0;

    public int Number
    {
        get
        {
            return number;
        }
        set
        {
            number = value;
            NumberText.text = number.ToString();
        }
    }

    private int successTimes = 0;

    public int SuccessTimes
    {
        get
        {
            return successTimes;
        }
        set
        {
            successTimes = value;
            SuccessTimesText.text = successTimes.ToString();
        }
    }

    List<int> highestSuccessTimes;

    
    //bool to know whether player clicked
    [SerializeField]private bool Clicked = false;
    //bool to know whether player should click
    [SerializeField]private bool NeedToClick = false;
    //bool to know whether game is ended
    [SerializeField]private bool GameOver = false;
    
    public GameObject TextPrefab;
    
    public TMP_Text NumberText;
    public TMP_Text DivisorText;
    public TMP_Text SuccessTimesText;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("ChangeDivisor", 1f, 30f);
        InvokeRepeating("ChangeNumber", 1f, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!StartedGame) return;
        
        NeedClick();

        if (GameOver)
        {
            return;
        }
        else
        {
            HighestSuccess();
        }
    }

    public void StartGame()
    {
        StartedGame = true;
    }

    //SET UP GET SET OF DIVISOR AND NUMBER AND THE LOGIC OF HOW IT MAY CHANGE OVER A SET PERIOD OF TIME
    void ChangeDivisor()
    {
        Debug.Log("Change Divisor");
        int[] numbers = { 3, 5, 7, 11 };
        int NewDivisor;

        do
        {
            NewDivisor = numbers[UnityEngine.Random.Range(0, numbers.Length)];
        } 
        while (NewDivisor == Divisor);

        Divisor = NewDivisor;

    }

    void ChangeNumber()
    {
        Debug.Log("Change Number");
        int NewNumber;

        do
        {
            NewNumber = UnityEngine.Random.Range(0, 100);
        } 
        while (NewNumber == Number);
        
        Number = NewNumber;
        
    }
    
    //WRITE THE FUNCTION OF PLAYERS INTERACTION
    public void Click()
    //set up the button to Onclick run start  or click for these two button organized functions
    {
        Debug.Log("Clicked");
        Clicked = true;
        SuccessOrLose();
    }
    
    //DETERMINE WHEN THE PLAYER SHOULD INTERACT
    void NeedClick()
    {
        if (Number % Divisor == 0)
        {
            NeedToClick = true;
        }
    }
    //IF PLAYER DIDN'T INTERACT WHEN IT NEEDS TO BE, THEY LOSE, SET UP HOW SUCCESS TIMES WOULD CHANGE
    void SuccessOrLose()
    {
        if (Clicked == true && NeedToClick == true)
        {
            SuccessTimes++;
        }
        else
        {
            GameOver = true;
        }
    }
    //TODO: Set up GET SET FOR HIGHEST SUCCESS TIMES AND A LIST OF TOP SUCCESS
    void HighestSuccess()
    {
        //the file path to where the highest success be store.
        string filePath = Application.dataPath + "/Resources/HighestSuccess.txt";

        if (highestSuccessTimes == null)
        {
            highestSuccessTimes = new List<int>();

            if (!File.Exists(filePath))
            {
                for (int i = 0; i < 5; i++)
                {
                    highestSuccessTimes.Add(i * 1);
                }
            }
            else
            {
                string rank = File.ReadAllText(filePath);
                
                string[] ranksArray = rank.Split(",");

                for (int i = 0; i < ranksArray.Length; i++)
                {
                    Debug.Log("Parse: " +  ranksArray[i]);
                    
                    //EVERYTIME GAME RUNS PRINT THE NEW LIST
                    Instantiate(TextPrefab, new Vector2(-580, 400-200*i), Quaternion.identity);
                    
                    TMP_Text tmp = TextPrefab.GetComponent<TMP_Text>();
                    
                    tmp.text = ranksArray[i];
                    
                    Debug.Log("GENERATE"+i);

                    try
                    {
                        highestSuccessTimes.Add(int.Parse(ranksArray[i]));
                    }
                    catch (FormatException fe)
                    {
                        Debug.Log(fe.Message);
                        Debug.Log(fe.StackTrace);
                    }
                }
            }
        }
        
        //INSERT NEW HIGHEST SUCCESS TIMES TO THE LIST AND PRINT LIST
        for (int i = 0; i<highestSuccessTimes.Count; i++)
        {
            if (highestSuccessTimes[i] >= successTimes)
            {
                highestSuccessTimes.Insert(i,successTimes);
                break;
            }

        }
        
        highestSuccessTimes.RemoveAt(0);

        string fileContents = "";

        for (int i = 0; i < highestSuccessTimes.Count; i++)
        {
            fileContents += highestSuccessTimes[i]+ ", ";
        }
        
        File.WriteAllText(filePath, fileContents);
    }
    
    
    
    
}

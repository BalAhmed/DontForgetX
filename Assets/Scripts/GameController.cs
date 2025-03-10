using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public TMP_Text displayTextTmp;
    private string number1 = ""; // Girilen say�y� saklayan de�i�ken
    public int enteredNumber;
    public int rNumber;
    public int trueAnswer;
    public int xValue;
    public int process = 4;
    public TMP_Text processTextTMP;
    public TMP_Text scoreTextTMP;
    public static int Score;
    public static int highScore;
    public TMP_Text topScoreTMP;
    public GameObject FinishScreen;
    public float timeRemaining = 10;
    public TMP_Text timeTextTMP;
    public TMP_Text finishScoreTextTMP;
    //public Text finishHighScoreText;
    public TMP_Text finishHighScoreTextTMP;
    public GameObject mainScreen;
    public GameObject[] hearths;
    int health = 3;
   

    
    

    private void Start()
    {
        rNumber = Random.Range(1, 5);
        int rNumber2 = Random.Range(1, 5);
        processTextTMP.text = rNumber + " + " + rNumber2 + " = X";
        trueAnswer = rNumber + rNumber2;
        if(enteredNumber == trueAnswer)
        {
            xValue = trueAnswer;
            Debug.Log(xValue);
        }

        highScore = PlayerPrefs.GetInt("highScore", 0);
        topScoreTMP.text = "HighScore: " + highScore;
    }

    private void Update()
    {
        if(timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            timeTextTMP.text = timeRemaining.ToString("F0");
        }
        else
        {
            health--;
            UptadeHearthUI();
            timeRemaining = 10;
            if(health == 0)
            {
                FinisGame();  
            }
        }
    }

    public void CreatingQuestion()
    {
        if(xValue > 75)
        {
            process = Random.Range(3, 5);
        }
        else
        {
            process = Random.Range(1, 5);
        }
        switch (process)
        {
            case 1:
                rNumber = Random.Range(10, 41);
                processTextTMP.text = "+ " + rNumber;
                trueAnswer = xValue + rNumber;
                break;

            case 2:
                rNumber = Random.Range(2, 11);
                processTextTMP.text = "x " + rNumber;
                trueAnswer = xValue * rNumber;
                break;

            case 3:
                if(xValue > 1)
                {
                    rNumber = Random.Range(1, xValue);
                    processTextTMP.text = "- " + rNumber;
                    trueAnswer = xValue - rNumber;
                    
                }
                else
                {
                    CreatingQuestion();
                }
                break;
                

            case 4:
                List<int> dividers = new List<int>();
                for (int i = 1; i <= xValue; i++)
                {
                    if(xValue % i == 0)
                    {
                        dividers.Add(i);
                    }
                }
                rNumber = dividers[Random.Range(0, dividers.Count)];
                processTextTMP.text = "/ " + rNumber;
                trueAnswer = xValue / rNumber;
                break;

                

        }

    }

    // Butona t�klan�nca �a�r�lacak fonksiyon
    public void OnButtonClick(Button btn)
    {
        string clickedNumber = btn.GetComponentInChildren<Text>().text; // Butonun �zerindeki say�y� al
        number1 += clickedNumber; // Say�y� de�i�kene ekle 
        displayTextTmp.text = number1;
        if (int.TryParse(number1, out enteredNumber))
        {
            // Ge�erli say�ya d�n��t�r�ld�
        }
        else
        {
            // Hatal� giri�, number'� s�f�rlayabilirsin
            enteredNumber = 0;
        }
    }
    public void DeleteLastCharacter()
    {
        if (number1.Length > 0)
        {
            number1 = number1.Substring(0, number1.Length - 1); // Son karakteri kald�r 
            displayTextTmp.text = number1;
            if (int.TryParse(number1, out enteredNumber))
            {
                // Ge�erli say�ya d�n��t�rebilirsek, number'� g�ncelle
            }
            else
            {
                // Ge�erli bir say�ya d�n��t�r�lemezse number'� s�f�rlayabilirsin
                enteredNumber = 0;
            }
        }
    }

    public void EnterButtonClick()
    {
        if(enteredNumber == trueAnswer)
        {
            xValue = trueAnswer;
            Score += 10;
            scoreTextTMP.text = ("Score: " + Score);
            timeRemaining = 10;
            Debug.Log("Do�ru cevap");
            CreatingQuestion();
            ClearInput();

            if (Score > highScore)
            {
                highScore = Score;
                topScoreTMP.text = "HighScore: " + highScore;
                PlayerPrefs.SetInt("highScore", highScore);
                PlayerPrefs.Save();
            }
        }
        else
        {
            Debug.Log("Yanl�s Cevap");
            ClearInput();
            health--;
            UptadeHearthUI();
            if(health == 0)
            {
                FinisGame();
            }
        }
        
    }
    

    public void ClearInput()
    {
        number1 = ""; // Say�y� s�f�rla 
        displayTextTmp.text = "";
        enteredNumber = 0;
    }

    public void FinisGame()
    {
        FinishScreen.SetActive(true);
        mainScreen.SetActive(false);
        finishScoreTextTMP.text = "Score: " + Score;
        finishHighScoreTextTMP.text = "HighScore: " + highScore;
        Score = 0;
    }

    public void Reset()
    {
        SceneManager.LoadScene("SampleScene");
    }

    void UptadeHearthUI()
    {
        for (int j = 0; j < hearths.Length; j++)
        {
            if (j < health)
            {
                hearths[j].SetActive(true);
            }
            else
                hearths[j].SetActive(false);
                
        }
    }
}

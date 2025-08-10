using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.SceneManagement;
using NUnit.Compatibility;
using System.Collections;

public class GameController : MonoBehaviour
{
    int health = 3;
    private string number1 = ""; // Girilen say�y� saklayan de�i�ken
    public int enteredNumber;
    public int rNumber;
    public int trueAnswer;
    public int xValue;
    public int process = 4;
    public float timeRemaining = 10;
    public TMP_Text processTextTMP;
    public TMP_Text scoreTextTMP;
    public TMP_Text displayTextTmp;
    public TMP_Text topScoreTMP;
    public TMP_Text timeTextTMP;
    public TMP_Text finishScoreTextTMP;
    public TMP_Text finishHighScoreTextTMP;
    public Text beginnerText;
    public Text easyText;
    public Text mediumText;
    public Text hardText;
    public static int Score;
    public static int highScore;
    public GameObject lobbyScreen;
    public GameObject FinishScreen;
    public GameObject mainScreen;
    public GameObject levelSelectScreen;
    public GameObject[] hearths;
    bool gameReady = false;
    public Image panel;
    public Color correctColor;
    public Color wrongColor;

    //----------------------------//
    int highXValue;
    int additiontMax;
    int additionMin;
    int multiplicationMax;
    int multiplicationMin;
    bool levelBeginner = false;
    bool levelEasy = false;
    bool levelMedium = false;
    bool levelHard = false;
    public static int beginnerHighScore;
    public static int easyHighScore;
    public static int mediumHighScore;
    public static int HardHighScore;


    private void Start()
    {
        CreateFirstQuestion();
        SetHighScore();
    }

    private void Update()
    {
        if(gameReady)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                timeTextTMP.text = timeRemaining.ToString("F0");
            }
            else
            {
                health--;
                UptadeHearthUI();
                timeRemaining = 10;
                if (health == 0)
                {
                    FinisGame();
                    gameReady = false;
                }
            }
        }
       
    }

    public void CreateFirstQuestion()
    {

        rNumber = Random.Range(1, 5);
        int rNumber2 = Random.Range(1, 5);
        processTextTMP.text = rNumber + " + " + rNumber2 + " = X";
        trueAnswer = rNumber + rNumber2;
        if (enteredNumber == trueAnswer)
        {
            xValue = trueAnswer;
            Debug.Log(xValue);
        }
    }

    public void SetHighScore()
    {

        highScore = PlayerPrefs.GetInt("beginnerHighScore", 0);
        beginnerText.text = "ACEM� \nHighScore = " + highScore;
        highScore = PlayerPrefs.GetInt("easyHighScore", 0);
        easyText.text = "KOLAY \nHighScore = " + highScore;
        highScore = PlayerPrefs.GetInt("mediumHighScore", 0);
        mediumText.text = "ORTA \nHighScore = " + highScore;
        highScore = PlayerPrefs.GetInt("hardHighScore", 0);
        hardText.text = "ZOR \nHighScore = " + highScore;
    }

    // Rastgele Say�lar �le Yeni Soru Olu�turma
    public void CreatingQuestion()
    {
        if(xValue > highXValue)
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
                rNumber = Random.Range(additionMin, additiontMax);
                processTextTMP.text = "+ " + rNumber;
                trueAnswer = xValue + rNumber;
                break;

            case 2:
                rNumber = Random.Range(multiplicationMin, multiplicationMax);
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

    // Buton �zerindeki Say�y� Yazd�rma
    public void OnButtonClick(Button btn)
    {
        string clickedNumber = btn.name; // Butonun �zerindeki say�y� al
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

    // Say� Geri Alma
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

    // Yaz�lan Say�y� Kontrol Ettirme
    public void EnterButtonClick()
    {
        if(enteredNumber == trueAnswer)
        {
            xValue = trueAnswer;
            Score += 10;
            scoreTextTMP.text = ("Score: " + Score);
            timeRemaining = 10;
            StartCoroutine(ChangeColor(correctColor));
            Debug.Log("Do�ru cevap");
            CreatingQuestion();
            ClearInput();

            if (Score > highScore)
            {
                highScore = Score;
                topScoreTMP.text = "HighScore: " + highScore;
                if (levelBeginner)
                {
                    PlayerPrefs.SetInt("beginnerHighScore", highScore);
                    PlayerPrefs.Save();
                }
                else if (levelEasy)
                {
                    PlayerPrefs.SetInt("easyHighScore", highScore);
                    PlayerPrefs.Save();
                }
                else if (levelMedium)
                {
                    PlayerPrefs.SetInt("mediumHighScore", highScore);
                    PlayerPrefs.Save();
                }
                else if (levelHard)
                {
                    PlayerPrefs.SetInt("hardHighScore", highScore);
                    PlayerPrefs.Save();
                }
            }
        }
        else
        {
            StartCoroutine(ChangeColor(wrongColor));
            Debug.Log("Yanl�s Cevap");
            ClearInput();
            health--;
            UptadeHearthUI();
            if(health == 0)
            {
                FinisGame();
                gameReady = false;
            }
        }
        
    }

    IEnumerator ChangeColor(Color newColor)
    {
        panel.gameObject.SetActive(true);
        panel.color = newColor;
        yield return new WaitForSeconds(0.1f);
        panel.gameObject.SetActive(false);
    }
    
    // Cevap �nput Temizleme
    public void ClearInput()
    {
        number1 = ""; // Say�y� s�f�rla 
        displayTextTmp.text = "";
        enteredNumber = 0;
    }

    // Oyun Biti� Sahnesini Sunma
    public void FinisGame()
    {
        FinishScreen.SetActive(true);
        //addScreen.SetActive(true);
        mainScreen.SetActive(false);
        finishScoreTextTMP.text = "Score: " + Score;
        finishHighScoreTextTMP.text = "HighScore: " + highScore;
        
    }

    // Oyunu Yeniden Ba�latma
    public void Reset()
    {
        Score = 0;
        health = 3;
        timeRemaining = 10;
        levelBeginner = false;
        levelEasy = false;
        levelMedium = false;
        levelHard = false;
        UptadeHearthUI();
        SetHighScore();
        FinishScreen.SetActive(false);
        levelSelectScreen.SetActive(true);
        //SceneManager.LoadScene("SampleScene");
    }


    // Ana Men�den Oyun Sahnesine Ge�i�
    public void PlayGame()
    {
        lobbyScreen.SetActive(false);
        levelSelectScreen.SetActive(true);
    }


    // Oyuncunun Can G�rsellerini G�ncelleme
    public void UptadeHearthUI()
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

    // Ekrana Reklam ��kartma
    public void ShowRewardedAd()
    {
        AddMob.Instance.ShowRewardedAd();
    }

    // Oyuncuyu Reklam Sonras� �d�llendirme
    public void RewardPlayer()
    {
        timeRemaining += 15; // 10 saniye ekle
        if (health < hearths.Length)
        {
            health++; // Can ekle (E�er maksimum de�ilse)
        }
        UptadeHearthUI(); // Can UI'yi g�ncelle

        FinishScreen.SetActive(false);
        mainScreen.SetActive(true);
        //addScreen.SetActive(false);

    }

    public void BeginnerLevel()
    {
        CreateFirstQuestion();
        highXValue = 20;
        additiontMax = 21;
        additionMin = 1;
        multiplicationMax = 4;
        multiplicationMin = 1;
        levelBeginner = true;
        highScore = PlayerPrefs.GetInt("beginnerHighScore", 0);
        topScoreTMP.text = "HighScore: " + highScore;
        levelSelectScreen.SetActive(false);
        mainScreen.SetActive(true);
        gameReady = true;
    }
    
    public void EasyLevel()
    {
        CreateFirstQuestion();
        highXValue = 30;
        additiontMax = 31;
        additionMin = 1;
        multiplicationMax = 6;
        multiplicationMin = 1;
        levelEasy = true;
        highScore = PlayerPrefs.GetInt("easyHighScore", 0);
        topScoreTMP.text = "HighScore: " + highScore;
        levelSelectScreen.SetActive(false);
        mainScreen.SetActive(true);
        gameReady = true;
    }

    public void MediumLevel() 
    {
        CreateFirstQuestion();
        highXValue = 75;
        additiontMax = 101;
        additionMin = 15;
        multiplicationMax = 16;
        multiplicationMin = 3;
        levelMedium = true;
        highScore = PlayerPrefs.GetInt("mediumHighScore", 0);
        topScoreTMP.text = "HighScore: " + highScore;
        levelSelectScreen.SetActive(false);
        mainScreen.SetActive(true);
        gameReady = true;
    }

    public void HardLevel()
    {
        CreateFirstQuestion();
        highXValue = 150;
        additiontMax = 151;
        additionMin = 50;
        multiplicationMax = 21;
        multiplicationMin = 7;
        levelHard = true;
        highScore = PlayerPrefs.GetInt("hardHighScore", 0);
        topScoreTMP.text = "HighScore: " + highScore;
        levelSelectScreen.SetActive(false);
        mainScreen.SetActive(true);
        gameReady = true;
    }
}

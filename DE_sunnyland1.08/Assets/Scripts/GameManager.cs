using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject gameOverTextObj;
    [SerializeField] GameObject gameClearTextObj;
    [SerializeField] GameObject pauseUI;
    [SerializeField] Text scoreText;
    [SerializeField] Text gemScoreText;
    [SerializeField] Text timeText;
    [SerializeField] Text ReStartText;
    [SerializeField] Text ReturnText;
    [SerializeField] Text QuitText;

    //SE
    [SerializeField] AudioClip clearSound;
    [SerializeField] AudioClip gameOverSound;

    public enum PAUSE_SELECT_STATUS
    {
        RESTART,
        RETURN,
        QUIT,
    }
    PAUSE_SELECT_STATUS pauseSelectStarus = PAUSE_SELECT_STATUS.RESTART;

    public static int currentCherry = 3;

    private const int MAX_SCORE = 999999;
    private const int MAX_CHERRY_SCORE = 99;
    private float countTime = 400;
    private float alphaTime;
    private int score = 0;
    private static int highScore;
    private int gemScore = 0;
    private bool stopTime = false;

    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        scoreText.text = score.ToString("D6");
        gemScoreText.text = "×" + gemScore.ToString("D2");
        timeText.text = countTime.ToString("0");

        highScore = PlayerPrefs.GetInt("highscore");
    }

    void Update()
    {
        if (!stopTime)
        {
            CountTime();
        }
        else
        {
            AddTimeScore();
        }

        Pause();
    }

    public void AddTimeScore()
    {
        if (countTime - 1 > 0.0f) 
        {
            countTime--;
            score += 100;
            Debug.Log(countTime);
            timeText.text = countTime.ToString("0");
            scoreText.text = score.ToString("D6");
        }

        if (highScore < score)
        {
            PlayerPrefs.SetInt("highscore", score);
            PlayerPrefs.Save();
        }
        
    }

    public void CountTime()
    {
        countTime -= Time.deltaTime;
        if (countTime < 0) 
        {
            GameOver();
        }
        timeText.text = countTime.ToString("0");
    }

    public void AddScore(int value)
    {
        score += value;
        if (score > MAX_SCORE)
        {
            score = MAX_SCORE;
        }
        scoreText.text = score.ToString("D6");
    }

    public void AddGemScore(int value)
    {
        gemScore += value;
        if (gemScore > MAX_CHERRY_SCORE)
        {
            gemScore = MAX_CHERRY_SCORE;
        }
        gemScoreText.text = "×" + gemScore.ToString("D2");
    }

    public void GameOver()
    {
        if (currentCherry == 0)
        {
            gameOverTextObj.SetActive(true);
            Invoke("TitleScene", 3.0f);
            audioSource.Stop();
            audioSource.PlayOneShot(gameOverSound);
            return;

        }
        audioSource.Stop();
        audioSource.PlayOneShot(gameOverSound);
        currentCherry -= 1;
        //Debug.Log("currentCherry" + currentCherry);
        Invoke("TransitionScene", 3.0f);
    }

    public void GameClear()
    {
        stopTime = true;
        gameClearTextObj.SetActive(true);
        audioSource.Stop();
        audioSource.PlayOneShot(clearSound);
        Invoke("TitleScene", 8.0f);
    }

    private void ReStartThisScene()
    {
        Scene ThisScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(ThisScene.name);
    }

    public void TransitionScene()
    {
        SceneManager.LoadScene("Transition");
    }

    public void TitleScene()
    {
        SceneManager.LoadScene("Title");
    }

    public void Pause()
    {
        if (Input.GetKeyDown("q"))
        {
            pauseUI.SetActive(true);
            Time.timeScale = 0f;
        }

        switch (pauseSelectStarus)
        {
            case PAUSE_SELECT_STATUS.RESTART:
                ReStartText.color = Color.black;
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    ReStartThisScene();
                    Time.timeScale = 1f;
                }
                break;
            case PAUSE_SELECT_STATUS.RETURN:
                ReturnText.color = Color.black;
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    pauseUI.SetActive(false);
                    Time.timeScale = 1f;
                }
                break;
            case PAUSE_SELECT_STATUS.QUIT:
                QuitText.color = Color.black;
                if (Input.GetKey(KeyCode.Return))
                {
                    TitleScene();
                    Time.timeScale = 1f;
                }
                break;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (pauseSelectStarus == PAUSE_SELECT_STATUS.RESTART)
            {
                pauseSelectStarus = PAUSE_SELECT_STATUS.RETURN;
                ReStartText.color = Color.gray;
            }
            else if (pauseSelectStarus == PAUSE_SELECT_STATUS.RETURN)
            {
                pauseSelectStarus = PAUSE_SELECT_STATUS.QUIT;
                ReturnText.color = Color.gray;
            }
            else
            {
                pauseSelectStarus = PAUSE_SELECT_STATUS.QUIT;
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (pauseSelectStarus == PAUSE_SELECT_STATUS.RESTART)
            {
                pauseSelectStarus = PAUSE_SELECT_STATUS.RESTART;
            }
            else if (pauseSelectStarus == PAUSE_SELECT_STATUS.RETURN)
            {
                pauseSelectStarus = PAUSE_SELECT_STATUS.RESTART;
                ReturnText.color = Color.gray;
            }
            else
            {
                pauseSelectStarus = PAUSE_SELECT_STATUS.RETURN;
                QuitText.color = Color.gray;
            }
        }
    }

    Color GetAlphaColor(Color color)
    {
        alphaTime += Time.deltaTime * 4.0f;
        color.a = Mathf.Sin(alphaTime) * 0.5f + 0.5f;

        return color;
    }
}

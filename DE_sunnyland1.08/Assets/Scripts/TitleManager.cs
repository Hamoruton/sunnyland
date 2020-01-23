using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField] Text startText;
    [SerializeField] Text quitText;
    [SerializeField] Text highScoreText;

    public enum SELECT_STATUS
    {
        START,
        QUIT
    }
    SELECT_STATUS selectStatus = SELECT_STATUS.START;

    private float alphaTime;

    void Start()
    {
        GameManager.currentCherry = 3;
        highScoreText.text = PlayerPrefs.GetInt("highscore").ToString("D6");
        //PlayerPrefs.SetInt("highscore", 0);
    }

    void Update()
    {
        Move();
    }

    public void Move()
    {
        switch (selectStatus)
        {
            case SELECT_STATUS.START:
                startText.color = GetAlphaColor(startText.color);
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    SceneManager.LoadScene("Transition");
                }
                break;
            case SELECT_STATUS.QUIT:
                quitText.color = GetAlphaColor(quitText.color);
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    UnityEngine.Application.Quit();
                }
                break;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (selectStatus == SELECT_STATUS.QUIT)
            {
                quitText.color = Color.black;
            }

            selectStatus = SELECT_STATUS.START;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {

            if (selectStatus == SELECT_STATUS.START)
            {
                startText.color = Color.black;
            }

            selectStatus = SELECT_STATUS.QUIT;
        }
        
    }

    Color GetAlphaColor(Color color)
    {
        alphaTime += Time.deltaTime * 4.0f;
        color.a = Mathf.Sin(alphaTime) * 0.5f + 0.5f;

        return color;
    }
}

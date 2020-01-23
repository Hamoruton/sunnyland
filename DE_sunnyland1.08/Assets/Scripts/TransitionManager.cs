using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    [SerializeField] Text playerRemaining;

   void Update()
    {
        PlayerRemaining();
        Invoke("GameScene", 2.0f);
    }

    public void PlayerRemaining()
    {
        playerRemaining.text = GameManager.currentCherry.ToString("0");
    }

    public void GameScene()
    {
        SceneManager.LoadScene("Game");   
    }
}

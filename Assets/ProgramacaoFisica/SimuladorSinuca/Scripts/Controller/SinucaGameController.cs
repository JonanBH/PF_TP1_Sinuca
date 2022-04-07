using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SinucaGameController : MonoBehaviour
{
    [SerializeField]
    private List<Ball> foodBalls;
    [SerializeField]
    private TMP_Text remainingBallsText;
    [SerializeField]
    private TMP_Text hitsCountText;

    [SerializeField]
    private GameObject scoreParent;
    [SerializeField]
    private GameObject gameoverParent;

    public static SinucaGameController singleton;

    private int hits = 0;

    private void Awake()
    {
        singleton = this;
    }

    private void Start()
    {
        UpdateRemainingBalls();
        Ball.OnBallPocketed += HandlePocketedBall;
    }

    public void HandlePocketedBall(Ball ball)
    {
        foodBalls.Remove(ball);
        UpdateRemainingBalls();

        if(foodBalls.Count == 0)
        {
            GameOver();
        }
    }

    private void HandleVictory()
    {
        Debug.Log("Victory");
    }

    public void UpdateRemainingBalls()
    {
        remainingBallsText.text = foodBalls.Count.ToString();
    }

    public void UpdateHitsCounter()
    {
        hits++;
        hitsCountText.text = hits.ToString();
    }

    public void WhiteBallPocketed()
    {
        GameOver();
    }

    private void GameOver()
    {
        gameoverParent.SetActive(true);
    }


    public void PlayAgain()
    {
        SceneManager.LoadScene(0);
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using a_Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaganer : Singleton<GameMaganer>
{
    [SerializeField] private TextMeshProUGUI scoreLabel;
	[SerializeField] private TextMeshProUGUI timeLabel; 
	[SerializeField] private TextMeshProUGUI gameOverScoreLabel;
	[SerializeField] private TextMeshProUGUI gameOverBestLabel; 
	[SerializeField] private Animator scoreEffect;
	[SerializeField] private Animator UIAnimator;
	[SerializeField] private Animator gameOverAnimator;
	[SerializeField] private AudioSource gameOverAudio;
	[SerializeField] private CarController car;
	
	private float time;
	private	int score;
	
	private	bool gameOver;
	
	private	void Start()
	{
		UpdateScore(0);
	}
	
	private void Update()
	{
		UpdateTimer();
		
		if(gameOver && (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0)))
		{
			UIAnimator.SetTrigger("Start");
			StartCoroutine(LoadScene(SceneManager.GetActiveScene().name));
		}
	}
	
	private void UpdateTimer()
	{
		time += Time.deltaTime;
		int timer = (int)time;
		
		int seconds = timer % 60;
		int minutes = timer / 60;
		
		string secondsRounded = ((seconds < 10) ? "0" : "") + seconds;
		string minutesRounded = ((minutes < 10) ? "0" : "") + minutes;
		
		timeLabel.text = minutesRounded + ":" + secondsRounded;
	}
	
	public void UpdateScore(int points)
	{
		score += points;
		
		scoreLabel.text = "" + score;
		if (points != 0)
		{
			scoreEffect.SetTrigger("Score");
		}
			
	}
	
	public void GameOver()
	{
		if(gameOver)
			return;
		
		SetScore();
		
		gameOverAnimator.SetTrigger("Game over");
		gameOverAudio.Play();
		
		gameOver = true;
		
		car.FallApart();
		
		foreach(BasicMovement basicMovement in FindObjectsByType<BasicMovement>(FindObjectsSortMode.None))
		{
			basicMovement.moveSpeed = 0;
			basicMovement.rotateSpeed = 0;
		}
	}
	
	private void SetScore()
	{
		if (score > PlayerPrefs.GetInt("best"))
		{
			PlayerPrefs.SetInt("best", score);
		}
			
		
		gameOverScoreLabel.text = "score: " + score;
		gameOverBestLabel.text = "best: " + PlayerPrefs.GetInt("best");
	}
	
	private IEnumerator LoadScene(string scene)
	{
		yield return new WaitForSeconds(0.6f);
		
		SceneManager.LoadScene(scene);
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;
using System.Net.Sockets;

public class GameManager : MonoBehaviour
{
    public List<GameObject> targets;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI livesText;
    public Button restartButton;
    public Button resumeButton;
    public Button mainMenuButton;
    public Button quitButton;
    public GameObject menuScreen;
    public GameObject PauseMenuScreen;
    public AudioSource mainAudio;
    public AudioClip gameOverSound;
    public AudioClip BadSound;
    public AudioClip Bad2Sound;
    public AudioClip GoodSound;
    public bool isGameActive;
    public bool gameOver = false;
    public bool isClicked = false;
    public bool gameIsPaused = false;
    public int highScore;
    public int score;
    public int lives;
    public string previousScene;
    public Difficulties difficulty;

    private float spawnRate = 1.0f;


    // Start is called before the first frame update
    void Start()
    {
        mainAudio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        InputForPauseMenu();
        SetScoreUnderZeroToZero();
    }

    IEnumerator SpawnTarget()
    {
        while (isGameActive)
        {
            int index = Random.Range(0, targets.Count);
            yield return new WaitForSeconds(spawnRate);
            Instantiate(targets[index]);
        }

    }

    public void StopMainAudioOnGameOver()
    {
        mainAudio.Stop();
        Debug.Log("Main audio stops on game over!");
    }


    public void ClickBadSound()
    {
        mainAudio.PlayOneShot(BadSound, 1.0f);
        Debug.Log("Good sound was played!");
    }

    public void ClickBad2Sound()
    {
        mainAudio.PlayOneShot(Bad2Sound, 1.0f);
        Debug.Log("Bad2 sound works!");
    }

    public void ClickGoodSound()
    {
        mainAudio.PlayOneShot(GoodSound, 1.0f);
        Debug.Log("Bad sound was played!");
    }

    public void GameOverSoundOnGameOver()
    {
        mainAudio.PlayOneShot(gameOverSound, 1.0f);
        Debug.Log("Game over was played!");
    }

    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score;
        Debug.Log("The score updates!");
    }

    public void UpdateLives(int livesToChange)
    {
        lives = lives + livesToChange;
        livesText.text = "Lives: " + lives;

        if (lives <= 0)
        {
            GameOver();        
            StopMainAudioOnGameOver();
            GameOverSoundOnGameOver();
            mainAudio.PlayOneShot(gameOverSound, 1.0f);
        }
    }

    public void SetScoreUnderZeroToZero()
    {
        if (score < 0)
        {
            score = 0;
            scoreText.text = "Score: " + score;
        }
    }

    public void CheckSaveHighScore()
    {

        if (score > highScore && difficulty == Difficulties.Easy)
        {
            highScore = score;
            PlayerPrefs.SetInt("highScoreEasy", highScore);
            Debug.Log("HighScore was checked and saved!");
        }

        else if (score > highScore && difficulty == Difficulties.Medium)
        {
            highScore = score;
            PlayerPrefs.SetInt("highScoreMedium", highScore);
            Debug.Log("HighScore was checked and saved!");
        }

        else if (score > highScore && difficulty == Difficulties.Hard)
        {
            highScore = score;
            PlayerPrefs.SetInt("highScoreHard", highScore);
            Debug.Log("HighScore was checked and saved!");
        }

        else if (score > highScore && difficulty == Difficulties.Insane)
        {
            highScore = score;
            PlayerPrefs.SetInt("highScoreInsane", highScore);
            Debug.Log("Highscore was checked and saved");
        }
        highScoreText.text = highScore.ToString();
    }

    public void UpdateHighScore(int highScore)
    {
        highScoreText.text = "HighScore: " + highScore;
        Debug.Log("HighScore updates");
    }
    public void GameOver()
    {
        isGameActive = false;
        restartButton.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(true);
        Debug.Log("The game is over!");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.Log("The restart button is enabled");
    }


    public void StartGame(Difficulties difficulty)
    {
        this.difficulty = difficulty;

        isGameActive = true;
        StartCoroutine(SpawnTarget());
        UpdateScore(0);
        UpdateHighScore(highScore);
        DifficultiesForEach(difficulty);
        LoadHighScoreOnStartGame(difficulty);
        UpdateLives(3);
        menuScreen.SetActive(false);
        Debug.Log("The game started!");
    }


    public void LoadHighScoreOnStartGame(Difficulties difficulty)
    {

        if (difficulty == Difficulties.Easy)
        {
            highScore = PlayerPrefs.GetInt("highScoreEasy");
            Debug.Log("Easy highscore loads on start!");
        }

        else if (difficulty == Difficulties.Medium)
        {
            highScore = PlayerPrefs.GetInt("highScoreMedium");
            Debug.Log("Medium highscore loads on start!");
        }

        else if (difficulty == Difficulties.Hard)
        {
            highScore = PlayerPrefs.GetInt("highScoreHard");
            Debug.Log("Hard highscore loads on start!");
        }

        else if (difficulty == Difficulties.Insane)
        {
            highScore = PlayerPrefs.GetInt("highScoreInsane");
            Debug.Log("Insane highscore loads on start!");
        }
        UpdateHighScore(highScore);
    }


    public void DifficultiesForEach(Difficulties difficulty)
    {
        if (difficulty == Difficulties.Easy && !isClicked)
        {
            isClicked = true;
            spawnRate = 1.0f;
            Debug.Log("The button easy was clicked!");
        }

        else if (difficulty == Difficulties.Medium && !isClicked)
        {
            isClicked = true;
            spawnRate = 0.8f;
            Debug.Log("The button medium was clicked!");
        }

        else if (difficulty == Difficulties.Hard && !isClicked)
        {
            isClicked = true;
            spawnRate = 0.6f;
            Debug.Log("The button hard was clicked!");
        }

        else if (difficulty == Difficulties.Insane && !isClicked)
        {
            isClicked = true;
            spawnRate = 0.4f;
            Debug.Log("The button insane was clicked!");
        }
    }

    public void InputForPauseMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isGameActive == true)
        {
            if (gameIsPaused)
            {
                Resume();
            }

            else
            {
                Pause();
            }
        }

    }

    public void Resume()
    {
        Debug.Log("The pause screen is deactivated and the game resumes!");
        PauseMenuScreen.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;

        mainAudio.Play();
    }

    public void Pause()
    {
        Debug.Log("The pause screen is active and the game is paused!");
        PauseMenuScreen.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;

        mainAudio.Stop();

    }

    public void Awake()
    {
        previousScene = SceneManager.GetActiveScene().name;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        Debug.Log("Loading menu...");
        SceneManager.LoadScene("Menu Scene");

    }

    public void OnResumeButtonClicked()
    {
        Resume();
        SceneManager.LoadScene(previousScene);
    }

    public void Quit()
    {
        Debug.Log("Quiting game and Editor play mode...");
        Application.Quit();
    }
}








using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{

    public ParticleSystem explosionParticle;
    public int pointValue;

    private Rigidbody targerRb;
    private GameManager gameManager;
    private float minSpeed = 8;
    private float maxSpeed = 15;
    private float minTorque = -10;
    private float maxTorque = 10;
    private float ySpawnPos = -2;
    private float xRange = 4;
    private float yRange = -10;



    // Start is called before the first frame update
    void Start()
    {
        targerRb = GetComponent<Rigidbody>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        transform.position = RandomSpawnPos();
        targerRb.AddForce(RandomForce(), ForceMode.Impulse);
        targerRb.AddTorque(RandomTorque(), RandomTorque(), RandomTorque(), ForceMode.Impulse);


    }

    // Update is called once per frame
    void Update()
    {
        DestroyGoodOutOfBounds();
        DestroyBadOutOfBounds();
    }

    private void OnMouseDown()
    {
        DestroyTarget();
    }

    public void DestroyTarget()
    {
        if (gameObject.CompareTag("Good") && gameManager.isGameActive && gameManager.gameIsPaused == false)
        {
            Destroy(gameObject);
            Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
            gameManager.UpdateScore(pointValue);
            gameManager.ClickGoodSound();
            Debug.Log("Click good sound works!");
        }

        if (gameObject.CompareTag("Bad") && gameManager.isGameActive && gameManager.gameIsPaused == false)
        {
            Destroy(gameObject);
            Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
            gameManager.UpdateScore(pointValue);
            gameManager.ClickBadSound();
            gameManager.UpdateLives(-1);
            Debug.Log("Click bad sound works!");
        }

        if (gameObject.CompareTag("Bad2") && gameManager.isGameActive && gameManager.gameIsPaused == false)
        {
            Destroy(gameObject);
            Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
            gameManager.UpdateScore(pointValue);
            gameManager.ClickBad2Sound();
            gameManager.UpdateLives(-1);
            Debug.Log("Click bad sound works!");
        }

    }

    public void DestroyGoodOutOfBounds()
    {
        if (transform.position.y < yRange && gameObject.CompareTag("Good") && gameManager.isGameActive)
        {
            gameManager.isGameActive = false;
            Destroy(gameObject);
            gameManager.GameOver();
            gameManager.StopMainAudioOnGameOver();
            gameManager.GameOverSoundOnGameOver();
            gameManager.CheckSaveHighScore();
            gameManager.UpdateHighScore(gameManager.highScore);
            Debug.Log("Good objects have been destroyed!");
        }

    }

    public void DestroyBadOutOfBounds()
    {
        if (transform.position.y < yRange && gameObject.CompareTag("Bad") && gameManager.isGameActive)
        {
            Destroy(gameObject);
            Debug.Log("Bad objects have been destroyed!");
        }
    }


    Vector3 RandomForce()
    {
        return Vector3.up * Random.Range(minSpeed, maxSpeed);
    }

    float RandomTorque()
    {
        return Random.Range(-minTorque, maxTorque);
    }

    Vector3 RandomSpawnPos()
    {
        return new Vector3(Random.Range(-xRange, xRange), ySpawnPos);
    }



}

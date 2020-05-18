using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class TestFoodEmitter : MonoBehaviour
{
    public GameObject[] food;

    public float spawnIntervall;

    public float speed;

    public float emitterWidth;
    public float startDelay;

    public float lifetime = 5;

    private bool started = false;

    private IEnumerator spawningCoroutine;

    private void Awake()
    {
        GameController.OnGameStateChanged += HandleGameStateChange;
    }

    private void OnDestroy()
    {
        GameController.OnGameStateChanged -= HandleGameStateChange;
    }

    private void Spawn()
    {
        var emittedFood = Instantiate(food[Random.Range(0, food.Length)],
                    new Vector3(Random.Range(-emitterWidth, emitterWidth), 0, 0) + transform.position, Quaternion.identity);
                emittedFood.GetComponent<Rigidbody>().velocity = Vector3.up * speed;
                
                Destroy(emittedFood, lifetime);
    }

    private void HandleGameStateChange(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.InGame:
                if (!started)
                {
                    InvokeRepeating(nameof(Spawn), startDelay, spawnIntervall);
                    started = true;
                }
                break;
            case GameState.GameOver:
                if (started)
                {
                    CancelInvoke(nameof(Spawn));
                    DestroySpawnedObjects();
                    started = false;
                }
                break;
        }
    }

    private void DestroySpawnedObjects()
    {
        var goodObjects = GameObject.FindGameObjectsWithTag("good");
        var badObjects = GameObject.FindGameObjectsWithTag("bad");

        foreach(var gm in goodObjects)
        {
            Destroy(gm);
        }

        foreach (var gm in badObjects)
        {
            Destroy(gm);
        }
    }
}

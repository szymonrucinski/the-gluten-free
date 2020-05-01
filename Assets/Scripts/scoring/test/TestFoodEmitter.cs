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

    private IEnumerator spawningCoroutine;

    private void Awake()
    {
        GameController.OnGameStateChanged += HandleGameStateChange;
    }

    private void OnDestroy()
    {
        GameController.OnGameStateChanged -= HandleGameStateChange;
    }


    // Start is called before the first frame update
    void Start()
    {
        spawningCoroutine = Spawn(spawnIntervall);
    }

    IEnumerator Spawn(float waitTime)
    {
        //wait start delay
        yield return new WaitForSeconds(startDelay);
        while (true)
        {
            var emittedFood = Instantiate(food[Random.Range(0, food.Length)],
                    new Vector3(Random.Range(-emitterWidth, emitterWidth), 0, 0) + transform.position,
                    Quaternion.identity);
                emittedFood.GetComponent<Rigidbody>().velocity = Vector3.up * speed;
                Destroy(emittedFood, lifetime);

                //wait spawned interval
                yield return new WaitForSeconds(waitTime);
        }
    }

    private void HandleGameStateChange(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.InGame: StartCoroutine(spawningCoroutine);
                break;
            case GameState.Pause: StopCoroutine(spawningCoroutine);
                break;
            case GameState.SHOW_SHOPPING_LIST:
            case GameState.GameOver: 
                StopCoroutine(spawningCoroutine);
                DestroySpawnedObjects();
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

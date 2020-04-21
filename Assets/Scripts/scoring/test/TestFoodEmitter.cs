using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class TestFoodEmitter : MonoBehaviour
{
    public static TestFoodEmitter Instance;

    public GameObject[] food;

    public float spawnIntervall;

    public float speed;

    public float emitterWidth;
    public float startDelay;

    public float lifetime = 5;

    private void Awake()
    {
        Instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Spawn", startDelay,spawnIntervall);
    }

    void Spawn()
    {
        if (GameController.Instance.currentGameState == GameState.InGame)
        {
            var emittedFood = Instantiate(food[Random.Range(0, food.Length)],
                new Vector3(Random.Range(-emitterWidth, emitterWidth), 0, 0) + transform.position, Quaternion.identity);
            emittedFood.GetComponent<Rigidbody>().velocity = Vector3.up * speed;
            Destroy(emittedFood, lifetime);
        }
    }
}

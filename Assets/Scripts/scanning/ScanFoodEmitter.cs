using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class ScanFoodEmitter : MonoBehaviour
{
    public static ScanFoodEmitter Instance;
    public static GameController gameController;


    public GameObject[] Food;
    public float SpawnIntervall;        // Spawn intervall
    public float Speed;                 // Emission speed
    public float EmitterWidth;          // Half width of emitter
    public float EmitterHeight;
    public float startDelay;
    public float lifeTime;              // time emitted food is active in the scene

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        gameController = GameController.Instance;
    }

    public void StopSpawning()
    {
        CancelInvoke();
    }

    public void StartSpawning()
    {
        gameController.StartGame();
        InvokeRepeating("Spawn", startDelay, SpawnIntervall);
    }

    void Spawn()
    {
        GameObject emittedFood = Instantiate(Food[Random.Range(0, Food.Length)], new Vector3(Random.Range(-EmitterWidth, EmitterWidth), Random.Range(-EmitterHeight, EmitterHeight), 6), Quaternion.identity);
        Destroy(emittedFood, lifeTime);                                                    // deletes object after it is out of sight
    }
}
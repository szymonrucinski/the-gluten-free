using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodEmitter : MonoBehaviour
{
    public GameObject[] Food;
    public float SpawnIntervall;        // Spawn intervall
    public float startDelay = 0;
    private Vector3 spawnPosition = new Vector3(0.57f, 4.34f, -3.1f);

    
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Spawn", startDelay, SpawnIntervall);
    }

    void Spawn()
    {
        GameObject emittedFood = Instantiate(Food[Random.Range(0, Food.Length)],spawnPosition,transform.rotation);
    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodEmitter : MonoBehaviour
{
    public GameObject[] Food;
    public float SpawnIntervall;        // Spawn intervall
    public float startDelay = 0;

    
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Spawn", startDelay, SpawnIntervall);
    }

    void Update()
    {
        //Spawn();
    }

    void Spawn()
    {
        GameObject emittedFood = Instantiate(Food[Random.Range(0, Food.Length)],new Vector3(0.8239996f, 1.175f,-3.39f),transform.rotation);
    }



}

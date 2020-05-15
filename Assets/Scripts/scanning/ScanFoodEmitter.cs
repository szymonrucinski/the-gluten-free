using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanFoodEmitter : MonoBehaviour
{
    public GameObject[] Food;
    public float SpawnIntervall;        // Spawn intervall
    public float Speed;                 // Emission speed
    public float EmitterWidth;          // Half width of emitter
    public float EmitterHeight;
    public float startDelay;
    public float lifeTime;              // time emitted food is active in the scene

    
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Spawn", startDelay, SpawnIntervall);
    }

    void Spawn()
    { 
        GameObject emittedFood = Instantiate(Food[Random.Range(0, Food.Length)],new Vector3(Random.Range(-EmitterWidth, EmitterWidth), Random.Range(-EmitterHeight, EmitterHeight), 3), Quaternion.identity);
        emittedFood.tag = Random.Range(0, 2) == 1 ? "good" : "bad";
        Destroy(emittedFood, lifeTime);                                                    // deletes object after it is out of sight
    }



}

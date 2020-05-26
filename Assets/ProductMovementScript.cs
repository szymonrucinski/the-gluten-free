using UnityEngine;
using System.Collections;



public class ProductMovementScript : MonoBehaviour
{
    private GameController gameController;
    private Vector3 startPosition;



    // Start is called before the first frame update
    void Start()
    {
        gameController = GameController.Instance;
        transform.position = new Vector3(Random.Range(-10.0f, 10.0f), 0, 40);
      

        

    }

    // Update is called once per frame
    void Update()
    {
        if (gameController.currentGameState == GameState.InGame)
        {
            transform.position += new Vector3(0.0f, 0.0f,-0.1f);
                }

        if (gameController.currentGameState == GameState.GameOver) Destroy(gameObject);
    }
}

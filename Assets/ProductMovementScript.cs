using UnityEngine;
using System.Collections;



public class ProductMovementScript : MonoBehaviour
{
    private GameController gameController;
    private Vector3 _startPosition;



    // Start is called before the first frame update
    void Start()
    {
        gameController = GameController.Instance;

        

    }

    // Update is called once per frame
    void Update()
    {
        if (gameController.currentGameState == GameState.InGame)
        {
            transform.position += new Vector3(0.0f, 0.0f, Mathf.Sin(Time.time)/10);
                }

        if (gameController.currentGameState == GameState.GameOver) Destroy(gameObject);
    }
}

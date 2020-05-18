using UnityEngine;

public class CameraController : MonoBehaviour
{
    private SimpleSmoothMouseLook simple;

    private void Awake()
    {
        simple = GetComponent<SimpleSmoothMouseLook>();
    }

    // Start is called before the first frame update
    void Start()
    {
        GameController.OnGameStateChanged += HandleGameStateChange;
    }

    private void HandleGameStateChange(GameState state)
    {
        switch (state)
        {
            case GameState.InGame: simple.enabled = true;
                break;
            case GameState.GameOver: simple.enabled = false;
                break;
            case GameState.Pause: simple.enabled = false;
                break;
            case GameState.SHOW_SHOPPING_LIST: simple.enabled = false;
                break;
        }
    }

    private void OnDestroy()
    {
        GameController.OnGameStateChanged -= HandleGameStateChange;
    }
}

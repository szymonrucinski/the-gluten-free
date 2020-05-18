using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ScannerVRController : MonoBehaviour
{
    private InputDevice device;
    private Quaternion deviceRotation;
    private GameController gameController;
    private ScanFoodEmitter scanFoodEmitter;
    private bool click;
    // Start is called before the first frame update
    void Start()
    {
        gameController = GameController.Instance;
        scanFoodEmitter = ScanFoodEmitter.Instance;
        device = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
    }

    // Update is called once per frame
    void Update()
    {
        device.TryGetFeatureValue(CommonUsages.triggerButton, out click);
        if (gameController.currentGameState == GameState.SHOW_SHOPPING_LIST || gameController.currentGameState == GameState.GameOver)
        {
            if (click)
            {
                gameController.StartGame();
                scanFoodEmitter.StartSpawning();
            }
            if(gameController.currentGameState == GameState.GameOver)
            {
                scanFoodEmitter.StopSpawning();
            }
        }
        if (device.TryGetFeatureValue(CommonUsages.deviceRotation, out deviceRotation)) {
            transform.rotation = deviceRotation;
            transform.localRotation *= Quaternion.Euler(0, -90, 0);
        } 

    }
}

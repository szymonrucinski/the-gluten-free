using TMPro;
using UnityEngine;

public class GameOverController : MonoBehaviour
{
    public TextMeshProUGUI gameOverReasonText;

    public void gameOverReason(bool timeOut)
    {
        Color red = Color.red;
        Color green = Color.green;

        if (timeOut)
        {
            gameOverReasonText.color = red;
            gameOverReasonText.text = "Time ran out! Sorry!";
        }
        else
        {
            gameOverReasonText.color = green;
            gameOverReasonText.text = "You bought all products! Well done!";
        }
    }
}

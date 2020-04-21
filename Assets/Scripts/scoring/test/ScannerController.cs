using UnityEngine;

public class ScannerController : MonoBehaviour
{ 
    // Update is called once per frame
    void Update()
    {
        if (GameController.Instance.currentGameState == GameState.InGame)
        {

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 2000, Color.white);

                if (Physics.Raycast(transform.position, transform.forward, out RaycastHit HitInfo))
                {
                    if (HitInfo.collider.CompareTag("good"))
                    {
                        ScoreController.Instance.scoreAction(true, false, HitInfo.transform.position);
                        Destroy(HitInfo.collider.gameObject);
                    }
                    else if (HitInfo.collider.CompareTag("bad"))
                    {
                        ScoreController.Instance.scoreAction(false, false, HitInfo.transform.position);
                        Destroy(HitInfo.collider.gameObject);
                    }
                }
            }
        }
    }

}

using UnityEngine;

public class ScannerController : MonoBehaviour
{

    public LineRenderer laserLine;

    public float lineWidth = 0.05f;
    public float length = 1000.0f;

    void Start()
    {
        laserLine.startWidth = lineWidth;
        laserLine.endWidth = lineWidth;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.Instance.currentGameState == GameState.InGame)
        {

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                var pos = transform.position;
                laserLine.enabled = true;
                laserLine.SetPosition(0,pos);
                laserLine.SetPosition(1, transform.TransformDirection(Vector3.forward) * length);

                Debug.DrawRay(pos, transform.TransformDirection(Vector3.forward) * 2000, Color.white);


                if (Physics.Raycast(pos, transform.forward, out RaycastHit HitInfo))
                {
                    if (HitInfo.collider.CompareTag("good"))
                    {
                        ScoreController.Instance.scoreAction(true, false, HitInfo.point);
                        Destroy(HitInfo.collider.gameObject);
                    }
                    else if (HitInfo.collider.CompareTag("bad"))
                    {
                        ScoreController.Instance.scoreAction(false, false, HitInfo.point);
                        Destroy(HitInfo.collider.gameObject);
                    }
                }
                

            }
            else
            {
                laserLine.enabled = false;
            }
        }
    }

}

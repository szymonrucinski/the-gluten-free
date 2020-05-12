using System.Collections;
using UnityEngine;
using UnityEngine.XR;

public class ScannerController : MonoBehaviour
{

    public LineRenderer laserLine;

    public float lineWidth = 0.05f;
    public float length = 1000.0f;

    private InputDevice device;

    private bool supportsTrigger;
    private bool supportsRotation;

    private IEnumerator laserCoroutine;
    private SimpleSmoothMouseLook ssml;

    void Start()
    {
        device = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        laserLine.startWidth = lineWidth;
        laserLine.endWidth = lineWidth;
        laserLine.enabled = false;
        ssml = GetComponent<SimpleSmoothMouseLook>();

        //disable SimpleSmoothMouse Script if a VR device with rotation capability was found
        if (device.TryGetFeatureValue(CommonUsages.deviceRotation, out var rot))
        {
            ssml.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.Instance.currentGameState != GameState.InGame) return;

        supportsTrigger = device.TryGetFeatureValue(CommonUsages.triggerButton, out var shooting);
        supportsRotation = device.TryGetFeatureValue(CommonUsages.deviceRotation, out var deviceRotation);

        if (supportsRotation)
        {
            ssml.enabled = false;
            this.gameObject.transform.rotation = deviceRotation;
        }

        if (!supportsTrigger && Input.GetButton("Fire1"))
        {
            Shoot();
        }
        else if (shooting)
        {
            Shoot();
        }
    }


    private void Shoot()
    {
        var pos = transform.position;
        laserLine.SetPosition(0, pos);
        laserLine.SetPosition(1, transform.TransformDirection(Vector3.forward) * length);

        laserCoroutine = HideLaser(0.1f);
        laserLine.enabled = true;

        StartCoroutine(laserCoroutine);

        if (Physics.SphereCast(pos, lineWidth, transform.forward, out var HitInfo, float.PositiveInfinity))
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

    IEnumerator HideLaser(float time)
    {
        yield return new WaitForSeconds(time);
        laserLine.enabled = false;
    }
}
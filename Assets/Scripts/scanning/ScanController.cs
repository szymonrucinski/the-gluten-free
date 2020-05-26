using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using UnityEngine;

using UnityEngine.XR;

public class ScanController : MonoBehaviour
{
    public static ScanController Instance;
    private const float dropSpeedMultiplier = 0.01f;
    private readonly Vector3 bagDisplacementVector = new Vector3(-0.3f, -0.5f, -0.3f);

    private InputDevice device;
    private bool shooting;

    private RaycastHit rayHit;
    private LineRenderer lineRenderer;
    private Ray ray;
    public float rayLength;
    public HashSet<string> shoppingList;        //Didn't know how we would assign it, just left it as public
    public Queue<GameObject> scannedObjects;
    private ScoreAction scoreAction;
    public GameObject correctScanFX;
    public GameObject wrongScanFX;
    private GameObject toDrop;
    private GameObject sphere;
    private GameObject cube;
    private GameObject bag;
    private bool hitBlocked;
    private bool canDropToBag;
    private float timeToNext;

    public AudioClip shootingSound;
    public AudioClip smoke;
    private AudioSource audio;



    void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        shoppingList = new HashSet<string>();
        shoppingList.Add("avocado");
        scoreAction = ScoreController.Instance;
        cube = this.transform.GetChild(0).gameObject;
        bag = GameObject.Find("Bag");
        sphere = GameObject.Find("ScannerLaserSphere");
        canDropToBag = true;
        timeToNext = 0.0f;
        toDrop = null;
        lineRenderer = transform.GetChild(0).GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        sphere.SetActive(false);
        scannedObjects = new Queue<GameObject>();
        audio = GetComponent<AudioSource>();

        device = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

    }

    void LateUpdate()
    {
        lineRenderer.enabled = false;
        sphere.SetActive(false);
        if(shooting || Input.GetMouseButton(0))
        {
            lineRenderer.enabled = true;
            sphere.SetActive(true);
            lineRenderer.SetPosition(0, cube.transform.position);
            lineRenderer.SetPosition(1, cube.transform.forward * rayLength);
            sphere.transform.position = lineRenderer.GetPosition(1);

        }
    }

    // Update is called once per frame
    void Update()
    {

        //Positio
        UnityEngine.Debug.Log("CUBE "+cube.transform.position);
        UnityEngine.Debug.Log("CUBE LOCAL" + cube.transform.localPosition);
        UnityEngine.Debug.Log("SCANNER" + transform.position);


        if (scannedObjects.Count != 0 && canDropToBag)
        {
            canDropToBag = false;
            timeToNext = 0.7f;
            toDrop = scannedObjects.Dequeue();
            //toDrop.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane)) + Camera.main.transform.forward * 0.4f;
            toDrop.SetActive(true);
            Destroy(toDrop, 0.00f);
        }
        if (!canDropToBag)
        {
            if (timeToNext <= 0.1f)
            {
                canDropToBag = true;
            }
            else
            {
                timeToNext -= Time.deltaTime;
                toDrop.transform.position += (bag.transform.position + (bag.transform.up * 0.3f) - (bag.transform.right * 0.3f) - (bag.transform.forward * 0.1f) - toDrop.transform.position) * dropSpeedMultiplier;
            }
        }

        device.TryGetFeatureValue(CommonUsages.triggerButton, out shooting);

        if (Physics.Raycast(cube.transform.position, cube.transform.forward, out rayHit, rayLength) && (shooting || Input.GetMouseButton(0)))
        {
            GameObject target = rayHit.collider.gameObject;
            if (target.tag != "scanned")
            {
                audio.PlayOneShot(shootingSound);

                if (target.tag == "good")
                {
                    Instantiate(correctScanFX, target.transform.position, Quaternion.identity);
                    target.tag = "scanned";
                    scannedObjects.Enqueue(target);
                    target.SetActive(false);
                    if (shoppingList.Contains(target.name))
                    {
                        scoreAction.scoreAction(true, true, target.transform.position);
                    }
                    else
                    {
                        scoreAction.scoreAction(true, false, target.transform.position);
                    }
                }
                else if (target.tag == "bad")
                {
                    Instantiate(wrongScanFX, target.transform.position, Quaternion.identity);
                    audio.PlayOneShot(smoke);
                    scoreAction.scoreAction(false, false, target.transform.position);
                    Destroy(target);
                }
            }
        }
    }
}

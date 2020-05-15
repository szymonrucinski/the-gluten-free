using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using UnityEngine;

public class ScanController : MonoBehaviour
{
    public static ScanController Instance;
    private const float dropSpeedMultiplier = 0.04f;
    private readonly Vector3 bagDisplacementVector = new Vector3(-0.3f, -0.5f, -0.3f);

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
        scoreAction = ScoreController.Instance;
        cube = this.transform.GetChild(0).gameObject;
        bag = GameObject.Find("Bag");
        sphere = GameObject.Find("Sphere");
        canDropToBag = true;
        timeToNext = 0.0f;
        toDrop = null;
        lineRenderer = transform.GetChild(0).GetComponent<LineRenderer>();
        scannedObjects = new Queue<GameObject>();
        audio = GetComponent<AudioSource>();

    }

    void LateUpdate()
    {
        lineRenderer.SetPosition(0, cube.transform.position);
        lineRenderer.SetPosition(1, cube.transform.forward * rayLength);
        sphere.transform.position = lineRenderer.GetPosition(1);
    }

    // Update is called once per frame
    void Update()
    {
        if(scannedObjects.Count != 0 && canDropToBag)
        {
            canDropToBag = false;
            timeToNext = 0.7f;
            toDrop = scannedObjects.Dequeue();  
            toDrop.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane)) + Camera.main.transform.forward * 0.4f;
            toDrop.SetActive(true);
            Destroy(toDrop, 0.7f);
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


        if (Physics.Raycast(cube.transform.position, cube.transform.forward, out rayHit, rayLength))
        {
            GameObject target = rayHit.collider.gameObject;
            if (target.tag != "scanned")
            {
                audio.PlayOneShot(shootingSound);

                if (target.tag == "good")
                {
                    target.tag = "scanned";
                    scannedObjects.Enqueue(target);
                    target.SetActive(false);
                    /*if (shoppingList.Contains(target.name))
                    {
                        scoreAction.scoreAction(true, true, target.transform.position);
                    }
                    else
                    {
                        scoreAction.scoreAction(true, false, target.transform.position);
                    }*/
                    Instantiate(correctScanFX, target.transform.position, Quaternion.identity);
                }
                else if (target.tag == "bad")
                {
                    audio.PlayOneShot(smoke);
                    //scoreAction.scoreAction(false, false, target.transform.position);
                    Destroy(target);
                    Instantiate(wrongScanFX, target.transform.position, Quaternion.identity);
                }
            }
        }
    }
}

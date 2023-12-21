using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateSc : MonoBehaviour
{
    GameManager gameManager;
    public bool isDoorAvailable = false;
    public float platformSpeed = 1f;
    public float laserSpeed = 150f;
    public GameObject particle, lockObj;
    public GameObject wallColliders = null;
    public Material greenMat, redMat;

    bool isPlatformMoving = false;
    public bool isLasersOpening = false;
    bool isUsed = false;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.AddToGates(GetComponent<GateSc>());
        if (gameObject.CompareTag("Platform"))
        {
            transform.Find("PlatformObj").Find("Base").Find("ColorizedPart").GetComponent<MeshRenderer>().material = redMat;
            transform.Find("Start").GetComponent<MeshRenderer>().enabled = false;
            transform.Find("End").GetComponent<MeshRenderer>().enabled = false;
            transform.Find("PlatformObj").localPosition = new Vector3(transform.Find("Start").localPosition.x, transform.Find("PlatformObj").localPosition.y, transform.Find("Start").localPosition.z);
        }
    }

    private void FixedUpdate()
    {
        if (isPlatformMoving)
        {
            Vector3 targetPoint = new Vector3(transform.Find("End").localPosition.x, transform.Find("PlatformObj").localPosition.y, transform.Find("End").localPosition.z);
            transform.Find("PlatformObj").localPosition = Vector3.MoveTowards(transform.Find("PlatformObj").localPosition, targetPoint, platformSpeed * Time.deltaTime);
            if(transform.Find("PlatformObj").localPosition == targetPoint)
            {
                isPlatformMoving = false;
                gameManager.UnloadPlayerFromPlatform();
                SetWalls(true);
            }
        }

        if(isLasersOpening)
        {
            float key = 0;
            foreach (Transform laser in transform.Find("Lasers"))
            {
                key = laser.GetComponent<SkinnedMeshRenderer>().GetBlendShapeWeight(0) + laserSpeed * Time.deltaTime;
                laser.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, key);
            }
            if(key >=100)
            {
                foreach (Transform laser in transform.Find("Lasers"))
                {
                    key = 100;
                    laser.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, key);
                }
                isLasersOpening = false;
                GetComponent<BoxCollider>().enabled = !isDoorAvailable;
                transform.Find("ColorizedPart").GetComponent<MeshRenderer>().material = greenMat;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && (gameObject.CompareTag("Gate") || gameObject.CompareTag("LaserGate")))
        {
            if(isDoorAvailable)
            {
                UnlockGate();
            }
            else
            {
                gameManager.LockedTx();
            }
        }
    }
    public void SetGateAvailable(bool keyStatu)
    {
        isDoorAvailable = keyStatu;
        //GetComponent<BoxCollider>().isTrigger = isDoorAvailable;
        //particle.SetActive(isDoorAvailable);
    }

    public void UnlockGate()
    {

        if (gameObject.CompareTag("Gate"))
        {
            GetComponent<BoxCollider>().enabled = !isDoorAvailable;
            lockObj.SetActive(!isDoorAvailable);
        }
        else if (gameObject.CompareTag("Platform") && isDoorAvailable)
        {
            transform.Find("PlatformObj").GetComponent<BoxCollider>().isTrigger = true;
            transform.Find("PlatformObj").Find("Base").Find("ColorizedPart").GetComponent<MeshRenderer>().material = greenMat;
        }
        else if (gameObject.CompareTag("LaserGate"))
        {
            OpenLasers();
        }
    }

    public void LoadThePlayer()
    {
        if (!isUsed)
        {
            isUsed = true;
            gameManager.LoadPlayerToPlatform(gameObject);
        }
    }

    public void MoveThePlatform()
    {
        isPlatformMoving = true;
        SetWalls(false);
    }

    public void SetWalls(bool walls)
    {
        wallColliders.SetActive(walls);
    }

    public void OpenLasers()
    {
        isLasersOpening = true;
    }
}

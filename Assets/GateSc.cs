using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateSc : MonoBehaviour
{
    GameManager gameManager;
    public bool isDoorAvailable = false;
    public float platformSpeed = 1f;
    public GameObject particle, lockObj;
    public GameObject wallColliders = null;

    bool isPlatformMoving = false;
    bool isUsed = false;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.AddToGates(GetComponent<GateSc>());
        transform.Find("Start").GetComponent<MeshRenderer>().enabled = false;
        transform.Find("End").GetComponent<MeshRenderer>().enabled = false;
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
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && gameObject.CompareTag("Gate"))
        {
            gameManager.LockedTx();
        }
    }
    public void SetGateAvailable(bool keyStatu)
    {
        isDoorAvailable = keyStatu;

        if (gameObject.CompareTag("Gate"))
        {
            GetComponent<BoxCollider>().enabled = !isDoorAvailable;
            lockObj.SetActive(!isDoorAvailable);
        }
        else if (gameObject.CompareTag("Platform"))
        {
            transform.Find("PlatformObj").GetComponent<BoxCollider>().enabled = true;
        }
        //GetComponent<BoxCollider>().isTrigger = isDoorAvailable;
        //particle.SetActive(isDoorAvailable);
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
}

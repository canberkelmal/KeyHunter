using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;

    public Vector3 cameraOffset = Vector3.zero;
    public float t = 0f;
    public float shakePower = 0f;
    public float shakeTime = 0f;
    public float shakeFreq = 0f;

    private float startRotationY;
    bool isShaking = false;
    float tempPower = 0f;

    public void SetPlayerOffset()
    {
        cameraOffset = transform.position - player.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.position + cameraOffset;

        if(isShaking )
        {
            if (shakeTime > 0)
            {
                // Z rotasyonunda shake effecti olu�turmak i�in sine fonksiyonunu kullanarak de�i�imi hesapla
                float yRotation = Mathf.Sin(Time.time * shakeFreq) * tempPower;

                // Kameran�n Z rotasyonunu belirtilen aral�kta g�ncelle
                transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, startRotationY + yRotation, transform.localRotation.eulerAngles.z);

                // Shake effectinin s�resini azalt
                shakeTime -= Time.deltaTime;
            }
            else
            {
                isShaking = false;
                // Shake s�resi bitti�inde kameran�n rotasyonunu s�f�rla
                transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, startRotationY, transform.localRotation.eulerAngles.z);
            }
        }
    }

    public void ShakeCam()
    {
        shakePower *= -1;
        tempPower = shakePower;
        isShaking = true;
        shakeTime = t;
    }

    public void ShakeCamWithPower(float powerMultiplier)
    {
        shakePower *= -1;
        tempPower = shakePower * powerMultiplier;
        isShaking = true;
        shakeTime = t;
    }
    /*public void ShakeCam(float power, float time, float freq)
    {
        shakePower = power;
        shakeTime = time;
        shakeFreq = freq;
        startRotationZ = transform.localRotation.eulerAngles.z; // Kameran�n ba�lang�� Z rotasyon de�eri
    }*/
}

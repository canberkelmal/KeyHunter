using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarSc : MonoBehaviour
{
    public float speed = 1f;
    public float glowSpeed = 3f;
    public float showTime = 2f;
    public float fadeTime = 0.5f;

    Image fillAmountUI;
    Image glow;
    float fillAmount = 0f;
    float targetFill = 0f;
    bool repeatingAnim = false;
    bool repeatingGlow = false;
    Transform camTransform, playerHealthBar;
    bool healthBarShowing = false;
    
    // Start is called before the first frame update
    void Awake()
    {        
        fillAmountUI = transform.Find("Health Bar Fill").GetComponent<Image>();
        glow = transform.Find("Glow").GetComponent<Image>();
        camTransform = Camera.main.transform;
        playerHealthBar = GameObject.Find("Player").transform.Find("Canvas").Find("HealthBarCanvas");
        Invoke("BarFadeOut", showTime);
    }

    private void Update()
    {
        if (transform.parent.parent.CompareTag("Player"))
        {
            transform.LookAt(camTransform.position);
        }
        else
        {
            transform.rotation = playerHealthBar.rotation;
        }
    }

    public void BarFadeIn()
    {
        healthBarShowing = true;
        GetComponent<CanvasGroup>().alpha = 0f;
        GetComponent<CanvasGroup>().DOFade(1, fadeTime);
        Invoke("BarFadeOut", fadeTime + showTime);
    }

    public void BarFadeOut()
    {
        healthBarShowing = false;
        GetComponent<CanvasGroup>().alpha = 1f;
        GetComponent<CanvasGroup>().DOFade(0, fadeTime);
    }

    public void SetFillColor(Color clr)
    {
        fillAmountUI.color = clr;
    }

    public void SetFillAmountDirect(float amount)
    {
        fillAmount = amount;
        fillAmountUI.fillAmount = fillAmount;
    }

    public void SetFillAmount(float amount, bool glowAnim)
    {
        //Debug.Log("SettingBorder to :" + amount);
        targetFill = amount;
        if (!healthBarShowing)
        {
            BarFadeIn();
        }


        if (!repeatingAnim)
        {
            repeatingAnim = true;
            CancelInvoke("FillBorderAnim");
            InvokeRepeating("FillBorderAnim", 0, Time.fixedDeltaTime);
        }
        if (glowAnim)
        {
            Color glowColor = targetFill > fillAmount ? Color.green : Color.red;
            glowColor.a = 0;
            glow.color = glowColor;

            if (!repeatingGlow)
            {
                repeatingGlow = true;
                InvokeRepeating("FillGlowAnimUp", 0, Time.fixedDeltaTime);
            }
            else
            {
                CancelInvoke("FillGlowAnimUp");
                CancelInvoke("FillGlowAnimDown");
                InvokeRepeating("FillGlowAnimUp", 0, Time.fixedDeltaTime);
            }
        }
    }

    void FillBorderAnim()
    {
        fillAmount = Mathf.MoveTowards(fillAmount, targetFill, speed * Time.deltaTime);
        fillAmountUI.fillAmount = fillAmount;
        

        if (fillAmount == targetFill)
        {
            repeatingAnim = false;
            CancelInvoke("FillBorderAnim");
        }
    }

    void FillGlowAnimUp()
    {
        Color glowColor = glow.color;
        glowColor.a = Mathf.MoveTowards(glowColor.a, 1, glowSpeed * Time.deltaTime);
        glow.color = glowColor;

        if(glowColor.a >=1)
        {
            CancelInvoke("FillGlowAnimUp");
            InvokeRepeating("FillGlowAnimDown", 0, Time.fixedDeltaTime);
        }
    }
    void FillGlowAnimDown()
    {
        Color glowColor = glow.color;
        glowColor.a = Mathf.MoveTowards(glowColor.a, 0, glowSpeed * Time.deltaTime);
        glow.color = glowColor;

        if (glowColor.a <= 0)
        {
            repeatingGlow = false;
            CancelInvoke("FillGlowAnimDown");
        }
    }
}

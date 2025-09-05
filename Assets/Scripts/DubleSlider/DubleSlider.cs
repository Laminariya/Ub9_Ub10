using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class DubleSlider : MonoBehaviour
{

    public Image LeftImage;
    public Image RightImage;
    public Slider LeftSlider;
    public Slider RightSlider;

    public float LeftCount;
    public float RightCount;

    public Action Action;
    
    void Start()
    {
        Init();
        
    }

    void Update()
    {
        LeftCount = LeftSlider.value;
        RightCount = RightSlider.value;
    }

    public void LeftDelta()
    {
        if (RightSlider.value - LeftSlider.value < 0.048f)
        {
            LeftSlider.value = RightSlider.value - 0.048f;
        }

        LeftImage.fillAmount = 1f - LeftSlider.value;
        Action.Invoke();
    }

    public void RightDelta()
    {
        if (RightSlider.value - LeftSlider.value < 0.048f)
        {
            RightSlider.value = LeftSlider.value + 0.048f;
        }

        RightImage.fillAmount = RightSlider.value;
        Action.Invoke();
    }

    public void Init()
    {
        LeftImage.fillAmount = 1f;
        RightImage.fillAmount = 1f;
        LeftSlider.value = 0f;
        RightSlider.value = 1f;
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonFloorPrefab : MonoBehaviour
{

    private int _floor;
    private TMP_Text _text;
    public Button b_Click;
    
    public void Init()
    {
        _text = GetComponentInChildren<TMP_Text>();
        gameObject.SetActive(false);
    }

    public void Activate(int numberFloor)
    {
        gameObject.SetActive(true);
        _floor = numberFloor;
        _text.text = numberFloor.ToString();
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
        b_Click.onClick.RemoveAllListeners();
    }
    
    
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GalereyaButton : MonoBehaviour
{
    public List<Sprite> Sprites = new List<Sprite>();
    private KomplexClass _komplex;

    private void Start()
    {
        _komplex = GetComponentInParent<KomplexClass>(true);
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        _komplex.OpenGalereya(Sprites);
    }
}

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KomplexClass : MonoBehaviour
{

    public GameObject GalereyaPanel;
    public Button b_Back;
    public Button b_Left;
    public Button b_Right;
    public Sprite Active;
    public Sprite NotActive;
    public TMP_Text CountSprite;
    public Button b_BackMenu;

    private Image _imageGalereya;
    private List<Sprite> _sprites = new List<Sprite>();
    private int _numberSprite = 0;
    private UbManager _gameManager;

    public void Init(UbManager manager)
    {
        _gameManager = manager;
        GalereyaPanel.SetActive(false);
        _imageGalereya = GalereyaPanel.GetComponent<Image>();
        b_Left.onClick.AddListener(OnLeft);
        b_Right.onClick.AddListener(OnRight);
        b_Back.onClick.AddListener(CloseGalereya);
        b_BackMenu.onClick.AddListener(OnBackMenu);
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void OpenGalereya(List<Sprite> sprites)
    {
        _sprites = new List<Sprite>(sprites);
        GalereyaPanel.SetActive(true);
        _imageGalereya.sprite = _sprites[0];
        _numberSprite = 0;
        b_Left.image.sprite = NotActive;
        b_Right.image.sprite = Active;
        CountSprite.text = "1/" + _sprites.Count;
    }

    private void OnLeft()
    {
        if (_numberSprite > 0)
        {
            _numberSprite--;
            _imageGalereya.sprite = _sprites[_numberSprite];
            b_Right.image.sprite = Active;
            if (_numberSprite <= 0)
            {
                b_Left.image.sprite = NotActive;
            }
            CountSprite.text = (_numberSprite+1)+"/" + _sprites.Count;
        }
    }

    private void OnRight()
    {
        if (_numberSprite < _sprites.Count - 1)
        {
            _numberSprite++;
            _imageGalereya.sprite = _sprites[_numberSprite];
            b_Left.image.sprite = Active;
            if (_numberSprite >= _sprites.Count - 1)
            {
                b_Right.image.sprite = NotActive;
            }
            CountSprite.text = (_numberSprite+1)+"/" + _sprites.Count;
        }
    }

    private void CloseGalereya()
    {
        GalereyaPanel.SetActive(false);
    }

    private void OnBackMenu()
    {
        Close();
    }

}

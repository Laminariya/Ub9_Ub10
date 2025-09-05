using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputIpPanel : MonoBehaviour
{

    public Button FirstButton;
    public Button SecondButton;
    public GameObject InputPanel;
    public TMP_InputField InputIp;
    public Button SaveButton;
    public TMP_Text Version;

    public Button CloseButton;

    private GameManager _manager;
    private float _time;
    private int _countFirstClick;
    private int _countSecondClick;
    private const string _key = "SaveIp";

    public void Init(GameManager manager)
    {
        _manager = manager;
        FirstButton.onClick.AddListener(OnFirst);
        //SecondButton.onClick.AddListener(OnSecond);
        SaveButton.onClick.AddListener(OnSave);
        InputPanel.SetActive(false);
        CloseButton.onClick.AddListener(Close);
        Version.text = "v" + Application.version;
    }

    private void Close()
    {
        InputPanel.SetActive(false);
    }

    private void OnFirst()
    {
        if (Time.time - _time > 1f)
        {
            _countFirstClick = 1;
            _countSecondClick = 0;
        }
        else
        {
            _countFirstClick++;
           
        }

        if (_countFirstClick >= 3)
        {
            InputPanel.SetActive(true);
            InputIp.text = PlayerPrefs.GetString(_key);
        }

        _time = Time.time;
        Debug.Log(_countFirstClick);
    }

    private void OnSecond()
    {
        if (Time.time - _time < 1 && _countFirstClick == 2)
        {
            _countSecondClick++;
            _time = Time.time;
        }
        else
        {
            _countFirstClick = 0;
        }
        
        Debug.Log(_countSecondClick);
        if (_countSecondClick == 2 && _countFirstClick == 2 && Time.time - _time < 1)
        {
            //Открываем панель
            InputPanel.SetActive(true);
            InputIp.text = PlayerPrefs.GetString(_key);
        }
    }

    private void OnSave()
    {
        PlayerPrefs.SetString(_key, InputIp.text);
        InputPanel.SetActive(false);
        //перезагружаем сервер
        _manager.UdpClient.Reload();
    }
}

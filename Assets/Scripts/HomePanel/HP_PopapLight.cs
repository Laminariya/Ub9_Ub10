using UnityEngine;
using UnityEngine.UI;

public class HP_PopapLight : MonoBehaviour
{

    public Button b_Back;
    public Button b_9;
    public Button b_9A;
    public Button b_10;
    public Button b_School;
    public Button b_Office1;
    public Button b_Office2;

    private GameManager _manager;

    private bool _is9 = false;
    private bool _is9A = false;
    private bool _is10 = false;
    private bool _isSchool = false;
    private bool _isOffice1 = false;
    private bool _isOffice2 = false;
    

    public void Init(GameManager manager)
    {
        _manager = manager;
        b_Back.onClick.AddListener(OnBack);
        b_9.onClick.AddListener(On9);
        b_9A.onClick.AddListener(On9A);
        b_10.onClick.AddListener(On10);
        b_School.onClick.AddListener(OnSchool);
        b_Office1.onClick.AddListener(OnOffice1);
        b_Office2.onClick.AddListener(OnOffice2);
        Close();
    }

    public void Open()
    {
        gameObject.SetActive(true);
        _manager.SendMessageToServer.OffAll();
        _is9 = false;
        _is9A = false;
        _is10 = false;
        _isSchool = false;
        _isOffice1 = false;
        _isOffice2 = false;
    }
    
    public void Close()
    {
        gameObject.SetActive(false);
    }

    private void OnBack()
    {
        Close();
    }

    private void On9()
    {
        if (_is9)
        {
            _is9 = false;
            _manager.SendMessageToServer.OffAll();
        }
        else
        {
            _manager.SendMessageToServer.OnAll();
            _is9 = true;
        }
    }

    private void On9A()
    {
        if (_is9A)
        {
            _is9A = false;
            _manager.UdpClient.AddMessage("0180011400000000", "Off On9A"); //1
        }
        else
        {
            _manager.UdpClient.AddMessage("0180010A00000000", "On On9A"); //1
            _is9A = true;
        }
    }

    private void On10()
    {
        if (_is10)
        {
            _is10 = false;
            _manager.UdpClient.AddMessage("0180041400000000", "Off On10"); //4!
        }
        else
        {
            _is10 = true;
            _manager.UdpClient.AddMessage("0180040A00000000", "On On10"); //4!
        }
    }

    private void OnSchool()
    {
        if (_isSchool)
        {
            _manager.UdpClient.AddMessage("0180021400000000", "Off School"); //2
            _isSchool = false;
        }
        else
        {
            _manager.UdpClient.AddMessage("0180020A00000000", "On School"); //2
            _isSchool = true;
        }
    }

    private void OnOffice1()
    {
        if (_isOffice1)
        {
            _manager.UdpClient.AddMessage("0180031400000000", "Off Office"); //Общая подсветка 5
            _isOffice1 = false;
        }
        else
        {
            _manager.UdpClient.AddMessage("0180030A00000000", "On Office"); //Общая подсветка 5
            _isOffice1 = true;
        }
    }

    private void OnOffice2()
    {
        if (_isOffice1)
        {
            _manager.UdpClient.AddMessage("0180031400000000", "Off Office"); //Общая подсветка 5
            _isOffice1 = false;
        }
        else
        {
            _manager.UdpClient.AddMessage("0180030A00000000", "On Office"); //Общая подсветка 5
            _isOffice1 = true;
        }
    }

}

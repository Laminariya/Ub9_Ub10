using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class HP_VideoPanel : MonoBehaviour
{

    public Button b_Close;
    public Button b_Video1;
    public Button b_Video2;
    public Button b_Video3;

    public Sprite SpriteVideo1Active;
    public Sprite SpriteVideo1NotActive;
    public Sprite SpriteVideo2Active;
    public Sprite SpriteVideo2NotActive;
    public Sprite SpriteVideo3Active;
    public Sprite SpriteVideo3NotActive;

    public TMP_Text NameText;
    private const string Video1Text = "Продуктовый ролик";
    private const string Video2Text = "Короткие рекламные ролики";
    private const string Video3Text = "Вертикальный ролик-презентация";

    public VideoPlayer VideoPlayer1;
    public RawImage RawImage1;
    public Image ScrollBar1;
    
    public VideoPlayer VideoPlayer2;
    public RawImage RawImage2;
    public Image ScrollBar2;
    
    public VideoPlayer VideoPlayer3;
    public RawImage RawImage3;
    public Image ScrollBar3;
    
    private UbManager _manager;
    private Image _currentImage;
    private VideoPlayer _currentVideoPlayer;
    
    public void Init(UbManager manager)
    {
        _manager = manager;
        b_Video1.onClick.AddListener(OnVideo1);
        b_Video2.onClick.AddListener(OnVideo2);
        b_Video3.onClick.AddListener(OnVideo3);
        b_Close.onClick.AddListener(Close);
        _currentVideoPlayer = VideoPlayer1;
        Close();
    }

    private void Update()
    {
        if (_currentVideoPlayer!=null && _currentVideoPlayer.isPlaying)
        {
            ChangeScroll();
        }
    }

    public void Open()
    {
        gameObject.SetActive(true);
        OnVideo1();
    }

    private void Close()
    {
        VideoPlayer1.Stop();
        VideoPlayer2.Stop();
        VideoPlayer3.Stop();
        gameObject.SetActive(false);
    }

    private void OnVideo1()
    {
        RawImage2.gameObject.SetActive(false);
        RawImage3.gameObject.SetActive(false);
        RawImage1.gameObject.SetActive(true);
        VideoPlayer1.frame = 0;
        VideoPlayer1.Play();
        VideoPlayer2.Stop();
        VideoPlayer3.Stop();
        _currentImage = ScrollBar1;
        _currentVideoPlayer = VideoPlayer1;
        b_Video1.image.sprite = SpriteVideo1Active;
        b_Video2.image.sprite = SpriteVideo2NotActive;
        b_Video3.image.sprite = SpriteVideo3NotActive;
        NameText.text = Video1Text;
    }
    
    private void OnVideo2()
    {
        RawImage1.gameObject.SetActive(false);
        RawImage3.gameObject.SetActive(false);
        RawImage2.gameObject.SetActive(true);
        VideoPlayer2.frame = 0;
        VideoPlayer2.Play();
        VideoPlayer1.Stop();
        VideoPlayer3.Stop();
        _currentImage = ScrollBar2;
        _currentVideoPlayer = VideoPlayer2;
        b_Video1.image.sprite = SpriteVideo1NotActive;
        b_Video2.image.sprite = SpriteVideo2Active;
        b_Video3.image.sprite = SpriteVideo3NotActive;
        NameText.text = Video2Text;
    }
    
    private void OnVideo3()
    {
        RawImage1.gameObject.SetActive(false);
        RawImage2.gameObject.SetActive(false);
        RawImage3.gameObject.SetActive(true);
        VideoPlayer3.frame = 0;
        VideoPlayer3.Play();
        VideoPlayer2.Stop();
        VideoPlayer1.Stop();
        _currentImage = ScrollBar3;
        _currentVideoPlayer = VideoPlayer3;
        b_Video1.image.sprite = SpriteVideo1NotActive;
        b_Video2.image.sprite = SpriteVideo2NotActive;
        b_Video3.image.sprite = SpriteVideo3Active;
        NameText.text = Video3Text;
    }

    private void ChangeScroll()
    {
        var delta = _currentVideoPlayer.frame / (float)_currentVideoPlayer.frameCount;
        _currentImage.fillAmount = delta;
    }

}

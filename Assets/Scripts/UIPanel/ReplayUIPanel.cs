using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ReplayUI面板类
/// </summary>
public class ReplayUIPanel : MonoBehaviour
{
    
    [Header("LeftButton")]
    [SerializeField]
    private Button saveButton; 
    
    [SerializeField]
    private Button loadButton;
    
    [SerializeField]
    private Button restButton;

    [Header("UpEvent")]
    [SerializeField]
    private ExtendedSlider slider;
    
    [SerializeField]
    private Button playButton;
    [SerializeField]
    private Sprite playImg;
    [SerializeField]
    private Sprite pauseImg;

    [SerializeField]
    private GameObject Mask;
    
    /// <summary>
    /// 添加按钮事件以及silder事件
    /// </summary>
    void Start()
    {
        saveButton.onClick.AddListener(() =>
        {
            ReplayManager.GetInstance().SaveToJson();
        });
        
        loadButton.onClick.AddListener(() => 
        {
            ReplayManager.GetInstance().StartReplay();
            slider.interactable = true;
            slider.maxValue = ReplayManager.GetInstance().GetLastFrame();

            playButton.image.sprite = ReplayManager.GetInstance().isPlay ? playImg : pauseImg;
            playButton.interactable = true;
            Mask.SetActive(true);
        });
        
        restButton.onClick.AddListener(() =>
        {
            ReplayManager.GetInstance().Rest();
            slider.interactable = false;
            slider.value = 0;
            
            playButton.image.sprite = pauseImg;
            playButton.interactable = false;
            Mask.SetActive(false);
        });

        playButton.onClick.AddListener(() =>
        {
            ReplayManager.GetInstance().isPlay = !ReplayManager.GetInstance().isPlay;
            playButton.image.sprite = ReplayManager.GetInstance().isPlay ? playImg : pauseImg;
        });
        
        
        slider.pointerDown.AddListener(()=>
        {
            ReplayManager.GetInstance().isPlay = false;
            playButton.image.sprite = pauseImg;
        });
        
        slider.onValueChanged.AddListener((val) =>
        {
            //当player为false的时候，数据随UI变化
            if (!ReplayManager.GetInstance().isPlay)
                ReplayManager.GetInstance().curFrame = (int)val;
        });
        
    }
    
    /// <summary>
    /// 当player为true时，UI随数据更新
    /// </summary>
    private void Update()
    {
        if (ReplayManager.GetInstance().isPlay)
        {
            slider.value = ReplayManager.GetInstance().curFrame;
        }
    }
}

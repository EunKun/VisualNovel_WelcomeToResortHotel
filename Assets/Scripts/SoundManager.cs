using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
    
public class SoundManager : MonoBehaviour {

    public static SoundManager ins;

    public GameObject mainObj;
    public Button btnCancel;
    public Button btnComfirm;
    public Button btnBack;

    float volumeDefault = 0.3f;

    [System.Serializable]
    public class SoundController
    {
        public AudioSource bgm;
        public AudioClip[] bgmSource;

        [Space]

        public AudioSource se;
        public AudioClip[] seSource;
    }
    public SoundController soundController;


    [System.Serializable]
    public class OptionPanel
    {
        public Slider bgmSlider;
        [Range(0, 1)]
        public float bgmValue;
        public Text bgmValueText;
        public GameObject bgmMutePanel;
        public bool isBGM_Mute;

        [Space]

        public Slider seSlider;
        [Range(0, 1)]
        public float seValue;
        public Text seValueText;
        public GameObject seMutePanel;
        public bool isSE_Mute;
        public Color[] color;
    }
    public OptionPanel option;

    private void Awake()
    {
        if (ins == null)
            ins = this;
        else if (ins != null)
            Destroy(this);

        soundController.bgmSource = Resources.LoadAll<AudioClip>("Music");
        soundController.seSource = Resources.LoadAll<AudioClip>("SE");
    }

    private void Start()
    {
        option.bgmSlider.value = PlayerPrefs.GetFloat("bgmVolume", volumeDefault);
        option.seSlider.value = PlayerPrefs.GetFloat("seVolumeValue", volumeDefault);
    }

    void Save()
    {
        PlayerPrefs.SetFloat("bgmVolume", option.bgmSlider.value);
        PlayerPrefs.SetFloat("seVolumeValue", option.seSlider.value);
    }

    // Update is called once per frame
    void Update () {
        //옵션창 텍스트값
        if (mainObj.activeSelf)
        {
            if (!option.isBGM_Mute)
            {
                if (Input.GetMouseButtonUp(0) && ViewValue(option.bgmSlider.value) != ViewValue(option.bgmValue))
                    Save();

                option.bgmValueText.text = ViewValue(option.bgmSlider.value);
            }
            else
            {
                option.bgmValueText.text = ViewValue(option.bgmValue);
                option.bgmSlider.value = option.bgmValue;
            }

            if (!option.isSE_Mute)
            {
                if (Input.GetMouseButtonUp(0) && ViewValue(option.seSlider.value) != ViewValue(option.seValue))
                    Save();

                option.seValueText.text = ViewValue(option.seSlider.value);
            }
            else
            {
                option.seValueText.text = ViewValue(option.seValue);
                option.seSlider.value = option.seValue;
            }
        }

        soundController.bgm.volume = option.bgmSlider.value;

        //옵션창 여는 클래스
        //OptionPanelKeyFuntion();
    }

    string ViewValue(float _value)
    {
        return string.Format("{0:F0}", (_value * 100f));
    }

    /// <summary>
    /// isActiveTitle 타이틀 돌아가기 활성화 결정
    /// isActiveExit 종료 버튼 활성활 결정
    /// isActiveComfirm 확인 및 [예] 버튼 활성화 결정
    /// </summary>
    /// <param name="optionSettingBtn"></param>
    /// <param name="exitBtn"></param>
    void OptionBtnPattern(bool isActiveTitle, bool isActiveExit, bool isActiveComfirm)
    {
        btnBack.gameObject.SetActive(isActiveTitle);
        btnCancel.gameObject.SetActive(isActiveExit);
        btnComfirm.gameObject.SetActive(isActiveComfirm);
    }
    /// <summary>
    /// 물리키 발동시 옵션창을 끈다
    /// </summary>
    void OptionPanelKeyFuntion()
    {
        /*
    //옵션키
    if (Input.GetKeyDown(KeyCode.Escape))
    {
        //옵션 및 게임 종료 화면 버튼 교체
        switch (GameManager.ins.progress)
        {
            case GameManager.Progress.title:
                OptionBtnPattern(false, true, true);
                break;
            case GameManager.Progress.play:
                OptionBtnPattern(true, true, false);
                break;
        }
        //종료창 타이틀 돌아가기 버튼. 현재는 안쓰기에 항상 꺼둠
        btnBack.gameObject.SetActive(false);
    }
        */

    }

    //음소거 버튼
    public void Btn_Mute_Icon(bool seBGM)
    {
        option.seValue = option.seSlider.value;
        option.bgmValue = option.bgmSlider.value;

        if (seBGM)
        {
            option.isSE_Mute = !option.isSE_Mute;
            SoundMute(option.isSE_Mute, option.seMutePanel, option.seValue, option.seSlider.value);
            //Mute_Support(option.isSE_Mute, soundController.se.transform.gameObject);
            soundController.se.clip = null;
        }

        if (!seBGM)
        {
            option.isBGM_Mute = !option.isBGM_Mute;
            SoundMute(option.isBGM_Mute, option.bgmMutePanel, option.bgmValue, option.bgmSlider.value);
            //Mute_Support(option.isBGM_Mute, soundController.bgm.transform.gameObject);
        }
    }
    /// <summary>
    /// mute : SE / BGM을 메뉴를 결정. bgmAndSE : mute bool에 따라 결정됨. 
    /// </summary>
    /// <param name="mute"></param>
    /// <param name="bgmAndSE"></param>
    /// <param name="yukariSpeedchBubbleFalse"></param>
    /// <param name="yukariSpeedchBubbleTrue"></param>
    void Mute_Support(bool mute, GameObject bgmAndSE)
    {
        if (mute)
            bgmAndSE.SetActive(false);
        else
            bgmAndSE.SetActive(true);
    }
    /// <summary>
    /// isMute가 true면 원래색 / false면 panel색을 반투명.
    /// soundValue 값을 value값으로 옮겨 음소거 시, 사운드 볼륨조절을 하지 못하게 함
    /// </summary>
    /// <param name="isMute"></param>
    /// <param name="panel"></param>
    /// <param name="soundValue"></param>
    /// <param name="value"></param>
    void SoundMute(bool isMute, GameObject panel, float soundValue, float value)
    {
        if (isMute)
            panel.GetComponent<Text>().color = option.color[0];
        else
            panel.GetComponent<Text>().color = option.color[1];

        soundValue = value;
    }
    
    //bgm 볼륨 조절
    public void Btn_OptionBGMValue(bool leftTrueRightFalse)
    {
        if (!option.isBGM_Mute && leftTrueRightFalse && option.bgmSlider.value > 0)
            option.bgmSlider.value -= 0.01f;
        else if (!option.isBGM_Mute && option.bgmSlider.value < 0)
            option.bgmSlider.value = 0;
        else if (!option.isBGM_Mute && !leftTrueRightFalse && option.bgmSlider.value < 0.99)
            option.bgmSlider.value += 0.01f;
    }
    //se 볼륨 조절
    public void Btn_OptionSEValue(bool leftTrueRightFalse)
    {
        if (!option.isSE_Mute && leftTrueRightFalse && option.seSlider.value > 0)
            option.seSlider.value -= 0.01f;
        else if (!option.isSE_Mute && option.seSlider.value < 0)
            option.seSlider.value = 0;
        else if (!option.isSE_Mute && !leftTrueRightFalse && option.seSlider.value < 0.99)
            option.seSlider.value += 0.01f;
    }

    /// <summary>
    /// 효과음을 재생하게 해줌. int로 효과음 번호지정
    /// </summary>
    /// <param name="soundEffectNumber"></param>
    public void SoundEffectPlay(int soundEffectNumber)
    {
        soundController.se.transform.gameObject.SetActive(false);
        soundController.se.clip = soundController.seSource[soundEffectNumber];
        soundController.se.transform.gameObject.SetActive(true);
    }

    /// <summary>
    /// 사운드을 재생하게 해줌. int로 효과음 번호지정
    /// </summary>
    /// <param name="soundEffectNumber"></param>
    public void BackGroundMusicPlay(int bgmNumber)
    {
        soundController.bgm.transform.gameObject.SetActive(false);
        soundController.bgm.clip = soundController.bgmSource[bgmNumber];
        soundController.bgm.transform.gameObject.SetActive(true);
    }
}

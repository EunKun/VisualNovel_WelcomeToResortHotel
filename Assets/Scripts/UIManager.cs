using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject mainTitleObj;
    public Button startBtn;
    public Button decisionPlayerNameBtn;
    public Button loadBtn;
    public Button optionBtn;
    public Button clearBonusBtn;

    [Header("게임종료패널")]
    public GameObject alretObj;
    public Text alretTitleText;
    public Text alretExplainText;
    public Button quitBtn;
    public Button titleBtn;
    readonly string[] quitTitle = { "앱 종료 확인", "타이틀 돌아감" };
    readonly string[] quitExplain = { "정말로 앱을 종료하시겠습니까?", "타이틀로 돌아가시겠습니까?"};

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            int _alertPanelCount = AlertPanelManager.alertObj.transform.childCount -1;
            //Debug.Log("경고창 수 : " + _alertPanelCount);

            for (int i = 0; i <= _alertPanelCount; i++)
            {
                GameObject _turnoffObj = AlertPanelManager.alertObj.transform.GetChild(i).gameObject;
                if (_turnoffObj.activeSelf)
                {
                    _turnoffObj.SetActive(false);
                    break;
                }
                else if(!_turnoffObj.activeSelf && i > 0)
                {
                    _turnoffObj.SetActive(true);
                    break;
                }
            }
        }
    }

    public void Quit(bool _quit)
    {
        if (_quit)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit() // 어플리케이션 종료
#endif
        }
        else
            alretObj.SetActive(false);    
    }

    public void Btn_CloseObj(string _name)
    {
        GameObject _temp = GameObject.Find(_name);
        _temp.SetActive(false);
    }

    public void Btn_BackTitle()
    {
        GameManager.ins.FirstStart();
        GameManager.ins.tm.FirstStart();
        FirstStart();
    }

    private void Start()
    {
        FirstStart();
    }

    void FirstStart()
    {
        if (alretObj.activeSelf)
            alretObj.SetActive(false);

        GameManager.ins.sm.Btn_Open();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogView : MonoBehaviour
{
    public static void MakeNormalModeLog(Transform _parent, string _script)
    {
        if (_parent.childCount > 100)
            Destroy(_parent.GetChild(0).gameObject);

        GameObject logObj = Instantiate(TextManager.logPrefab, _parent);

        var parent = _parent.GetComponent<RectTransform>();
        parent.sizeDelta = new Vector2(parent.sizeDelta.x, _parent.GetComponent<HorizontalOrVerticalLayoutGroup>().padding.top);
        float parentSizeY = (logObj.transform.GetComponent<RectTransform>().sizeDelta.y + _parent.GetComponent<HorizontalOrVerticalLayoutGroup>().spacing) * _parent.childCount;
        if (_parent.childCount != 0)
        {
            parent.sizeDelta = new Vector2(parent.sizeDelta.x, parentSizeY);
            _parent.parent.parent.GetComponent<ScrollRect>().verticalScrollbar.value = 0;
        }

        logObj.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = _script;
    }

    public static void LogClear()
    {
        if(TextManager.normalLogParentPos.childCount > 1)
        {
            foreach (GameObject _log in TextManager.normalLogParentPos.GetComponentsInChildren<GameObject>())
                Destroy(_log);
        }
    }

    public static void OpenLog()
    {
        if (GameManager.ins.state == GameManager.State.Log)
        {
            TextManager.mainLogObj.SetActive(false);
            GameManager.ins.state = GameManager.State.Play;
        }
        else
        {
            GameManager.ins.state = GameManager.State.Log;
            TextManager.mainLogObj.SetActive(true);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControl
{
    public static void LogCheck(GameObject _novel, GameObject _normal)
    {
        if(TextManager.isNovelScriptMode)
        {
            _novel.SetActive(true);
            _normal.SetActive(false);
        }
        else
        {
            _novel.SetActive(false);
            _normal.SetActive(true);
        }
    }
}

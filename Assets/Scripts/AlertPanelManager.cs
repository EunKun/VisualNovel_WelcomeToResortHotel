using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertPanelManager : MonoBehaviour
{
    public static GameObject alertObj;
    public static InputField playerNameField;
    public static GameObject playerNameInputFieldObj;

    // Start is called before the first frame update
    void Start()
    {
        playerNameField = GameObject.Find("PlayerNameInputField").GetComponent<InputField>();
        playerNameInputFieldObj = this.gameObject.transform.GetChild(0).gameObject;
        alertObj = this.gameObject;

        //Debug.Log(playerNameInputFieldObj.name);
        playerNameInputFieldObj.gameObject.SetActive(false);
    }
}

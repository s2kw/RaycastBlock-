using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonReaction : MonoBehaviour
{
    void Start()
    {
        var b = GetComponent<Button>();
        b.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        Debug.Log($"OnClick:{this.gameObject.name}",this);
    }

}

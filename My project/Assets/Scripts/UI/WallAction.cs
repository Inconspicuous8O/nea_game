using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WallAction : MonoBehaviour
{
    public Button corner;
    public Button regular;
    public Button T;
    public Button plus;


    public void ShowOptions()
    {
        corner.gameObject.SetActive(!corner.gameObject.activeSelf);
        ///corner.interactable = !corner.interactable;

        regular.gameObject.SetActive(!regular.gameObject.activeSelf);
        ///regular.interactable = !regular.interactable;

        T.gameObject.SetActive(!T.gameObject.activeSelf);
        ///T.interactable = !T.interactable;

        plus.gameObject.SetActive(!plus.gameObject.activeSelf);
        ///plus.interactable = !plus.interactable;
    }
}

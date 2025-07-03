using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class HyperLinkBehaviour : MonoBehaviour
{
    [SerializeField] string url;

    public void OpenURL()
    {
        Application.OpenURL(url);
    }
}

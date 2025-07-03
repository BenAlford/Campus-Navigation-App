using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsBehaviour : MonoBehaviour
{
    [SerializeField] GameObject showObject;
    MoveMap map;
    bool moveMapAfter = true;


    private void Start()
    {
        map = GameObject.FindGameObjectWithTag("map").GetComponent<MoveMap>();
    }

    public void Show()
    {
        moveMapAfter = map.canMove;
        map.canMove = false;
        showObject.SetActive(true);
    }

    public void Hide()
    {
        if (moveMapAfter)
            map.canMove = true;
        showObject.SetActive(false);
    }
}

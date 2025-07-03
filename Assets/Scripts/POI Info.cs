using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class POIInfo : MonoBehaviour
{
    Vector3 upPos = new Vector3(0, -1.05f, 0);
    Vector3 downPos;
    bool headingUp = false;
    bool headingDown = false;
    float moveTimer = 0;
    public float moveTime;
    public bool startUp = false;

    MoveMap map;

    // Start is called before the first frame update
    void Start()
    {
        map = GameObject.FindGameObjectWithTag("map").GetComponent<MoveMap>();
        downPos = transform.position;
        if (startUp)
        {
            GoUp();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (headingUp)
        {
            moveTimer += Time.deltaTime;
            transform.position = Vector3.Lerp(downPos, upPos, moveTimer / moveTime);
            if (transform.position ==  upPos)
            {
                headingUp = false;
                moveTimer = 0;
            }
        }
        else if (headingDown)
        {
            moveTimer += Time.deltaTime;
            transform.position = Vector3.Lerp(upPos, downPos, moveTimer / moveTime);
            if (transform.position == downPos)
            {
                headingDown = false;
                moveTimer = 0;
                map.canMove = true;
                if (startUp)
                {
                    GameObject.FindGameObjectWithTag("map").GetComponent<MoveMap>().FocusUser();
                }
            }
        }

    }

    public void GoUp()
    {
        headingUp = true;
        map.canMove = false;
    }

    public void GoDown()
    {
        headingDown = true;
    }
}

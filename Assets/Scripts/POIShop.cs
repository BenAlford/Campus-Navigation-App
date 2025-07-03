using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms.Impl;

public class POIShop : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI text;
    public string place_name;
    MoveMap map;
    [SerializeField] float showThreshold;

    GameObject display;

    bool show = true;
    bool changing = false;

    float changingTimer = 0.5f;

    [SerializeField] List<GameObject> showObjects;

    private void Start()
    {
        text.text = place_name;
        display = gameObject.transform.GetChild(0).gameObject;
        //display.SetActive(false);
        map = GameObject.FindGameObjectWithTag("map").GetComponent<MoveMap>();

    }

    private void Update()
    {
        if (changing)
        {
            changingTimer -= Time.deltaTime;
            if (changingTimer < 0)
            {
                changingTimer = 0;
                changing = false;
            }

            if (!show)
            {
                for (int i = 0; i < showObjects.Count; i++)
                {
                    Color colour = showObjects[i].GetComponent<SpriteRenderer>().color;
                    colour.a = changingTimer * 2;
                    showObjects[i].GetComponent<SpriteRenderer>().color = colour;
                }
                Color textColour = text.color;
                textColour.a = changingTimer * 2;
                text.color = textColour;
            }
            else
            {
                for (int i = 0; i < showObjects.Count; i++)
                {
                    Color colour = showObjects[i].GetComponent<SpriteRenderer>().color;
                    colour.a = 1 - (changingTimer * 2);
                    showObjects[i].GetComponent<SpriteRenderer>().color = colour;
                }
                Color textColour = text.color;
                textColour.a = 1 - (changingTimer * 2);
                text.color = textColour;
            }

            if (!changing)
            {
                changingTimer = 0.5f;
            }
        }
        else
        {
            if (!show && map.gameObject.transform.lossyScale.x > showThreshold)
            {
                show = true;
                changing = true;
            }
            else if (show && map.gameObject.transform.lossyScale.x < showThreshold)
            {
                show = false;
                changing = true;
            }
        }
    }
}

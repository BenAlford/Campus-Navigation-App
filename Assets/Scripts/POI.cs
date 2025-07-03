using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms.Impl;

public class POI : MonoBehaviour
{
    public POIInfo poi;
    GameObject ring;
    bool reached = false;

    [SerializeField] TextMeshProUGUI text;
    public string place_name;

    private void Start()
    {
        ring = GameObject.FindGameObjectWithTag("Radius");

    }

    private void OnMouseDown()
    {
        if (ring.GetComponent<CircleCollider2D>().OverlapPoint(transform.position))
        {
            poi.GoUp();
            if (!reached)
            {
                reached = true;
                for (int i = 0; i < transform.childCount; i++)
                {
                    if (transform.GetChild(i).GetComponent<SpriteRenderer>())
                        transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = !transform.GetChild(i).GetComponent<SpriteRenderer>().enabled;
                }
                text.text = place_name;
                text.gameObject.SetActive(true);
            }
        }
    }
}

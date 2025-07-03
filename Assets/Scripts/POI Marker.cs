using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class POIMarker : MonoBehaviour
{
    public GameObject poi;
    GameObject poiRef;
    public POIInfo poiInfo;

    [SerializeField] string place_name;
    // Start is called before the first frame update
    void Start()
    {
        poiRef = Instantiate(poi);
        poiRef.transform.position = transform.position;
        poiRef.GetComponent<POI>().poi = poiInfo;
        poiRef.GetComponent<POI>().place_name = place_name;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        poiRef.transform.position = transform.position;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationBehaviour : MonoBehaviour
{
    [SerializeField] GameObject map;
    [SerializeField] float tolerance;
    float lastAngle = 0;
    float targetAngle = 0;
    [SerializeField] float rotSpeed = 0.1f;

    float lastMapRot = 0;
    // Start is called before the first frame update
    void Start()
    {
        //Input.location.Start();
        Input.compass.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        //float mapRotChange = lastMapRot - map.transform.eulerAngles.z;
        //transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + mapRotChange);
        //targetAngle += mapRotChange;
        //targetAngle = targetAngle % 360;
        //lastMapRot = map.transform.eulerAngles.z;

        ////print(Input.compass.trueHeading);
        //float heading = Input.compass.trueHeading;
        //if (Mathf.Abs(heading - lastAngle) > tolerance && Mathf.Abs(heading - lastAngle) < 360 - tolerance)
        //{
        //    lastAngle = heading;
        //    targetAngle = 360-((lastAngle + map.transform.rotation.eulerAngles.z) % 360);
        //    print(targetAngle);
        //    //transform.rotation = Quaternion.Euler(0, 0, Input.compass.trueHeading + map.transform.rotation.eulerAngles.z);
        //}

        //float difference = transform.eulerAngles.z - targetAngle;
        //if (difference > 180)
        //{
        //    difference -= 360;
        //}
        //else if (difference < -180)
        //{
        //    difference += 360;
        //}
        ////print(difference);
        ////print(rotSpeed * Time.deltaTime);
        //if (Mathf.Abs(difference) < rotSpeed * Time.deltaTime)
        //{
        //    //print(1);
        //    transform.rotation = Quaternion.Euler(0, 0, targetAngle);
        //}
        //else if (difference > 0)
        //{
        //    transform.rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z - rotSpeed * Time.deltaTime);
        //}
        //else
        //{
        //    transform.rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z + rotSpeed * Time.deltaTime);
        //}

        targetAngle = (map.transform.eulerAngles.z - Input.compass.trueHeading) % 360;
        float difference = transform.eulerAngles.z - targetAngle;
        if (difference > 180)
        {
            difference -= 360;
        }
        else if (difference < -180)
        {
            difference += 360;
        }
        //print(difference);
        if (difference > 10 || difference < -10)
        {
            transform.rotation = Quaternion.Euler(0,0,transform.eulerAngles.z - difference * rotSpeed * Time.deltaTime);
        }
        //transform.rotation = Quaternion.Euler(0,0,map.transform.eulerAngles.z - Input.compass.trueHeading);
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class LocationTest : MonoBehaviour
{
    Vector2 start_loc = new Vector2();

    float bottom_lat = 51.500226963818704f;
    float top_lat = 51.504540381446304f;

    float left_long = -2.550023069321581f;
    float right_long = -2.5451315857846777f;

    [SerializeField] GameObject bottomright;
    [SerializeField] GameObject topleft;
    [SerializeField] GameObject bottomleft;
    [SerializeField] GameObject topright;

    [SerializeField] GameObject ring;

    IEnumerator Start()
    {
        // Check if the user has location service enabled.
        if (!Input.location.isEnabledByUser)
            Debug.Log("Location not enabled on device or app does not have permission to access location");

        // Starts the location service.
        Input.location.Start(1,1);

        // Waits until the location service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // If the service didn't initialize in 20 seconds this cancels location service use.
        if (maxWait < 1)
        {
            Debug.Log("Timed out");
            yield break;
        }

        // If the connection failed this cancels location service use.
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.LogError("Unable to determine device location");
            yield break;
        }
        else
        {
            // If the connection succeeded, this retrieves the device's current location and displays it in the Console window.
            Debug.Log("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
            start_loc = new Vector2(Input.location.lastData.latitude, Input.location.lastData.longitude);
        }

        // Stops the location service if there is no need to query location updates continuously.
        //Input.location.Stop();
    }

    private void LateUpdate()
    {
        Vector2 loc_new = new Vector2(Input.location.lastData.longitude, Input.location.lastData.latitude);

        float user_percent_x = (loc_new.x - left_long) / (right_long - left_long);
        //float user_x = topleft.transform.position.x + ((bottomright.transform.position.x - topleft.transform.position.x) * user_percent_x);
        //print(loc_new.x);

        float user_percent_y = (loc_new.y - bottom_lat) / (top_lat - bottom_lat);
        //float user_y = bottomright.transform.position.y + ((topleft.transform.position.y - bottomright.transform.position.y) * user_percent_y);

        //transform.position = new Vector3(user_x, user_y);

        Vector2 horizontal_position = topleft.transform.position + ((topright.transform.position - topleft.transform.position) * user_percent_x);
        Vector2 vertical_position = bottomleft.transform.position + ((topleft.transform.position - bottomleft.transform.position) * user_percent_y);

        Vector2 horizontal_vector = (topright.transform.position - topleft.transform.position).normalized;
        Vector2 vertical_vector = (bottomleft.transform.position - topleft.transform.position).normalized;


        float hori_m = (horizontal_vector.y / horizontal_vector.x);
        float vert_m = (vertical_vector.y / vertical_vector.x);

        if (hori_m == 0)
        {
            float user_x = topleft.transform.position.x + ((bottomright.transform.position.x - topleft.transform.position.x) * user_percent_x);
            float user_y = bottomright.transform.position.y + ((topleft.transform.position.y - bottomright.transform.position.y) * user_percent_y);
            transform.position = new Vector3(user_x, user_y);
        }
        else
        {

            float hori_c = vertical_position.y - (vertical_position.x * hori_m);
            float vert_c = horizontal_position.y - (horizontal_position.x * vert_m);

            // eq1: y = hori_m (x) + hori_c
            // eq2: y = vert_m (x) + vert_c

            // hori_m (x) + hori_c = vert_m (x) + vert_c
            // x(hori_m - vert_m) = vert_c - hori_c
            // x = (vert_c - hori_c) / (hori_m - vert_m)

            float x = (vert_c - hori_c) / (hori_m - vert_m);
            float y = (hori_m * x) + (hori_c);


            transform.position = new Vector3(x, y);

        }

        ring.transform.position = transform.position;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMap : MonoBehaviour
{
    Vector3 lastMousePos;
    bool drag = false;
    bool rotAndScale = false;
    float startDistance = 0;
    Vector3 startVector = Vector3.zero;
    public Camera cam;
    public GameObject centre;
    public BoxCollider2D bounds;

    bool focus = false;
    public bool canMove = true;
    Vector3 target_pos = Vector3.zero;
    Vector3 start_pos = Vector3.zero;
    float timer = 0;

    [SerializeField] GameObject ring;
    [SerializeField] float detailedMapThreshold;
    [SerializeField] GameObject detailedMap;
    [SerializeField] GameObject basicMap;

    bool detailed = true;
    bool changing = false;

    float changingTimer = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        float ringScale = transform.localScale.x * ring.GetComponent<RingBehaviour>().scale;
        ring.transform.localScale = new Vector3(ringScale, ringScale, ringScale);
    }

    // Update is called once per frame
    void Update()
    {
        MapVisuals();

        if (!canMove)
        {
            if (focus)
                FocusUserMovement(Time.deltaTime);
        }
        else
        {
            if (Input.touchCount == 1)
            {
                //rotAndScale = false;
                Vector3 mousePos = cam.ScreenToWorldPoint(Input.touches[0].position);
                if (drag)
                {
                    Vector3 current_pos = transform.position;
                    transform.position += mousePos - lastMousePos;

                    if (!bounds.OverlapPoint(new Vector2(0, 0)) && Vector2.Distance(new Vector2(0, 0), transform.position) > Vector2.Distance(new Vector2(0, 0), current_pos))
                    {
                        //print("not in");
                        transform.position = current_pos;
                    }
                }
                else
                {
                    drag = true;
                }
                lastMousePos = mousePos;
            }
            else if (Input.touchCount == 2)
            {
                Vector3 mousePos1 = cam.ScreenToWorldPoint(Input.touches[0].position);
                Vector3 mousePos2 = cam.ScreenToWorldPoint(Input.touches[1].position);
                if (rotAndScale)
                {
                    float newDistance = Vector3.Distance(mousePos1, mousePos2);
                    float scale = (newDistance / startDistance);
                    if (scale * transform.localScale.x > 3)
                    {
                        scale = 3 / transform.localScale.x;
                    }
                    if (scale * transform.localScale.x < 0.5f)
                    {
                        scale = 0.5f / transform.localScale.x;
                    }
                    centre.transform.position = (mousePos1 + mousePos2) / 2;
                    centre.transform.localScale = new Vector3(scale, scale, scale);

                    float ringScale = scale * transform.localScale.x * ring.GetComponent<RingBehaviour>().scale;
                    ring.transform.localScale = new Vector3(ringScale, ringScale, ringScale);

                    Vector3 newVector = (mousePos1 - mousePos2).normalized;
                    float angle = Vector3.Angle(startVector, newVector);
                    if (Vector3.Cross(startVector, newVector).z < 0)
                    {
                        angle = -angle;
                    }
                    //print(angle);
                    centre.transform.rotation = Quaternion.Euler(0, 0, angle);
                }
                else
                {
                    centre.transform.position = (mousePos1 + mousePos2) / 2;
                    transform.SetParent(centre.transform, true);
                    startDistance = Vector3.Distance(mousePos1, mousePos2);
                    startVector = (mousePos1 - mousePos2).normalized;
                    rotAndScale = true;
                }
            }
        }

        if (Input.touchCount != 2 && rotAndScale)
        {
            transform.SetParent(null, true);
            rotAndScale = false;
            centre.transform.localScale = new Vector3(1, 1, 1);
            //centre.transform.rotation = Quaternion.Euler(0, 0, 0);
            centre.transform.localEulerAngles = new Vector3(0, 0, 0);
        }

        if (Input.touchCount != 1 && drag)
        {
            drag = false;
        }
        //if (transform.position.magnitude > 10 * transform.localScale.x)
        //{
        //    transform.position = transform.position.normalized * (10 * transform.localScale.x);
        //}
    }

    public void FocusUser()
    {
        if (!focus)
        {
            focus = true;
            canMove = false;
            // we want to make the user's position 0
            GameObject user = GameObject.FindGameObjectWithTag("Player");
            target_pos = transform.position - user.transform.position;
            start_pos = transform.position;
            timer = 0;
        }
    }

    private void FocusUserMovement(float dt)
    {
        timer += dt;
        transform.position = Vector3.Lerp(start_pos, target_pos, timer);
        if (timer > 1)
        {
            focus = false;
            canMove = true;
        }
    }

    private void MapVisuals()
    {
        if (changing)
        {
            changingTimer -= Time.deltaTime;
            if (changingTimer < 0)
            {
                changingTimer = 0;
                changing = false;
            }

            if (detailed)
            {
                detailedMap.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1 - (changingTimer * 2));
                basicMap.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, (changingTimer * 2));
            }
            else
            {
                basicMap.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1 - (changingTimer * 2));
                detailedMap.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, (changingTimer * 2));
            }

            if (!changing)
            {
                changingTimer = 0.5f;
            }
        }
        else
        {
            if (!detailed && transform.lossyScale.x > detailedMapThreshold)
            {
                detailed = true;
                changing = true;
            }
            else if (detailed && transform.lossyScale.x < detailedMapThreshold)
            {
                detailed = false;
                changing = true;
            }
        }
    }
}

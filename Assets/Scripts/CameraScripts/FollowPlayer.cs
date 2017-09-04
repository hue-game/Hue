using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    public float smoothTimeX;
    public float smoothTimeY;

    private Vector2 velocity;

    void FixedUpdate()
    {
        float posXSmoothed = Mathf.SmoothDamp(transform.position.x, player.transform.position.x + 3f, ref velocity.x, smoothTimeX);
        float posYSmoothed = Mathf.SmoothDamp(transform.position.y, player.transform.position.y, ref velocity.y, smoothTimeY);

        //Apply the new smoothed position to the camera
        transform.position = new Vector3(posXSmoothed, posYSmoothed, transform.position.z);

        //Old CameraFollow without smoothing
        //transform.position = new Vector3(player.transform.position.x + 3f, player.transform.position.y, transform.position.z);
    }
}

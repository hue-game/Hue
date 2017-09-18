using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    public float smoothTimeX;
    public float smoothTimeY;
    public float offSetX;
    public float offSetY;

    private Vector2 velocity;

    void Start()
    {
        ResetCamera();
    }

    public void ResetCamera()
    {
        transform.position = new Vector3(player.transform.position.x - offSetX, player.transform.position.y - offSetY, transform.position.z);
    }
    void FixedUpdate()
    {
        float posXSmoothed = Mathf.SmoothDamp(transform.position.x, player.transform.position.x - offSetX, ref velocity.x, smoothTimeX);
        float posYSmoothed = Mathf.SmoothDamp(transform.position.y, player.transform.position.y - offSetY, ref velocity.y, smoothTimeY);

        //Apply the new smoothed position to the camera
        transform.position = new Vector3(posXSmoothed, posYSmoothed, transform.position.z);
    }
}

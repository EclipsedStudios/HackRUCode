using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchBoxToHead : MonoBehaviour
{
    public GameObject boxCollider, aCamera;

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3(transform.position.x, aCamera.transform.position.y, transform.position.z);
    }
}

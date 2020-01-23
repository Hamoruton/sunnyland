using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundManager : MonoBehaviour
{
    void Update()
    {
        transform.Translate(-0.05f, 0, 0);
        if (transform.position.x < -19.0f)
        {
            transform.position = new Vector3(38.0f, 0, 0);
        }
    }
}

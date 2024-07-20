using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTarget : MonoBehaviour
{
    void Update()
    {
        transform.position = Input.mousePosition;
    }
}

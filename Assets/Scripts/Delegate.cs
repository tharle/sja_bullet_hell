using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Delegate : MonoBehaviour
{
    public delegate float SomeDelegate();
    private SomeDelegate floatExample;

    public delegate float ParamDelegate(float min, float max);
    private ParamDelegate paramExample;

    private void Start()
    {
        floatExample += CreateNumber;
        paramExample += CreateNumber;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (floatExample == null) return;

            float resp = floatExample.Invoke();
            Debug.Log(resp);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (paramExample == null) return;

            float resp = paramExample.Invoke(50f, 80f);
            Debug.Log(resp);
        }
    }

    private float CreateNumber()
    {
        return UnityEngine.Random.Range(0f, 10f);
    }

    private float CreateNumber(float min, float max)
    {
        return UnityEngine.Random.Range(min, max);
    }
}

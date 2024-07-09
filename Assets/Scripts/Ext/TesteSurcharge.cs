using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesteSurcharge : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    [SerializeField] int[] m_DiceRolls;

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            int rand = Random.Range(1, 20);
            m_DiceRolls = m_DiceRolls.Add(rand);
        }
    }
}

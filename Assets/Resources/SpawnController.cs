using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] GameObject[] pos;
    Vector3 prev;
    void Start()
    {
        pos = GameObject.FindGameObjectsWithTag("Spawn");
        
       
        prev = Vector3.zero;
        for(int i = 0; i < 1; i++)
        {
            int random = Random.Range(0, pos.Length);

            if (i < 5 )
            {
                Instantiate(prefab, pos[random].transform.position, Quaternion.identity);
                prev = pos[random].transform.position;
            }
        }
       
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CheckpointArrow : MonoBehaviour
{
    public static CheckpointArrow instance;
    public Transform[] target;
    public float arrowspeed;
    private int index;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    private void Update()
    {
        
        Vector3 relativePos = target[index].position - transform.position;


        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        transform.rotation = rotation;

    }

    public void updateArrow(int level)
    {
        index = level;
    }

    

}
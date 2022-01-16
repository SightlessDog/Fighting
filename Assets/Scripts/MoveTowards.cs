using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowards : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;
    private Vector3 newtrans;

    void Start ()
    {
        player = GameObject.Find("Player");
        offset.x = transform.position.x - player.transform.position.x;
        offset.z = transform.position.z - player.transform.position.z;
        newtrans=transform.position;
    }
    void LateUpdate ()
    {
        newtrans.x= player.transform.position.x + offset.x;
        newtrans.z= player.transform.position.z + offset.z;
        transform.position = newtrans;
    }
}

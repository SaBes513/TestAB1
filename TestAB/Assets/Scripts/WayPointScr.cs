using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointScr : MonoBehaviour
{
    public GameObject[] ArrayWayPoints;


    private void OnDrawGizmos()
    {
        for (int i = 0; i < ArrayWayPoints.Length; i++)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(ArrayWayPoints[i].transform.position, .6f);
            if (i > 0)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(ArrayWayPoints[i].transform.position, ArrayWayPoints[i - 1].transform.position);
            }
        }
    }

    void Start()
    {
        LinkMO.SetWayPoints(this);
        for (int i = 0; i < ArrayWayPoints.Length; i++)
        {
            ArrayWayPoints[i].SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScr : MonoBehaviour
{
    public int WeaponID;

    public float speed = 3;
    Ray ray;
    RaycastHit hit;
    public Animator PlrAnimator;
    

    void Start()
    {
        LinkMO.SetPlayer(this);
        PlrAnimator.SetTrigger("Idle");
    }

    void Update()
    {
        if (LinkMO.move)
        {
            transform.position = Vector3.MoveTowards(transform.position, LinkMO.GetNextPosWayPoint().position, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, LinkMO.GetNextPosWayPoint().position) < .1)
            {
                PlrAnimator.SetTrigger("Idle");
                LinkMO.NextWayPoint();
                transform.LookAt(LinkMO.GetNextPosWayPoint().position);
                LinkMO.move = false;
                LinkMO.GetMain().ButtAIM.gameObject.SetActive(false);
                LinkMO.CheckIsl(false);
            }
        }
        else 
        {
            LinkMO.GetMain().Sec += Time.deltaTime;
        }
        if (LinkMO.GetMain()!=null && LinkMO.GetMain().ButtAIM.gameObject.activeSelf)
        {
            if (Input.GetMouseButtonDown(0))
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    Quaternion dir = Quaternion.LookRotation(hit.point - transform.position);
                    LinkMO.GetMain().CreateBullet(transform.position, dir);
                }
            }
        }
    }


    public void Rotate(Vector3 Pos)
    {
        transform.LookAt(Pos);
        //transform.rotation = Quaternion.LookRotation(Pos);
    }
}
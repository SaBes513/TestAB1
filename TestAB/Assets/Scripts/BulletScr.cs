using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScr : MonoBehaviour
{
    float Damage, Speed, MaxSpeed, AccSpeed, RadBoom, BoomDamage;
    Rigidbody RBBul;
    BoxCollider BCol;

    // Start is called before the first frame update
    void Start()
    {
        RBBul = GetComponent<Rigidbody>();
        BCol = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Speed > 0f) 
        {
            if (Speed < MaxSpeed) { Speed += AccSpeed * Time.deltaTime; RBBul.velocity = Speed * transform.forward; }
            if (Speed > MaxSpeed) { Speed = MaxSpeed; RBBul.velocity = Speed * transform.forward; }
        }
    }

    public void SetVars(float IDamage, float ISpeed, float IMaxSpeed, float IAccSpeed, float IRadBoom, float IBoomDamage)
    {
        Damage = IDamage;
        Speed = ISpeed;
        AccSpeed = IAccSpeed;
        MaxSpeed = IMaxSpeed;
        RadBoom = IRadBoom;
        BoomDamage = IBoomDamage;
        BCol.enabled = true;

        RBBul.velocity = Speed * transform.forward;
    }

    private void OnCollisionEnter(Collision collision)
    {
        EnemyScr Enem = collision.transform.GetComponent<EnemyScr>();
        if (Enem !=null)
        {
            Enem.TakeDamage(Damage);
            if (RadBoom > 0)
            {
                LinkMO.checkRad(transform.position, Damage, RadBoom);
            }
        }
        Speed = 0;
        MaxSpeed = 0;
        BCol.enabled = false;
        RBBul.velocity = Vector3.zero;
        transform.position = Vector3.zero;
    }
}

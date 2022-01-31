using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScr : MonoBehaviour
{
    int IdThis;
    public int NIsland;
    public float Health = 100;
    Animator EnmAnimator;
    // Start is called before the first frame update
    void Start()
    {
        EnmAnimator = GetComponent<Animator>();
        IdThis = LinkMO.AddEnem(this);
        EnmAnimator.SetTrigger("Idle");
    }

    private void OnDestroy()
    {
        LinkMO.DelEnem(IdThis);
        //Ragdoll?
    }

    public void ChangeId(int Id)
    {
        IdThis = Id;
    }

    public void TakeDamage(float Damage)
    {
        Health -= Damage;
        if (Health <= 0) { Destroy(this.gameObject); }
    }
}

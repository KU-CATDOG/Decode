using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MpOrb : Enemy
{
    public MpOrb[] ConnectedOrb;
    public bool isActive;
    public bool isAttacked;
    public int defineMP;
    Renderer mat;

    protected override void Start()
    {
        mat = gameObject.GetComponent<Renderer>();
        isAttacked = false;
        MaxHealth = Health = 1000f;
        MaxMP = 10f;
        MP = defineMP;
        isAttacked = MP == 0 ? false : true;
        ConnectValue(Define.ChangableValue.Mp, typeof(Enemy).GetProperty("MaxMP"), typeof(Enemy).GetProperty("MP"));
        SetColor();
        base.Start();
    }

    protected override Queue<IEnumerator> DecideNextRoutine()
    {
        Queue<IEnumerator> nextRoutines = new Queue<IEnumerator>();

        return nextRoutines;
    }

    private void Update()
    {
        if(MP<=0 && isAttacked)
        {
            ConnectedOrb[0].Reverse();
            ConnectedOrb[1].Reverse();
            isAttacked = !isAttacked;
        }
    }

    public void Reverse()
    {
        if (!this.isActive)
        {
            MP = 10f;
            isAttacked = true;
        }
        this.isActive = !this.isActive;
        SetColor();
    }

    private void SetColor()
    {
        if (isActive)
        {
            mat.material.color = Color.blue;
        }
        else
        {
            mat.material.color = Color.black;
        }
    }
}

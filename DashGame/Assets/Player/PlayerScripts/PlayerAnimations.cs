using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] Animator anim;

    string idle = "idle";
    string aim = "aim";
    string launch = "launch";
    string dashHit = "hit";
    string stop = "stop";

    [SerializeField] bool aiming=false;
    [SerializeField] PlayerMove moveScript;
    [SerializeField] float playbackSpeed = 1f;
    [SerializeField] public float aimPower=0f;

    [Header("Flash Hurt")]
    [SerializeField] Color flashColor;
    [SerializeField] SkinnedMeshRenderer bodyMesh;
    [SerializeField] List<MeshRenderer> propMeshes;
    Coroutine hurtFlash;
  
    private void Awake()
    {
        if(anim==null) anim = GetComponentInChildren<Animator>();

        bodyMesh=GetComponentInChildren<SkinnedMeshRenderer>();
        propMeshes = new List<MeshRenderer>(GetComponentsInChildren<MeshRenderer>());
    }

    private void Update()
    {
        if (moveScript.moveState == PlayerMove.MoveState.Dashing)
        {
            playbackSpeed=1f+(moveScript.currentVelocity.magnitude/100);
        }
        else
        {
            playbackSpeed = 1f;
        }
        anim.speed = playbackSpeed;

        if (aiming)
        {
            anim.Play("aim", 0, aimPower);
        }

        //debug
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                HurtAnim();
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                BounceAnim();
            }
        }
    }

    public void IdleAnim()
    {
        anim.SetTrigger(idle);
    }

    public void AimAnim()
    {
        //anim.SetTrigger(aim); //played in update instead.
        aiming = true;
    }

    public void LaunchAnim()
    {
        anim.SetTrigger(launch);
        aiming= false;
        aimPower = 0f;
    }

    public void DashHitAnim()
    {
        anim.SetTrigger(dashHit);
    }

    public void StopAnim()
    {
        anim.SetTrigger(stop);
    }

    public void HurtAnim()
    {
        anim.SetTrigger("hurt");

        if (hurtFlash == null)
        {
            hurtFlash = StartCoroutine(HurtFlash());
        }
    }
    public void BounceAnim()
    {
        print("bounce");
        anim.SetTrigger("bounce");
    }


    IEnumerator HurtFlash() //TODO material property block?
    {
        bodyMesh.material.color = flashColor;

        foreach(MeshRenderer ms in propMeshes)
        {
            ms.material.color = flashColor;
        }

            yield return new WaitForSeconds(0.25f);

        bodyMesh.material.color = Color.white;

        foreach (MeshRenderer ms in propMeshes)
        {
            ms.material.color = Color.white;
        }
        hurtFlash = null;
    }

}

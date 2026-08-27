using UnityEngine;
using System.Collections;

public class EnemyAnimations : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] EnemyMove move;
    [SerializeField] EnemyCombat combat;

    public bool isWalking=false;
    public bool isAttacking=false;
    public bool poolBall=false;

    [Header("Flash")]
    [SerializeField] SkinnedMeshRenderer skm;
    [SerializeField] MeshRenderer[] mr;
    [SerializeField] Color hurtColor;
    Coroutine flashRoutine;
    //performace
    private static MaterialPropertyBlock sharedPropertyBlock;
    private static readonly int ColorPropertyID = Shader.PropertyToID("_BaseColor");
    private readonly WaitForSeconds flashWait = new WaitForSeconds(0.25f);

    private void Awake()
    {
        if (sharedPropertyBlock == null)
        {
            sharedPropertyBlock = new MaterialPropertyBlock();
        }
    }

    private void Start()
    {
        if(anim==null) anim = GetComponent<Animator>();
        if(move==null) move=GetComponentInParent<EnemyMove>();
        if(combat==null) combat=GetComponentInParent<EnemyCombat>();
    }

    private void Update()
    {
        if (move.agent.enabled) //zombie mode
        {
            if (poolBall) { poolBall = false; anim.SetTrigger("stop"); }
            //combat check
            if (combat.inCombat)
            {
                if (!isAttacking)
                {
                    anim.SetTrigger("combat");
                    isAttacking = true;
                    isWalking = false;
                }
            }
            else//not in combat range
            {
                if (move.agent.velocity.sqrMagnitude > 0.01f) //agent is moving
                {
                    if (!isWalking)
                    {
                        isWalking = true;
                        anim.SetTrigger("walk");
                    }
                }
                else //agent is still
                {
                    if (isWalking)
                    {
                        isWalking = false;
                        anim.SetTrigger("idle");
                    }
                }
                isAttacking=false;
            }
        }
        else //pool ball mode
        {
            isWalking=false;
            isAttacking = false;
            if (!poolBall)
            {
                anim.SetTrigger("hurt");

                Flash();

                poolBall = true;
            }

        }
    }

    public void Flash()
    {
        if (flashRoutine != null) StopCoroutine(flashRoutine);
        flashRoutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        SetRendererColor(hurtColor);

        yield return flashWait;

        SetRendererColor(Color.white);

        flashRoutine = null;
    }
    private void SetRendererColor(Color color)
    {
        sharedPropertyBlock.SetColor(ColorPropertyID, color);

        if (skm != null && skm.enabled)
        {
            skm.SetPropertyBlock(sharedPropertyBlock);
        }

        if (mr != null)
        {
            foreach (var m in mr)
            {
                if (m != null && m.enabled)
                {
                    m.SetPropertyBlock(sharedPropertyBlock);
                }
            }
        }
    }

}

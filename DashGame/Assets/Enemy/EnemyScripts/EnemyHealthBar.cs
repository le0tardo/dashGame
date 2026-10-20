using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    private Vector3 initialScale;
    private Quaternion fixedRotation;
    private Transform parentTransform;

    private EnemyCombat enemy;
    private EnemyRangedBehavour rangedEnemy;
    private float hp;
    private float maxHp;

    [SerializeField] private Transform fillScale;

    SpriteRenderer[] renderers;
    bool showBar=false;

    private void Awake()
    {
        renderers = GetComponentsInChildren<SpriteRenderer>();

        initialScale = transform.localScale;
        fixedRotation = Quaternion.Euler(0,90,0);
        parentTransform = transform.parent;

        enemy = GetComponentInParent<EnemyCombat>();
        if (enemy != null)
        {
            hp = enemy.health; maxHp = hp;
        }
        else
        {
            rangedEnemy=GetComponentInParent<EnemyRangedBehavour>();
            if (rangedEnemy != null)
            {
                hp=rangedEnemy.health; maxHp = hp;
            }
        }

        foreach (SpriteRenderer r in renderers)
        {
            r.enabled = false;
        }
    }

    private void LateUpdate()
    {
        if (enemy == null && rangedEnemy==null) return;

        if(enemy!=null) hp = enemy.health;
        if(rangedEnemy!=null)hp = rangedEnemy.health;


        if (!showBar && hp < maxHp)
        {
            showBar = true;
            foreach(SpriteRenderer r in renderers)
            {
                r.enabled = true;
            }
        }

        transform.rotation = fixedRotation;

        if (parentTransform != null)
        {
            Vector3 parentScale = parentTransform.lossyScale;

            if (parentScale.x != 0f && parentScale.y != 0f && parentScale.z != 0f)
            {
                transform.localScale = new Vector3(
                    initialScale.x / parentScale.x,
                    initialScale.y / parentScale.y,
                    initialScale.z / parentScale.z
                );
            }
        }

        if (hp < maxHp)
        {
            float pc = Mathf.Clamp01(hp / maxHp);
            fillScale.localScale = new Vector3(pc, fillScale.localScale.y, fillScale.localScale.z);
        }
    }
}
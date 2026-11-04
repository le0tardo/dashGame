using System.Collections;
using UnityEngine;

public class FlashRed : MonoBehaviour
{
    [SerializeField] SkinnedMeshRenderer[] smr;
    [SerializeField] MeshRenderer[] mr;

    [SerializeField] Color flashColor;

    [SerializeField] PlayerEquipmentGetter equipped;

    private static MaterialPropertyBlock sharedPropertyBlock;
    private static readonly int ColorPropertyID = Shader.PropertyToID("_BaseColor");
    private readonly WaitForSeconds flashWait = new WaitForSeconds(0.2f);

    Coroutine flashRoutine;

    private void Awake()
    {
        if (sharedPropertyBlock == null)
        {
            sharedPropertyBlock = new MaterialPropertyBlock();
        }
    }

    private void OnEnable()
    {
        if (sharedPropertyBlock != null)
        {
            SetRendererColor(Color.white);
        }
    }

    private void Start()
    {
        if (equipped != null)
        {
            Invoke(nameof(SetPlayerEquipmentRenderers), 1f);
        }
    }
    
    void SetPlayerEquipmentRenderers()
    {
        MeshRenderer helmetMr = equipped.equippedHelmet != null ? equipped.equippedHelmet.GetComponentInChildren<MeshRenderer>() : null;
        MeshRenderer mainHandMr = equipped.equippedMainHand != null ? equipped.equippedMainHand.GetComponentInChildren<MeshRenderer>() : null;
        MeshRenderer offHandMr = equipped.equippedOffHand != null ? equipped.equippedOffHand.GetComponentInChildren<MeshRenderer>() : null;

        mr[0] = helmetMr;
        mr[1] = mainHandMr;
        mr[2] = offHandMr;
    }
    public void Flash()
    {
        if (flashRoutine != null) StopCoroutine(flashRoutine);
        flashRoutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        SetRendererColor(flashColor);

        yield return flashWait;

        SetRendererColor(Color.white);

        flashRoutine = null;
    }
    private void SetRendererColor(Color color)
    {
        sharedPropertyBlock.SetColor(ColorPropertyID, color);

        if (smr != null && smr.Length>0)
        {
            foreach (var sm in smr)
            {
                if (sm != null && sm.enabled)
                {
                    sm.SetPropertyBlock(sharedPropertyBlock);
                }
            }
        }

        if (mr != null&& mr.Length>0)
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

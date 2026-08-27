using System.Collections;
using UnityEngine;

public class FlashRed : MonoBehaviour
{
    [SerializeField] SkinnedMeshRenderer[] smr;
    [SerializeField] MeshRenderer[] mr;

    [SerializeField] Color flashColor;

    private static MaterialPropertyBlock sharedPropertyBlock;
    private static readonly int ColorPropertyID = Shader.PropertyToID("_BaseColor");
    private readonly WaitForSeconds flashWait = new WaitForSeconds(0.25f);

    Coroutine flashRoutine;

    private void Awake()
    {
        if (sharedPropertyBlock == null)
        {
            sharedPropertyBlock = new MaterialPropertyBlock();
        }
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
                    print("flashed color on skinned mesh renderer");
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
                    print("flashed color on mesh renderer");
                }
            }
        }
    }
}

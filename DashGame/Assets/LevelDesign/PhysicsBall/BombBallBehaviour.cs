using System.Collections;
using UnityEngine;

public class BombBallBehaviour : MonoBehaviour
{
    [SerializeField] float explosionPower = 1f;
    [SerializeField] float fuseTime;
    [SerializeField] GameObject fuseGfx;
    [SerializeField] Animator anim;
    [SerializeField] bool lit=false;
    [SerializeField] FlashRed flash;
    [SerializeField] AudioClip fuseSound;
    Coroutine explosionRoutine=null;

    public void Ignite()
    {
        if (lit) return;

        lit= true;
        fuseGfx?.SetActive(true);

        if (explosionRoutine == null)
        {
            explosionRoutine = StartCoroutine(ExplosionRoutine());
        }

    }

    IEnumerator ExplosionRoutine()
    {
        AudioManager.inst.PlayCustomSound(fuseSound,0.5f);

        yield return new WaitForSeconds(fuseTime / 2f);
        anim?.SetTrigger("pulse");
        if (flash != null) flash.Flash();

        yield return new WaitForSeconds(fuseTime *0.25f);
        if (flash != null) flash.Flash();

        yield return new WaitForSeconds(fuseTime / 2f);
        Explode();
    }

    void Explode()
    {
        //activate explostion from pool
        ExplosionPool.inst.SpawnExplosion(transform.position, explosionPower);
        this.gameObject.SetActive(false);
    }
}

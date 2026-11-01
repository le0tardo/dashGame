using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossDoorBehaviour : MonoBehaviour, IHittable, IEnemy
{
    [Header("References")]
    [SerializeField] GameObject eye;
    [SerializeField] Transform player;
    [SerializeField] Animator anim;

    [Header("UI references")]
    [SerializeField] GameObject bossHealthObject;
    [SerializeField]Image healthBar;
    [SerializeField]Image juiceBar;
    [SerializeField]TextMeshProUGUI healthPrecentage;
    [SerializeField]TextMeshProUGUI bossNameText;

    [Header("Boss Stats")]
    [SerializeField] string bossName;
    [SerializeField] public float health;
    [SerializeField] float currentHealth;

    [Header("Particles")]
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] ParticleSystem[] chargeParticles;
    [SerializeField] ParticleSystem[] BurstParticles;

    [SerializeField] float bounce = 1f;
    public float hitBounce => bounce;

    private void Start()
    {
        player = LevelManager.inst.playerMove.transform;
        currentHealth = health;
        bossHealthObject.SetActive(true);
        UpdateUI();

        lineRenderer.enabled = false;

        ChargeUp();
        Invoke(nameof(Fire), 2f);
    }

    private void Update()
    {
        if (eye != null && player != null)
        {
            RotateEyeball();
        }
    }

    public void OnHit(Vector3 hitPosition, float power)
    {
        Vector3 fxPos = (transform.position + hitPosition) / 2;
        HitFxManager.inst.HitFX1(fxPos, power / 10);
        CameraShake.inst.Shake(0.1f, 1f);

        AudioManager.inst.PlayImpactSound(power);

        print("player hit the fucking eye");
    }

    public void TakeDamage(float damage)
    {
        print("door takes damage: "+damage);
        currentHealth-=damage;

        if (currentHealth <= 0)
        {
            print("you beat the game!!");
        }
        else
        {
            print("better luck next time!");
        }
        anim.SetTrigger("hurt");
        UpdateUI();
    }

    void UpdateUI()
    {
        bossNameText.text = bossName;
        float p = currentHealth / health;
        healthBar.transform.localScale=new Vector3(p, 1, 1);
        int pc = Mathf.RoundToInt(p * 100);
        healthPrecentage.text=pc.ToString()+"%";

        StartCoroutine(HealthJuice(p));
    }

    void ChargeUp()
    {
        foreach (ParticleSystem c in chargeParticles)
        {
            c.Play();
        }
    }
    void Fire()
    {
        foreach (ParticleSystem c in chargeParticles)
        {
            c.Stop();
        }

        foreach (ParticleSystem b in BurstParticles)
        {
            b.Play();
        }

        print("Fire!");
        lineRenderer.enabled = true;

        Vector3 startPos=new Vector3(lineRenderer.transform.position.x,1.5f,lineRenderer.transform.position.z);
        Vector3 endPos=new Vector3(player.position.x,1f,player.position.z);

        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);

        StartCoroutine(Beam());
    }

    void RotateEyeball() 
    {
        Vector3 direction = player.position - eye.transform.position;

        if (direction != Vector3.zero)
        {
            Quaternion worldTarget = Quaternion.LookRotation(direction);
            Quaternion localTarget = Quaternion.Inverse(transform.rotation) * worldTarget;

            //fkn shit arbitrary conversion that shoulnd exist...
            float localY = localTarget.eulerAngles.y;
            if (localY > 180f) localY -= 360f;
            localY -= 90f;

            float clampedY = Mathf.Clamp(localY, -45f, 45f);
            eye.transform.localRotation = Quaternion.Euler(0f, clampedY, 0f);
        }
    }

    IEnumerator Beam()
    {
        float e = 0f;
        float d = 0.25f;
        lineRenderer.widthMultiplier = 1f;

        while (e < d)
        {
            e += Time.deltaTime;
            float t = e / d;
            lineRenderer.widthMultiplier-=4*Time.deltaTime;
            yield return null;
        }

        lineRenderer.enabled = false;

    }
    IEnumerator HealthJuice(float targetScale)
    {
        float duration = 0.5f;
        float elapsedTime = 0f;

        Vector3 initialScale = juiceBar.transform.localScale;
        Vector3 finalScale = new Vector3(targetScale, 1f, 1f);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            juiceBar.transform.localScale = Vector3.Lerp(initialScale, finalScale, t);

            yield return null;
        }
        juiceBar.transform.localScale = finalScale;
    }
}

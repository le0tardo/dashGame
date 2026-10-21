using TMPro;
using UnityEngine;
using System.Collections;

public class LobbyStatsUI : MonoBehaviour
{
    public static LobbyStatsUI inst;

    [SerializeField] TextMeshProUGUI levelString;
    [SerializeField] TextMeshProUGUI health;
    [SerializeField] TextMeshProUGUI stamina;
    [SerializeField] TextMeshProUGUI weapon;
    [SerializeField] TextMeshProUGUI impact;
    [SerializeField] TextMeshProUGUI defence;
    [SerializeField] TextMeshProUGUI speed;
    [SerializeField] TextMeshProUGUI drag;
    [SerializeField] TextMeshProUGUI bounce;

    [SerializeField] private float countDuration = 0.8f;

    private Coroutine countRoutine;

    int prevHealth;
    int prevStamina;
    int prevWeapon; 
    int prevImpact;
    int prevDefence;
    int prevSpeed;
    int prevDrag;
    int prevBounce;

    private void Awake()
    {
        inst = this;
    }

    private void Start()
    {
        DrawStatsInstantly();
    }
    public void DrawStatsInstantly()
    {
        var stats = LobbyStatManager.inst;
        levelString.text= stats.currentHero.heroLevel.ToString();
        health.text = stats.heroHealth.ToString();
        stamina.text=stats.heroStamina.ToString();
        weapon.text=stats.heroWeaponDamage.ToString();
        impact.text=stats.heroImpactDamage.ToString();
        defence.text=stats.heroDefence.ToString();
        speed.text=stats.heroMaxSpeed.ToString();
        drag.text=stats.heroDrag.ToString();
        bounce.text=Mathf.RoundToInt(stats.heroBounciness*100).ToString();

        //set as previous values for coroutine here
        prevHealth = Mathf.RoundToInt(stats.heroHealth);
        prevStamina=Mathf.RoundToInt(stats.heroStamina);
        prevWeapon = Mathf.RoundToInt(stats.heroWeaponDamage);
        prevImpact=Mathf.RoundToInt(stats.heroImpactDamage);
        prevDefence=Mathf.RoundToInt(stats.heroDefence);
        prevSpeed = Mathf.RoundToInt(stats.heroMaxSpeed);
        prevDrag=Mathf.RoundToInt(stats.heroDrag);
        prevBounce = Mathf.RoundToInt(stats.heroBounciness);
    }
    public void DrawStats()
    {
        if (countRoutine != null)
        {
            StopCoroutine(countRoutine);
        }
        countRoutine = StartCoroutine(AnimateStatsRoutine());

        var stats = LobbyStatManager.inst;

        levelString.text = stats.currentHero.heroLevel.ToString();
        health.text = stats.heroHealth.ToString();
        stamina.text = stats.heroStamina.ToString();
        weapon.text = stats.heroWeaponDamage.ToString();
        impact.text = stats.heroImpactDamage.ToString();
        defence.text = stats.heroDefence.ToString();
        speed.text = stats.heroMaxSpeed.ToString();
        drag.text = stats.heroDrag.ToString();
        bounce.text = Mathf.Floor(stats.heroBounciness * 100).ToString();
    }

    private IEnumerator AnimateStatsRoutine()
    {
        var stats = LobbyStatManager.inst;

        levelString.SetText("{0}", Mathf.RoundToInt(stats.currentHero.heroLevel));

        int targetHealth = Mathf.RoundToInt(stats.heroHealth);
        int targetStamina = Mathf.RoundToInt(stats.heroStamina);
        int targetWeapon = Mathf.RoundToInt(stats.heroWeaponDamage);
        int targetImpact = Mathf.RoundToInt(stats.heroImpactDamage);
        int targetDefence = Mathf.RoundToInt(stats.heroDefence);
        int targetSpeed = Mathf.RoundToInt(stats.heroMaxSpeed);
        int targetDrag = Mathf.RoundToInt(stats.heroDrag);
        int targetBounce = Mathf.FloorToInt(stats.heroBounciness * 100f);

        //snap to correct starting int
        health.SetText("{0}", prevHealth);
        stamina.SetText("{0}", prevStamina);
        weapon.SetText("{0}", prevWeapon);
        impact.SetText("{0}", prevImpact);
        defence.SetText("{0}", prevDefence);
        speed.SetText("{0}", prevSpeed);
        drag.SetText("{0}", prevDrag);
        bounce.SetText("{0}", prevBounce);

        float elapsed = 0f;

        while (elapsed < countDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float progress = Mathf.Clamp01(elapsed / countDuration);
            float easeProgress = 1f - Mathf.Pow(1f - progress, 3f);

            health.SetText("{0}", Mathf.RoundToInt(Mathf.Lerp(prevHealth, targetHealth, easeProgress)));
            stamina.SetText("{0}", Mathf.RoundToInt(Mathf.Lerp(prevStamina, targetStamina, easeProgress)));
            weapon.SetText("{0}", Mathf.RoundToInt(Mathf.Lerp(prevWeapon, targetWeapon, easeProgress)));
            impact.SetText("{0}", Mathf.RoundToInt(Mathf.Lerp(prevImpact, targetImpact, easeProgress)));
            defence.SetText("{0}", Mathf.RoundToInt(Mathf.Lerp(prevDefence, targetDefence, easeProgress)));
            speed.SetText("{0}", Mathf.RoundToInt(Mathf.Lerp(prevSpeed, targetSpeed, easeProgress)));
            drag.SetText("{0}", Mathf.RoundToInt(Mathf.Lerp(prevDrag, targetDrag, easeProgress)));
            bounce.SetText("{0}", Mathf.RoundToInt(Mathf.Lerp(prevBounce, targetBounce, easeProgress)));

            yield return null;
        }

        //snap to correct value
        health.SetText("{0}", targetHealth);
        stamina.SetText("{0}", targetStamina);
        weapon.SetText("{0}", targetWeapon);
        impact.SetText("{0}", targetImpact);
        defence.SetText("{0}", targetDefence);
        speed.SetText("{0}", targetSpeed);
        drag.SetText("{0}", targetDrag);
        bounce.SetText("{0}", targetBounce);

        //set as prev for nextCorotuine run
        prevHealth = Mathf.RoundToInt(stats.heroHealth);
        prevStamina = Mathf.RoundToInt(stats.heroStamina);
        prevWeapon = Mathf.RoundToInt(stats.heroWeaponDamage);
        prevImpact = Mathf.RoundToInt(stats.heroImpactDamage);
        prevDefence = Mathf.RoundToInt(stats.heroDefence);
        prevSpeed = Mathf.RoundToInt(stats.heroMaxSpeed);
        prevDrag = Mathf.RoundToInt(stats.heroDrag);
        prevBounce = Mathf.RoundToInt(stats.heroBounciness);

        countRoutine = null;
    }
}

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour
{
    public static LevelManager inst { get; private set; }

    [Header("Session Data")]
    [SerializeField] public SessionData sessionData;

    [Header("Hero Stats")]
    [SerializeField] public HeroStatsObject heroStats;
    [SerializeField] public PlayerMove playerMove;
    [SerializeField] public PlayerStats playerStats;
    [SerializeField] public bool playerIsDead=false;

    [Header("Level Stats")]
    [SerializeField] public float level;
    [SerializeField] public float health;
    [SerializeField] public float maxHealth;
    [SerializeField] public float stamina;
    [SerializeField] public float maxStamina;
    [SerializeField] public float staminaRegenRate = 10f;
    [SerializeField] public float impactDamage;
    [SerializeField] public float weaponDamage;
    [SerializeField] public float defence;
    [SerializeField] public float maxSpeed;
    [SerializeField] public float bounce;
    [SerializeField] public float drag;


    [Header("Other stats")]
    [SerializeField] public float xp;
    [SerializeField] public float maxXp;

    [SerializeField] public float staminaRegenTime = 1f;
    [SerializeField] public float staminaRegenDelay = 1f;

    [SerializeField] public float keys;
    [SerializeField] public float gold;

    [SerializeField] public float timeScale=1f;
    [SerializeField] public float score=0f;

    [SerializeField] public Vector3 checkPoint;

    [Header("UI")]
    [SerializeField] CanvasScript canvas;
    [SerializeField] GameObject gameOverScreen;

    Coroutine slowDownTimeRoutine = null;

    private void Awake()
    {
        inst = this;
        maxHealth = health;
        GameObject playerObj = GameObject.Find("Player");
        if (playerObj != null) {playerMove=playerObj.GetComponent<PlayerMove>();playerStats = playerObj.GetComponent<PlayerStats>(); }

        GameObject bulletPool = GameObject.Find("BulletPool");
        if (bulletPool == null) { new GameObject("BulletPool");}


        if (sessionData.hero != null)
        {
            LoadStatsFromSessionData();
        }

        SetHeroStats();

        checkPoint=playerMove.gameObject.transform.position;

        if(gameOverScreen.activeInHierarchy)gameOverScreen.SetActive(false);

    }

    void LoadStatsFromSessionData()
    {
        heroStats.heroHealth = sessionData.health;
        heroStats.heroStamina = sessionData.stamina;
        heroStats.heroImpactDamage = sessionData.impactDamage;
        heroStats.heroWeaponDamage = sessionData.weaponDamage;
        heroStats.heroDefence = sessionData.defence;
        heroStats.heroMaxSpeed = sessionData.maxSpeed;
        heroStats.heroDrag=sessionData.drag;
        heroStats.heroBounce=sessionData.bounce;

        //also set from base hero stats...
        staminaRegenRate = heroStats.heroStaminaRegenRate;
        staminaRegenTime=heroStats.heroStaminaRegenSpeed;
    }

    void SetHeroStats() //direcly from scriptable
    {
        level=heroStats.heroLevel;
        health = heroStats.heroHealth;
        maxHealth = heroStats.heroHealth;
        stamina = heroStats.heroStamina;
        maxStamina = heroStats.heroStamina;
        staminaRegenRate = heroStats.heroStaminaRegenRate;

        weaponDamage= heroStats.heroWeaponDamage;
        impactDamage = heroStats.heroImpactDamage;
        defence = heroStats.heroDefence;

        maxSpeed = heroStats.heroMaxSpeed;
        bounce = heroStats.heroBounce;
        drag=heroStats.heroDrag;

        //set hero movement stats
        playerMove.maxVelocity = maxSpeed;
        playerMove.drag = drag;
        playerMove.bounciness = bounce;
    }

    public void SetStatsAfterLevelUp()
    {
        playerMove.maxVelocity = maxSpeed;
        playerMove.drag = drag;
        playerMove.bounciness = bounce;

        playerMove.SetMoveStatsAfterLevelUp();
    }

    private void Update()
    {
        Time.timeScale = timeScale;

        if (stamina < maxStamina && timeScale>0)
        {
            FillStamina();
        }
    }

    public void ResetLevel()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
    public void ChangeHealth(int _health) //for both + and - health
    {
        health += _health;
        health=Mathf.Clamp(health, 0, maxHealth);
        canvas.UpdateHealth();

        if (health <= 0)
        {
            //do the die thing
            playerIsDead = true;
            gameOverScreen.SetActive(true);
            StopAllCoroutines();
            FreezeTime();
            //timescale to 0, interrupt aim script? 
        }

    }
    public void UseStamina(float amount)
    {
        stamina = Mathf.Clamp(stamina - amount, 0f, maxStamina);
        //print("used "+amount +" stamina");
        canvas.UpdateStamina();

        staminaRegenTime = staminaRegenDelay;//pause before regenig
    }
    public void GetStamina(float amount)
    {
        stamina = Mathf.Clamp(stamina + amount, 0f, maxStamina);
        canvas.UpdateStamina();
    }

    public void GetXp(float amount)
    {
        xp += amount;
        if (xp >= maxXp)
        {

            LevelUp();

        }
        canvas.UpdateXp();
    }

    void LevelUp()
    {
        maxXp = Mathf.Floor(maxXp * 1.5f);
        level++;
        xp = 0;


        float levelOffset = Mathf.Max(0, level - 1);
        float flatMultiplier = levelOffset * 12f;
        float growthRate = 0.10f;

        maxHealth =  MathF.Floor (maxHealth * Mathf.Pow(1f + growthRate, levelOffset));
        defence = MathF.Floor (defence * Mathf.Pow(1f + growthRate, levelOffset));
        impactDamage = MathF.Floor(impactDamage * Mathf.Pow(1f + growthRate, levelOffset));
        weaponDamage = MathF.Floor(weaponDamage * Mathf.Pow(1f + growthRate, levelOffset));

        maxStamina = MathF.Floor(maxStamina * Mathf.Pow(1f + growthRate, levelOffset));
        staminaRegenRate = staminaRegenRate++;

        maxSpeed = MathF.Floor(maxSpeed * Mathf.Pow(1f + growthRate, levelOffset));
        drag = drag++;

        bounce = MathF.Floor(bounce * Mathf.Pow(1f + growthRate, levelOffset) * 100f) / 100f; //to perserve decimal points...

        canvas.UpdateLevel();
        canvas.OpenLevelUpBox();
        canvas.UpdateHealth();

        FreezeTime();
    }
    void FillStamina()
    {
        if (staminaRegenTime > 0f)
        {
            staminaRegenTime-= Time.deltaTime;//unsclaed time for consistant regen on aim
            return;
        }
        if (stamina < maxStamina)
        {
            stamina += staminaRegenRate * Time.unscaledDeltaTime;

            if (stamina > maxStamina)
            {
                stamina = maxStamina;
            }
        }
        canvas.UpdateStamina();
    }

    public void AddKey()
    {
        keys++;
        canvas.UpdateKeys();
    }
    public void UseKey()
    {
        keys--;
        canvas.UpdateKeys();
    }

    public void AddGold()
    {
        LevelDeathStats.inst.goldCollected++;
        gold++;
        canvas.UpdateGold();
    }

    public void SlowDownTime()
    {
        StopAllCoroutines();
        if (slowDownTimeRoutine == null)
        {
            slowDownTimeRoutine = StartCoroutine(SlowDown());
        }

    }

    public void FreezeTime()
    {
        StopAllCoroutines();
        if (slowDownTimeRoutine == null)
        {
            slowDownTimeRoutine=StartCoroutine(FreezeTimeRoutine());
        }
    }

    public void ResetTime()
    {
        StopAllCoroutines();
        timeScale = 1f;
        slowDownTimeRoutine = null;
    }
    private IEnumerator SlowDown()
    {
        float startScale = timeScale;
        float targetScale = 0.25f;
        float duration = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = elapsedTime / duration;
            timeScale = Mathf.Lerp(startScale, targetScale, normalizedTime);
            yield return null;
        }

        timeScale = targetScale; //snap
    }

    private IEnumerator FreezeTimeRoutine()
    {
        float startScale = timeScale;
        float targetScale = 0f;
        float duration = 0.25f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = elapsedTime / duration;
            timeScale = Mathf.Lerp(startScale, targetScale, normalizedTime);
            yield return null;
        }

        timeScale = targetScale; //snap
    }

    public void SetCheckPoint(Vector3 point)
    {
        checkPoint = point;
    }

}

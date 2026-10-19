using TMPro;
using UnityEngine;
using System.Collections;

public class LevelDeathStatsUI : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI goldText;
    [SerializeField] TextMeshProUGUI damageDealtText;
    [SerializeField] TextMeshProUGUI damageTakenText;

    private float countDuration = 1.0f;
    private Coroutine countCoroutine;
    private void OnEnable()
    {
        if (countCoroutine != null)
        {
            StopCoroutine(countCoroutine);
        }

        countCoroutine = StartCoroutine(AnimateStatsRoutine());
    }

    private IEnumerator AnimateStatsRoutine()
    {
        // 1. Fetch target stats from manager
        float targetGold = LevelDeathStats.inst.goldCollected;
        float targetDamageDealt = LevelDeathStats.inst.damageDealt;
        float targetDamageTaken = LevelDeathStats.inst.damageTaken;

        // 2. Initialize display to 0 without string allocation
        goldText.SetText("{0}", 0);
        damageDealtText.SetText("{0}", 0);
        damageTakenText.SetText("{0}", 0);

        float elapsed = 0f;

        // 3. Animate count-up over countDuration
        while (elapsed < countDuration)
        {
            elapsed += Time.unscaledDeltaTime; // Use unscaledDeltaTime so animation works even if game is paused
            float progress = Mathf.Clamp01(elapsed / countDuration);

            // Ease-out curve makes numbers slow down gracefully near the end
            float easeProgress = 1f - Mathf.Pow(1f - progress, 3f);

            int currentGold = Mathf.RoundToInt(Mathf.Lerp(0, targetGold, easeProgress));
            int currentDealt = Mathf.RoundToInt(Mathf.Lerp(0, targetDamageDealt, easeProgress));
            int currentTaken = Mathf.RoundToInt(Mathf.Lerp(0, targetDamageTaken, easeProgress));

            // Update TMP text zero-alloc
            goldText.SetText("{0}", currentGold);
            damageDealtText.SetText("{0}", currentDealt);
            damageTakenText.SetText("{0}", currentTaken);

            yield return null; // Wait until next frame
        }

        // 4. Ensure final exact target values are set at the end
        goldText.SetText("{0}", targetGold);
        damageDealtText.SetText("{0}", targetDamageDealt);
        damageTakenText.SetText("{0}", targetDamageTaken);

        countCoroutine = null;
    }
}

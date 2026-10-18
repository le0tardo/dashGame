using UnityEngine;
using System.Collections.Generic;

public class LevelUpBox : MonoBehaviour
{
    [SerializeField] private List<UpgradeObject> upgrades;
    private List<UpgradeObject> availableUpgrades;

    [SerializeField] private UpgradeButton[] buttons;

    [SerializeField] Animator anim;

    [SerializeField] AudioClip fanfare;
    [SerializeField] AudioClip multiPop;

    private void OnEnable()
    {
        AudioManager.inst.PlayCustomSound(fanfare,0.85f);

        anim.SetTrigger("pop");

        AudioManager.inst.PlayCustomSound(multiPop, 0.5f);



        availableUpgrades = new List<UpgradeObject>(upgrades);

        for (int i = 0; i < buttons.Length; i++)
        {
            if (availableUpgrades.Count == 0)
            {
                buttons[i].gameObject.SetActive(false);
                continue;
            }

            buttons[i].gameObject.SetActive(true);

            int r = Random.Range(0, availableUpgrades.Count);

            buttons[i].SetUpgrade(availableUpgrades[r]);

            availableUpgrades.RemoveAt(r);
        }
    }
}

using UnityEngine;

[CreateAssetMenu(fileName = "HeroRelic", menuName = "Scriptable Objects/Hero Relic")]
public class HeroRelicObject : ScriptableObject
{
    [Header("Relic Info")]
    public string relicName;
    public Sprite relicIcon;

    [Header("Relic Stats")]
    public float relicAttackPower;
    public float relicAttackSpeed;
}

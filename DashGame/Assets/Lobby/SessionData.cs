using UnityEngine;

[CreateAssetMenu(fileName = "SessionData", menuName = "Scriptable Objects/Session Data")]
public class SessionData : ScriptableObject
{
    public HeroStatsObject hero; //for when there are multiple

    public float health;
    public float stamina;
    public float impactDamage;
    public float weaponDamage;
    public float defence;
    public float maxSpeed;
    public float drag;
    public float bounce;

    public HeroEquipmentObject helmet;
    public HeroEquipmentObject mainHand;
    public HeroEquipmentObject offHand;

    /*
    public HeroRelicObject relic1;
    public HeroRelicObject relic2;
    public HeroRelicObject relic3;
    */
}

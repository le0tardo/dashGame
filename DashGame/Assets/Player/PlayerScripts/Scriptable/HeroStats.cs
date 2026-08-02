using UnityEngine;

[CreateAssetMenu(fileName = "HeroStats", menuName = "Scriptable Objects/Hero Stats")]
public class HeroStats : ScriptableObject
{
    [Header("Stats")]
    string heroName;
    float heroHealth;
    float heroDamage;
    float heroSpeed;
    //etc...
}

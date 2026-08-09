using UnityEngine;

[CreateAssetMenu(fileName = "EnemyObject", menuName = "Scriptable Objects/Enemy Object")]
public class EnemyObject : ScriptableObject
{
    public string enemyName;
    public Sprite enemyIcon;
    public float enemyHealth;
    public float enemyPhysicalDamage;
    public float enemyElementalDamage;
    public ElementalType enemyElementalType;

}

using UnityEngine;

[CreateAssetMenu(fileName = "PetObject", menuName = "Scriptable Objects/Pet Object")]

public class PetObject : ScriptableObject
{
    [Header("Info")]
    [SerializeField] public string petName;
    [SerializeField] public Sprite petIcon;

    [Header("Stats")]
    [SerializeField] public float petAttackPower;
    [SerializeField] public float petAttackSpeed;
}

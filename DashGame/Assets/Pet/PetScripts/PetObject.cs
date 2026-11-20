using UnityEngine;

[CreateAssetMenu(fileName = "PetObject", menuName = "Scriptable Objects/Pet Object")]

public class PetObject : ScriptableObject
{
    public enum PetType
    {
        StoneSpitter,
        FireDragon,
        EyeBall,
        FrostDragon

    }

    [Header("Info")]
    [SerializeField] public string petName;
    [SerializeField] public PetType petType;
    [SerializeField] public Sprite petIcon;
    [SerializeField] public int petLevel;

    [Header("Stats")]
    [SerializeField] public float petAttackPower;
    [SerializeField] public float petAttackSpeed;

    [Header("Prefab")]
    [SerializeField] public GameObject petPrefab;
    [SerializeField] public AudioClip petSound;
}

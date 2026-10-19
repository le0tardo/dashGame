using UnityEngine;

public class LevelDeathStats : MonoBehaviour
{
    public static LevelDeathStats inst;

    [SerializeField] public float goldCollected = 0f;
    [SerializeField] public float damageDealt= 0f;
    [SerializeField] public float damageTaken = 0f;

    private void Awake()
    {
        inst = this;
    }

}

using UnityEngine;

public class PetCombat : MonoBehaviour
{
    RoomBehaviour currentRoom;

    Transform currentTarget;

    Coroutine attackRoutine;

    float attackDamage;
    float attackSpeed;

    private void Update()
    {
        if (currentTarget != null)
        {
            //start attack corotuine
        }
    }
}

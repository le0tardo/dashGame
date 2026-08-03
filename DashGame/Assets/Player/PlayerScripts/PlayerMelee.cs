using System.Collections.Generic;
using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    [SerializeField] List<EnemyCombat> meleeTargets = new List<EnemyCombat>();
    [SerializeField] EnemyCombat meleeTarget=null;

    public bool meleeCombat=false;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            meleeTargets.Add(other.gameObject.GetComponent<EnemyCombat>());
            meleeCombat = true;
            meleeTarget = meleeTargets[0];
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            meleeTargets.Remove(other.gameObject.GetComponent<EnemyCombat>());
        }
        if (meleeTargets.Count <= 0)
        {
            meleeCombat=false;
        }
    }

}

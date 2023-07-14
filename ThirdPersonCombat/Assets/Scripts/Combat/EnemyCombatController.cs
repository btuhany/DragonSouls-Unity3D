using UnityEngine;
namespace Combat
{
    public class EnemyCombatController : MonoBehaviour
    {
        public Attack[] Attacks;
        [HideInInspector] public Attack CurrentAttack;

        [SerializeField] WeaponController _weapon;

        //Animation Events
        public void StartAttack()
        {
            _weapon.StartAttack(CurrentAttack.damage);
        }

        public void EndAttack()
        {
            _weapon.StopAttack();    
        }
        
    }
}

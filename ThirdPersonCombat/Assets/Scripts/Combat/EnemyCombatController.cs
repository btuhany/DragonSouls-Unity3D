using States;
using UnityEngine;
namespace Combat
{
    public class EnemyCombatController : MonoBehaviour
    {
        public Attack[] Attacks;
        [HideInInspector] public Attack CurrentAttack;

        [SerializeField] WeaponController _weapon;
        [SerializeField] ProjectileController[] _projectiles;
        [SerializeField] Transform _projectileSpawnPoint;

        //Animation Events
        public void StartAttack()
        {
            _weapon.StartAttack(CurrentAttack.damage);
        }

        public void EndAttack()
        {
            _weapon.StopAttack();    
        }

        public void SpawnProjectile(int projectileNum)
        {
            _projectiles[projectileNum].targetTransform = PlayerStateMachine.Instance.targetPointTransform;
            Instantiate(_projectiles[projectileNum], _projectileSpawnPoint.position, _projectileSpawnPoint.rotation);
        }
        
    }
}

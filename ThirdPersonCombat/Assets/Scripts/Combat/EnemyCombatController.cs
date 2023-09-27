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
        [SerializeField] Transform[] _projectileSpawnPoints;
        [SerializeField] WeaponController[] _weapons;
        

        //Animation Events
        public void StartAttack()
        {
            _weapon.StartAttack(CurrentAttack.damage);
        }
        public void StartAttackOf(int attackNum)
        {
            _weapon.StartAttack(Attacks[attackNum].damage);
        }
        public void EndAttack()
        {
            if(_weapon != null)
                _weapon.StopAttack();    
        }
        public void SetWeapon(int weaponNum)
        {
            _weapon = _weapons[weaponNum];
        }

        public void SpawnProjectile(int projectileNum)
        {
            _projectiles[projectileNum].targetTransform = PlayerStateMachine.Instance.targetPointTransform;
            Instantiate(_projectiles[projectileNum], _projectileSpawnPoints[projectileNum].position, Quaternion.identity);
        }

        
    }
}

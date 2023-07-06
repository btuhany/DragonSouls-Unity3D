using UnityEngine;
namespace Combat
{
    public class EnemyCombatController : MonoBehaviour
    {
        public Attack[] Attacks;
        [HideInInspector] public Attack CurrentAttack;

    }
}

using System;
using UnityEngine;

namespace PlayerController
{
    [Serializable]
    public struct Attack
    {
        public string animationName;
        public float animationTransitionDuration;
    }
    public class CombatController : MonoBehaviour
    {
        public Attack[] Attacks;
    }
}

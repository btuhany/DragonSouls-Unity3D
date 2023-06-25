using Inputs;
using PlayerController;
using UnityEngine;

namespace States
{
    public class PlayerStateMachine : StateMachine
    {
        [Header("Components")]
        public InputReader InputReader;
        public PlayerAnimationController AnimationController;
        public TargetableCheck TargetableCheck;
        public MovementController Movement;
        public CombatController CombatController;

        [HideInInspector] public PlayerFreeLookState FreeLookPlayerState;
        [HideInInspector] public PlayerCombatTargetState UnarmedTargetState;
        [HideInInspector] public PlayerCombatState UnarmedAttackTransitionState;

        private void Awake()
        {
            FreeLookPlayerState = new PlayerFreeLookState(this);
            UnarmedTargetState = new PlayerUnarmedTargetState(this);
            UnarmedAttackTransitionState = new PlayerUnarmedFreeState(this, Weapon.Unarmed, true);
        }
        private void OnEnable()
        {
            InputReader.JumpEvent += HandleOnDodgeEvent;
            InputReader.DodgeEvent += HandleOnDodgeEvent;
            InputReader.TargetEvent += HandleOnTargetEvent;
        }
        private void Start()
        {
            ChangeState(FreeLookPlayerState);
        }
        private void Update()
        {
            UpdateState(Time.deltaTime);

        }
        void HandleOnJumpEvent()
        {

        }
        void HandleOnDodgeEvent()
        {

        }
        void HandleOnTargetEvent()
        {

        }
    }
}

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
        [HideInInspector] public PlayerUnarmedTargetState UnarmedTargetState;
        [HideInInspector] public PlayerUnarmedFreeState UnarmedFreeTransitionState;
        [HideInInspector] public PlayerSwordFreeState SwordFreeState;
        [HideInInspector] public PlayerSwordTargetState SwordTargetState;
        [HideInInspector] public PlayerUnarmedFreeState UnarmedFreeState;
        private void Awake()
        {
            FreeLookPlayerState = new PlayerFreeLookState(this);
            UnarmedTargetState = new PlayerUnarmedTargetState(this);
            SwordTargetState = new PlayerSwordTargetState(this);
            SwordFreeState = new PlayerSwordFreeState(this);
            UnarmedFreeState = new PlayerUnarmedFreeState(this);
            UnarmedFreeTransitionState = new PlayerUnarmedFreeState(this, Weapon.Unarmed, true, true);
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

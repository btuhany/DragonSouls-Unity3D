
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
        [HideInInspector] public PlayerTargetState TargetPlayerState;
        [HideInInspector] public PlayerAttackingState LightAttackState;

        private void Awake()
        {
            FreeLookPlayerState = new PlayerFreeLookState(this);
            TargetPlayerState = new PlayerTargetState(this);
            LightAttackState = new PlayerAttackingState(this,AttackType.Light);
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

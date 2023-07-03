using Inputs;
using PlayerController;
using UnityEngine;
using Movement;
using Cinemachine;

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
        public ForceReceiver ForceReceiver;
        public Transform AimStateFocus;
        public CameraController CameraController;
        
        //States
        public PlayerFreeLookState FreeLookPlayerState;
        public PlayerUnarmedTargetState UnarmedTargetState;
        public PlayerUnarmedFreeState UnarmedFreeTransitionState;
        public PlayerSwordFreeState SwordFreeState;
        public PlayerSwordTargetState SwordTargetState;
        public PlayerUnarmedFreeState UnarmedFreeState;
        public PlayerAimState AimState;
        public PlayerSwordReturnState ReturnSwordState;
        private void Awake()
        {
            FreeLookPlayerState = new PlayerFreeLookState(this);
            UnarmedTargetState = new PlayerUnarmedTargetState(this);
            SwordTargetState = new PlayerSwordTargetState(this);
            SwordFreeState = new PlayerSwordFreeState(this);
            UnarmedFreeState = new PlayerUnarmedFreeState(this);
            UnarmedFreeTransitionState = new PlayerUnarmedFreeState(this, Weapon.Unarmed, true, true);
            AimState = new PlayerAimState(this);
            ReturnSwordState = new PlayerSwordReturnState(this);
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

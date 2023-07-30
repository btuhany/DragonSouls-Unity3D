using Inputs;
using PlayerController;
using UnityEngine;
using Movement;
using Combat;
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
        public CameraController CameraController;
        public Health Health;
        
        //States
        public PlayerFreeLookState FreeLookPlayerState;
        public PlayerUnarmedTargetState UnarmedTargetState;
        public PlayerUnarmedFreeState UnarmedFreeTransitionState;
        public PlayerSwordFreeState SwordFreeState;
        public PlayerSwordTargetState SwordTargetState;
        public PlayerUnarmedFreeState UnarmedFreeState;
        public PlayerAimState AimState;
        public PlayerSwordReturnState ReturnSwordState;
        public PlayerRollState RollState;

        public bool IsSprintHolding;
        public bool IsSprinting;
        public bool IsRoll = false;

        public static PlayerStateMachine Instance;
        public PlayerStateMachine()
        {
            Instance = this;
        }
        private void Awake()
        {
            SingletonObject();
            FreeLookPlayerState = new PlayerFreeLookState(this);
            UnarmedTargetState = new PlayerUnarmedTargetState(this);
            SwordTargetState = new PlayerSwordTargetState(this);
            SwordFreeState = new PlayerSwordFreeState(this);
            UnarmedFreeState = new PlayerUnarmedFreeState(this);
            UnarmedFreeTransitionState = new PlayerUnarmedFreeState(this, Weapon.Unarmed, true, true);
            AimState = new PlayerAimState(this);
            ReturnSwordState = new PlayerSwordReturnState(this);
            RollState = new PlayerRollState(this);
            Health = GetComponent<Health>();
        }

        private void SingletonObject()
        {
            if (Instance != null && Instance != this)
                Destroy(this.gameObject);
            else
                Instance = this;
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

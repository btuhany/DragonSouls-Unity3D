using Inputs;
using PlayerController;
using UnityEngine;
using Movement;
using Combat;
using System;
using Sounds;
using System.Xml;

namespace States
{
    public class PlayerStateMachine : StateMachine
    {
        [SerializeField] private ParticleSystem _healFX;
        [SerializeField] private int _healFlask = 3;

        [Header("Components")]
        public InputReader InputReader;
        public PlayerAnimationController animationController;
        public TargetableCheck targetableCheck;
        public MovementController movement;
        public CombatController combatController;
        public ForceReceiver forceReceiver;
        public CameraController cameraController;
        public Health health;
        public Stamina stamina;
        public PlayerSoundController sound;
        
        //States
        public PlayerFreeLookState freeLookPlayerState;
        public PlayerUnarmedTargetState unarmedTargetState;
        public PlayerUnarmedFreeState unarmedFreeTransitionState;
        public PlayerSwordFreeState swordFreeState;
        public PlayerSwordTargetState swordTargetState;
        public PlayerUnarmedFreeState unarmedFreeState;
        public PlayerAimState aimState;
        public PlayerSwordReturnState returnSwordState;
        public PlayerRollState rollState;
        public PlayerDeadState deadState;

        [HideInInspector] public bool isSprintHolding;
        [HideInInspector] public bool isSprinting;
        [HideInInspector] public bool isRoll = false;
        public Transform targetPointTransform;
        public static PlayerStateMachine Instance;
        public PlayerStateMachine()
        {
            Instance = this;
        }
        private void Awake()
        {
            SingletonObject();
            freeLookPlayerState = new PlayerFreeLookState(this);
            unarmedTargetState = new PlayerUnarmedTargetState(this);
            swordTargetState = new PlayerSwordTargetState(this);
            swordFreeState = new PlayerSwordFreeState(this);
            unarmedFreeState = new PlayerUnarmedFreeState(this);
            unarmedFreeTransitionState = new PlayerUnarmedFreeState(this, Weapon.Unarmed, true, true);
            aimState = new PlayerAimState(this);
            returnSwordState = new PlayerSwordReturnState(this);
            rollState = new PlayerRollState(this);
            deadState = new PlayerDeadState(this);
            health = GetComponent<Health>();
            stamina = GetComponent<Stamina>();
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
            this.health.OnHealthUpdated += HandleOnHealthUpdate;
            InputReader.JumpEvent += HandleOnDodgeEvent;
            InputReader.DodgeEvent += HandleOnDodgeEvent;
            InputReader.TargetEvent += HandleOnTargetEvent;
            InputReader.HealEvent += HandleOnHealEvent;
            health.OnHealthIncreased += HandleOnHealthIncreased;
            health.OnDead += HandleOnDead;
        }

        private void HandleOnHealEvent()
        {
            if (_healFlask <= 0) return;
            health.IncreaseHealth(10);
        }
        private void HandleOnHealthIncreased()
        {
            _healFlask--;
            _healFX.Play();
        }
        private void HandleOnDead()
        {
            ChangeState(deadState);
        }
        private void HandleOnHealthUpdate(int arg1, int arg2)
        {
            PlayerCombatState combat = _currentState as PlayerCombatState;
            if (combat != null && combat.IsAttacking) return;
            //if (UnityEngine.Random.Range(0, 11) > 5)
                sound.PlayHurtSFX();
            if (swordFreeState.IsAttacking || swordTargetState.IsAttacking || unarmedFreeState.IsAttacking || unarmedTargetState.IsAttacking)
                return;
            //animationController.PlayGetHit();
        }

        private void Start()
        {
            ChangeState(freeLookPlayerState);
        }
        private void Update()
        {
            if(health.IsDead) return;
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

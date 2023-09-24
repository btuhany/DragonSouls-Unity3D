using Inputs;
using PlayerController;
using UnityEngine;
using Movement;
using Combat;
using Sounds;

namespace States
{
    public class PlayerStateMachine : StateMachine
    {
        [SerializeField] private ParticleSystem _healFX;
        [SerializeField] private int _healFlask = 3;
        [HideInInspector] public Bonfire currentBonfire;

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
        public PlayerBonfireState bonfireState;

        [HideInInspector] public bool isSprintHolding;
        [HideInInspector] public bool isSprinting;
        [HideInInspector] public bool isRoll = false;
        public Transform targetPointTransform;
        public static PlayerStateMachine Instance;
        private Vector3 _initialPos;
        private Quaternion _initialRotation;
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
            bonfireState = new PlayerBonfireState(this);
            health = GetComponent<Health>();
            stamina = GetComponent<Stamina>();
            _initialPos = transform.position;
            _initialRotation = transform.rotation;
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
            InputReader.JumpEvent += HandleOnJumpEvent;
            InputReader.DodgeEvent += HandleOnDodgeEvent;
            InputReader.TargetEvent += HandleOnTargetEvent;
            InputReader.HealEvent += HandleOnHealEvent;
            InputReader.LightBonfireEvent += HandleOnLightBonfireEvent;
            InputReader.DodgeEvent += HandleOnDodgeEvent;
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
        private void HandleOnHealthUpdate(int health, int damage)
        {
            PlayerCombatState combat = _currentState as PlayerCombatState;
            if (combat != null && combat.IsAttacking) return;
            //if (UnityEngine.Random.Range(0, 11) > 5)
            if(damage != 0)
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
            if(health.IsDead && _currentState != deadState) return;
            UpdateState(Time.deltaTime);
        }
        void HandleOnLightBonfireEvent()
        {
            if (currentBonfire == null) return;

            if(currentBonfire.isLighted)
            {
                if(_currentState != aimState && _currentState != bonfireState)
                {
                    if (swordFreeState.IsAttacking || swordTargetState.IsAttacking || unarmedFreeState.IsAttacking || unarmedTargetState.IsAttacking)
                        return;
                    targetableCheck.ClearTarget();
                    currentBonfire.TakeRest();
                    ChangeState(bonfireState);
                }
            }
            else
            {
                currentBonfire.KindleBonfire();
            }
        }
        public void Respawn()
        {
            health.ResetHealth();
            if(BonfiresManager.Instance.lastInteractedBonfire == null)
            {
                transform.position = _initialPos;
                transform.rotation = _initialRotation;
            }
            else
            {
                transform.position = BonfiresManager.Instance.lastInteractedBonfire.respawnPoint.position;
            }
            ChangeState(freeLookPlayerState);

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

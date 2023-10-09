using Inputs;
using PlayerController;
using UnityEngine;
using Movement;
using Combat;
using Sounds;
using TMPro;
using DG.Tweening;
using Cinemachine;

namespace States
{
    public class PlayerStateMachine : StateMachine
    {
        [SerializeField] Material[] _armorMaterials;
        [SerializeField] SkinnedMeshRenderer[] _skinnedMeshes;
        private int _armorUpgradeCount = 0;
        [SerializeField] private ParticleSystem _healFX;
        [SerializeField] private int _initialHealFlask = 3;
        [SerializeField] private TextMeshProUGUI _healPotionText;
        [HideInInspector] public Bonfire currentBonfire;
        [SerializeField] private int _healAmount;
        [SerializeField] private GameObject _pauseScreen;
        private int _healFlask = 3;
        public event System.Action OnPlayerRespawn;
        public event System.Action OnPlayerTeleported;
        public bool isInvisible = false;
        [SerializeField] private CinemachineCollider _cinemachineCollider;

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
        private void Awake()
        {
            SingletonThis();
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
            BonfiresManager.Instance.OnTakeRestEvent += LookToBonfire;
            health.OnArmorUpgrade += HandleOnArmorUprade;
            _healFlask = _initialHealFlask;
        }

        private void SingletonThis()
        {
            if (Instance != null && Instance != this)
                Destroy(this.gameObject);
            else
                Instance = this;
        }

        private void OnEnable()
        {
            ResetSetHealFlask();
            this.health.OnHealthUpdated += HandleOnHealthUpdate;
            InputReader.JumpEvent += HandleOnJumpEvent;
            InputReader.DodgeEvent += HandleOnDodgeEvent;
            InputReader.TargetEvent += HandleOnTargetEvent;
            InputReader.HealEvent += HandleOnHealEvent;
            InputReader.LightBonfireEvent += HandleOnLightBonfireEvent;
            InputReader.DodgeEvent += HandleOnDodgeEvent;
            health.OnHealthIncreased += HandleOnHealthIncreased;
            health.OnDead += HandleOnDead;
            InputReader.PauseEvent += HandleOnPause;
        }

        private void Start()
        {
            ChangeState(freeLookPlayerState);
            SoundManager.Instance.PlayAuidoClip(3, 0);
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
                    if (targetableCheck.IsEnemyNearby(6f)) return;
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
            movement.CharacterController.enabled = false;
            if(BonfiresManager.Instance.LastInteractedBonfire == null)
            {
                //movement.CharacterController.Move(_initialPos - transform.position);
                transform.position = _initialPos;

            }
            else
            {
                transform.position = BonfiresManager.Instance.LastInteractedBonfire.respawnPoint.position;
                //movement.CharacterController.Move(BonfiresManager.Instance.LastInteractedBonfire.respawnPoint.position - transform.position);
            }
            transform.rotation = _initialRotation;
            movement.CharacterController.enabled = true;

            if (combatController.IsSwordInSheath)
                ChangeState(freeLookPlayerState);
            else if (combatController.IsSwordReturned)
                ChangeState(swordFreeState);
            else
                ChangeState(unarmedFreeState);
            ResetSetHealFlask();
            OnPlayerRespawn?.Invoke();
        }
        public void TeleportTo(Vector3 pos)
        {
            _cinemachineCollider.enabled = false;
            OnPlayerTeleported?.Invoke();
            health.ResetHealth();
            movement.CharacterController.enabled = false;
            forceReceiver.disableForce = true;
            transform.DOMove(transform.position + Vector3.up * 180f, 1f).onComplete = () =>
            {
                transform.DOMove(new Vector3(pos.x, transform.position.y, pos.z), 1f).onComplete = () => 
                {
                    transform.position = pos + Vector3.up;
                    transform.rotation = _initialRotation;
                    forceReceiver.disableForce = false;
                    movement.CharacterController.enabled = true;
                    if (combatController.IsSwordInSheath)
                        ChangeState(freeLookPlayerState);
                    else if (combatController.IsSwordReturned)
                        ChangeState(swordFreeState);
                    else
                        ChangeState(unarmedFreeState);
                    ResetSetHealFlask();
                    _cinemachineCollider.enabled = true;
                };
            };
        }
        private void LookToBonfire()
        {
            Vector3 targetDir = BonfiresManager.Instance.LastInteractedBonfire.transform.position - transform.position;
            targetDir.y = 0f;
            transform.rotation = Quaternion.LookRotation(
            targetDir, Vector3.up);
        }
        private void HandleOnArmorUprade()
        {
            _armorUpgradeCount++;
            if (_armorUpgradeCount == 1)
            {
                foreach (SkinnedMeshRenderer skinnedMeshRenderer in _skinnedMeshes)
                {
                    skinnedMeshRenderer.material = _armorMaterials[0];
                }
            }
            else if (_armorUpgradeCount == 2)
            {
                foreach (SkinnedMeshRenderer skinnedMeshRenderer in _skinnedMeshes)
                {
                    skinnedMeshRenderer.material = _armorMaterials[1];
                }
            }
        }
        private void HandleOnPause()
        {
            _pauseScreen.SetActive(!_pauseScreen.activeSelf);
        }

        private void HandleOnHealEvent()
        {
            if (_healFlask <= 0) return;
            health.IncreaseHealth(_healAmount);
        }
        private void HandleOnHealthIncreased()
        {
            _healFlask--;
            _healPotionText.text = _healFlask.ToString();
            _healFX.Play();
            sound.PlayHealSFX();
        }
        public void ResetSetHealFlask()
        {
            _healFlask = _initialHealFlask;
            _healPotionText.text = _healFlask.ToString();
        }
        private void HandleOnDead()
        {
            ChangeState(deadState);
            SoulsManager.Instance.ResetSouls();
            if (BossManager.Instance.IsInBoss)
                BossManager.Instance.ExitBossFight();
        }
        private void HandleOnHealthUpdate(int health, int damage)
        {
            PlayerCombatState combat = _currentState as PlayerCombatState;
            if (combat != null && combat.IsAttacking) return;
            //if (UnityEngine.Random.Range(0, 11) > 5)
            if (damage != 0)
                sound.PlayHurtSFX();
            if (swordFreeState.IsAttacking || swordTargetState.IsAttacking || unarmedFreeState.IsAttacking || unarmedTargetState.IsAttacking)
                return;
            //animationController.PlayGetHit();
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

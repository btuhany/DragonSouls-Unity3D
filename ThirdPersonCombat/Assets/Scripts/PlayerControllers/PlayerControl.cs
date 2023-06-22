using UnityEngine;
using Inputs;
using States;
using PlayerController;

namespace PlayerControllers
{
    //This class is being used as a data transmitter.
    public class PlayerControl : MonoBehaviour  
    {
        [Header("FreeLookState")]
        public float MovementSpeed;
        public float FaceDirectionRotationLerpTimeScale = 3f;
        [Header("TargetState")]
        public float TargetDirectionRotationLerpTimeScale = 2f;

        [Header("Scripts")]
        public InputReader InputReader;
        public CharacterController CharacterController;
        public PlayerAnimationController AnimationController; 
        public PlayerStateMachine StateMachine;
        public TargetableCheck TargetableCheck;
        

        [HideInInspector] public PlayerFreeLookState FreeLookPlayerState; 
        [HideInInspector] public PlayerTargetState TargetPlayerState;
        private void Awake()
        {
            StateMachine = new PlayerStateMachine();
            FreeLookPlayerState = new PlayerFreeLookState(this);
            TargetPlayerState = new PlayerTargetState(this);
        }
        private void OnEnable()
        {
            InputReader.JumpEvent += HandleOnDodgeEvent;
            InputReader.DodgeEvent += HandleOnDodgeEvent;
            InputReader.TargetEvent += HandleOnTargetEvent;
        }
        private void Start()
        {
            StateMachine.ChangeState(FreeLookPlayerState);
        }
        private void Update()
        {
            StateMachine.UpdateState(Time.deltaTime);

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


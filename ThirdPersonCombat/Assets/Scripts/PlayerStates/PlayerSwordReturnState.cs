using PlayerController;
using States;
using UnityEngine;
using Combat;
public class PlayerSwordReturnState : PlayerBaseState
{
    private CombatController _combat;
    private Transform targetTransform;
    bool _isTargeted = false;

    public PlayerSwordReturnState(PlayerStateMachine player) : base(player)
    {
        _combat = player.CombatController;
    }
    public override void Enter()
    {
        _combat.TryReturnSword();
        animationController.PlaySwordReturn();

        if(stateMachine.PreviousState == stateMachine.UnarmedTargetState)
        {
            if (targetableCheck.TryTransferTarget())
            {
                targetTransform = targetableCheck.CurrentTargetTransform;
                _isTargeted = true;
            }
        }

        base.Enter();
    }
    public override void Tick(float deltaTime)
    {
        Vector2 movementOn2DAxis = inputReader.MovementOn2DAxis;
        animationController.TargetStateSetFloats(inputReader.MovementOn2DAxis);

        //Cinemachine IsBlending doesn't work properly at start
        if (!_isTargeted && stateMachine.CameraController.IsTargetCamActive) return;

        if (_isTargeted)
        {
            
            RotateCharacter(movement.TargetRelativeMotionVector(targetTransform.position), deltaTime);
            MoveCharacter(MotionVectorAroundTarget(), movement.TargetMovementSpeed, deltaTime);
        }
        else
        {
            if (IsSprintHold || IsSprint)
            {
                MoveCharacter(movement.CamRelativeMotionVector(movementOn2DAxis.normalized), movement.ReturnSwordRunMovementSpeed, deltaTime);
            }
            else
            {
                MoveCharacter(movement.CamRelativeMotionVector(movementOn2DAxis), movement.ReturnSwordMovementSpeed, deltaTime);
            }


            if (movementOn2DAxis.magnitude > 0f)
            {
                if (stateMachine.CameraController.IsTransition)
                {
                    return;
                }
                RotateCharacter(movement.CamRelativeMotionVector(movementOn2DAxis), deltaTime);
            }
        }


        HandleSprintControl();
        if(_combat.IsSwordReturned)
        {
            if (_isTargeted)
                stateMachine.ChangeState(stateMachine.SwordTargetState);
            else
                stateMachine.ChangeState(stateMachine.SwordFreeState);
        }
    }
    public override void Exit()
    {
        _isTargeted = false;
        targetTransform = null;
        targetableCheck.ClearTarget();
        base.Exit();
    }

    protected override void HandleOnHeavyAttackEvent()
    {
    }

    protected override void HandleOnLightAttackEvent()
    {
    }

    protected override void HandleOnTargetEvent()
    {
        if (_isTargeted)
        {
            _isTargeted = false;
            targetTransform = null;
            targetableCheck.ClearTarget();
            animationController.UntargetedAnimation();//for state driven camera
        }
        else
        {
            if (!targetableCheck.TrySelectTarget()) return;
            targetTransform = targetableCheck.CurrentTargetTransform;
            animationController.TargetedAnimation();
            _isTargeted = true;
        }
    }

    protected override void HandleSheathEvent()
    {
    }
    private Vector3 MotionVectorAroundTarget()
    {
        //Character always looks to target
        Vector3 motion = Vector3.zero;
        motion += transform.right * inputReader.MovementOn2DAxis.x;
        motion += transform.forward * inputReader.MovementOn2DAxis.y;
        return motion;
    }
}

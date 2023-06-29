using PlayerController;
using States;
using UnityEngine;

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
        animationController.PlaySwordReturn();
        _combat.TryReturnSword();

        base.Enter();
    }
    public override void Tick(float deltaTime)
    {
        Vector2 movementOn2DAxis = inputReader.MovementOn2DAxis;
        animationController.TargetStateSetFloats(inputReader.MovementOn2DAxis);

        if (_isTargeted)
        {
            if (isSprintHold || isSprint)
            {
                RotateCharacter(movement.CamRelativeMotionVector(inputReader.MovementOn2DAxis), deltaTime);
                MoveCharacter(movement.CamRelativeMotionVector(inputReader.MovementOn2DAxis), movement.TargetRunSpeed, deltaTime);
            }
            else
            {
                Vector3 relativeVector = targetTransform.position - Camera.main.transform.position;
                relativeVector.y = 0f;
                RotateCharacter(relativeVector, deltaTime);
                MoveCharacter(MotionVectorAroundTarget(), movement.TargetMovementSpeed, deltaTime);
            }
        }
        else
        {
            if (isSprintHold || isSprint)
            {
                MoveCharacter(movement.CamRelativeMotionVector(movementOn2DAxis.normalized), movement.ReturnSwordMovementSpeed, deltaTime);
            }
            else
            {
                MoveCharacter(movement.CamRelativeMotionVector(movementOn2DAxis), movement.ReturnSwordMovementSpeed, deltaTime);
            }


            if (movementOn2DAxis.magnitude > 0f)
            {
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
            animationController.FreeCombat(Weapon.Sword);
            animationController.PlaySwordReturn();
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

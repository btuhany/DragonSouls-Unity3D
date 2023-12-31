using DG.Tweening;
using States;
using UnityEngine;

public class FaceToPlayer : ActionNode
{
    public float rotateSpeed = 35f;
    public float animCrossFade = 0.1f;
    public float chaseRotationLerpTime = 2f;
    private float _timeCounter;
    private float _animationTimeCounter;

    private readonly int _animLeftStare = Animator.StringToHash("leftstare");
    private readonly int _animRightStare = Animator.StringToHash("rightstare");

    private bool _isStartRotationEnded = false;
    private bool _animationPlayed = false;
    protected override void OnStart()
    {
        _timeCounter = 0f;
        _animationTimeCounter = 0f;
        PlayTurnAnimation();

        if (agent.isFaceToPlayer) return;
        Vector3 dir = agent.playerTransform.position - agent.transform.position;
        dir.y = 0f;

        //Variable rotatingTime according to turning distance
        float similarity = Mathf.Abs(Vector3.Dot(dir.normalized, agent.transform.forward));
        if (similarity < 0.5f)
        {
            agent.transform.DORotateQuaternion(Quaternion.LookRotation(dir), 500 * similarity / rotateSpeed).SetEase(Ease.InOutBack).onComplete = () =>
            {
                _isStartRotationEnded = true;
            };
        }
        else
        {
            agent.transform.DORotateQuaternion(Quaternion.LookRotation(dir), 200 * similarity / rotateSpeed).SetEase(Ease.InOutBack).onComplete = () =>
            {
                _isStartRotationEnded = true;
            };
        }
    }

    private void PlayTurnAnimation()
    {
        if (_animationPlayed) return;
        _animationPlayed = true;
        Vector3 targetRelativePoint = agent.transform.InverseTransformPoint(agent.playerTransform.position);
        if (targetRelativePoint.x <= 0)
        {
            agent.animator.CrossFadeInFixedTime(_animLeftStare, animCrossFade);
        }
        else
        {
            agent.animator.CrossFadeInFixedTime(_animRightStare, animCrossFade);
        }
    }

    protected override void OnStop()
    {
        _isStartRotationEnded = false;
    }

    protected override State OnUpdate()
    {
        if (agent.isFaceToPlayer) return State.Success;
        if (_animationPlayed)
        {
            _animationTimeCounter += Time.deltaTime;
            if(_animationTimeCounter > 0.6f)
            {
                _animationTimeCounter = 0f;
                _animationPlayed = false;
            }
        }
        if (agent.playerTransform == null)
            return State.Failure;

        if (IsFaceToPlayer())
            return State.Success;
        else
        {
            if (_isStartRotationEnded)
            {
                Vector3 dir = agent.playerTransform.position - agent.transform.position;
                dir.y = 0f;
                agent.locomotion.LookRotation(dir, chaseRotationLerpTime, Time.deltaTime);
                _timeCounter += Time.deltaTime;
                _animationTimeCounter += Time.deltaTime;
                if (_timeCounter > 0.6f)
                {
                    _timeCounter = 0f;
                    PlayTurnAnimation();
                }
            }
            return State.Running;
        }
    }
    private bool IsFaceToPlayer()
    {
        Vector3 dir = PlayerStateMachine.Instance.transform.position - agent.transform.position;
        dir.y = 0f;
        float similarity = Vector3.Dot(dir.normalized, agent.transform.forward);
        if (similarity > 0.99f)
            return true;
        return false;
    }

}

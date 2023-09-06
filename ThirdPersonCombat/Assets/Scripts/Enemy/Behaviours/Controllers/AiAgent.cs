using Combat;
using EnemyControllers;
using Sounds;
using States;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AiAgent : MonoBehaviour
{
    [HideInInspector] public NavMeshAgent navmeshAgent;
    [HideInInspector] public CharacterController characterController;
    [HideInInspector] public AiLocomotion locomotion;
    [HideInInspector] public Animator animator;
    [HideInInspector] public EnemyForceReceiver forceReceiver;
    [HideInInspector] public EnemyCombatController combat;
    [HideInInspector] public Health health;

    [HideInInspector] public Transform playerTransform;
    [HideInInspector] public bool faceToPlayer;
    [HideInInspector] public float faceLerpTime = 2f;
    [HideInInspector] public bool reactToHit;

    [SerializeField] private GameObject _agentFX;
    [SerializeField] SoundClips[] _sfxClips;
    [SerializeField] private AudioSource _audioSource;

    private BehaviourTreeRunner _treeRunner;
    public bool isFaceToPlayer => IsFaceToPlayer();

    private readonly int _animGotHit = Animator.StringToHash("Hit1");
    private readonly int _animDead = Animator.StringToHash("death");
    private void Awake()
    {
        navmeshAgent = GetComponent<NavMeshAgent>();
        characterController = GetComponent<CharacterController>();
        locomotion = GetComponent<AiLocomotion>();
        animator = GetComponent<Animator>();
        forceReceiver = GetComponent<EnemyForceReceiver>();
        combat = GetComponent<EnemyCombatController>();
        health = GetComponent<Health>();
        _treeRunner = GetComponent<BehaviourTreeRunner>();
        _audioSource = GetComponent<AudioSource>();
        playerTransform = PlayerStateMachine.Instance.transform;
    }

    private void OnEnable()
    {
        _treeRunner.stop = false;
        navmeshAgent.isStopped = true;
        health.OnHealthUpdated += HandleOnTakeHit;
    }
    private void Update()
    {
        if (faceToPlayer)
        {
            //Debug.Log("face");
            FaceToPlayer();
        }
    }
    private void OnDisable()
    {
        health.OnHealthUpdated -= HandleOnTakeHit;
    }
    private void HandleOnTakeHit(int health, int damage)
    {
        if(reactToHit && _agentFX != null)
        {
            if(_agentFX.activeSelf)
                _agentFX.SetActive(false);
            animator.CrossFadeInFixedTime(_animGotHit, 0.1f);
            _treeRunner.stop = true;
        }
        if(health <= 0)
        {
            _treeRunner.stop = true;
            animator.CrossFadeInFixedTime(_animDead, 0.1f);
            Destroy(gameObject, 2f);
        }
        else
        {
            _treeRunner.Tree.blackboard.isHit = true;
            StopAllCoroutines();
            StartCoroutine(ResetIsHit());
        }
    }
    WaitForSeconds _resetIsHitTime = new WaitForSeconds(0.2f);
    private IEnumerator ResetIsHit()
    {
        yield return _resetIsHitTime;
        _treeRunner.Tree.blackboard.isHit = false;
        _treeRunner.stop = false;
        yield return null;
    }
    private void FaceToPlayer()
    {
        if (IsFaceToPlayer()) return;
        Vector3 dir = playerTransform.position - transform.position;
        dir.y = 0f;
        locomotion.LookRotation(dir, faceLerpTime, Time.deltaTime);
    }

    private bool IsFaceToPlayer()
    {
        Vector3 dir = PlayerStateMachine.Instance.transform.position - transform.position;
        dir.y = 0f;
        float similarity = Vector3.Dot(dir.normalized, transform.forward);
        if (similarity > 0.99f)
            return true;
        return false;
    }

    //Animation Event
    public void PlaySFX(int sfxNum)
    {
        _audioSource.pitch = _sfxClips[sfxNum].Pitch;
        _audioSource.volume = _sfxClips[sfxNum].Volume;
        _audioSource.PlayOneShot(_sfxClips[sfxNum].AudioClips[0]);
    }
}

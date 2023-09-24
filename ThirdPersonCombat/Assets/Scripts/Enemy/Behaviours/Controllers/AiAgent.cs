using Combat;
using EnemyControllers;
using Movement;
using Sounds;
using States;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AiAgent : MonoBehaviour
{
    [SerializeField] private int _soulPoint = 10;
    [SerializeField] private float _destroyTime = 2f;
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
    [SerializeField] private GameObject[] _trailRenderers;
    [SerializeField] SoundClips[] _sfxClips;
    private AudioSource[] _audioSources;

    private BehaviourTreeRunner _treeRunner;
    private bool _isDead;
    public bool isFaceToPlayer => IsFaceToPlayer();

    private readonly int _animGotHit = Animator.StringToHash("Hit1");
    private readonly int _animDead = Animator.StringToHash("death");
    private Vector3 _initialWorldPos;
    private Quaternion _initialWorldRotation;
    private WaitForSeconds _disableDelay;
    private void Awake()
    {
        _disableDelay = new WaitForSeconds(_destroyTime);
        navmeshAgent = GetComponent<NavMeshAgent>();
        characterController = GetComponent<CharacterController>();
        locomotion = GetComponent<AiLocomotion>();
        animator = GetComponent<Animator>();
        forceReceiver = GetComponent<EnemyForceReceiver>();
        combat = GetComponent<EnemyCombatController>();
        health = GetComponent<Health>();
        _treeRunner = GetComponent<BehaviourTreeRunner>();
        _audioSources = GetComponents<AudioSource>();
        playerTransform = PlayerStateMachine.Instance.transform;
        BonfiresManager.Instance.OnTakeRestEvent += Respawn;
        _initialWorldPos = transform.position;
        _initialWorldRotation = transform.rotation;
    }

    private void OnEnable()
    {
        _treeRunner.stop = false;
        forceReceiver.isCharacterControllerDisabled = false;
        health.OnHealthUpdated += HandleOnTakeHit;
        _isDead = false;
    }
    private void Start()
    {
        navmeshAgent.isStopped = true;
        
    }
    private void Update()
    {
        if (faceToPlayer)
        {
            FaceToPlayer();
        }
    }
    private void OnDisable()
    {
        health.OnHealthUpdated -= HandleOnTakeHit;
        _isDead = false;
    }
    private void HandleOnTakeHit(int health, int damage)
    {
        if(reactToHit)
        {
            combat.EndAttack();
            if (_agentFX != null)
                if(_agentFX.activeSelf)
                _agentFX.SetActive(false);
            animator.CrossFadeInFixedTime(_animGotHit, 0.1f);
            _treeRunner.stop = true;
        }
        if(health <= 0)
        {
            _isDead = true;
            SoulsManager.Instance.AddSoul(_soulPoint, this.transform.position);
            _treeRunner.stop = true;
            faceToPlayer = false;
            animator.CrossFadeInFixedTime(_animDead, 0.1f);
            //Destroy(gameObject, _destroyTime);
            StopAllCoroutines();
            StartCoroutine(DisableThisWithDelay());
        }
        else
        {
            _treeRunner.Tree.blackboard.isHit = true;
            StopAllCoroutines();
            StartCoroutine(ResetIsHit());
        }
    }

    private IEnumerator DisableThisWithDelay()
    {
        yield return _disableDelay;
        this.gameObject.SetActive(false);
        yield return null;
    }

    WaitForSeconds _resetIsHitTime = new WaitForSeconds(0.2f);
    private IEnumerator ResetIsHit()
    {
        yield return _resetIsHitTime;
        //_treeRunner.Tree.blackboard.isHit = false;
        if(!_isDead)
            _treeRunner.stop = false;
        yield return null;
    }
    private void FaceToPlayer()
    {
        //if (IsFaceToPlayer()) return;
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
    public Vector3 RandomPointOnNavMesh(Vector3 center, float range, float samplePointRange)
    {
        Vector3 randomVector = Random.insideUnitSphere;
        //randomVector.y = 0f;
        
        Vector3 randomPoint = center + randomVector * range;
        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomPoint, out hit, samplePointRange, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return center;
    }

    public void SetSecondaryAuidoSource(bool isPlay)
    {
        if(isPlay)
        {
            _audioSources[1].Play();
        }
        else
        {
            _audioSources[1].Stop();
        }
    }
    public void SetTrailRenderer(bool active)
    {
        foreach (GameObject trailRenderer in _trailRenderers)
        {
            trailRenderer.SetActive(active);
        }
    }
    public void Respawn()
    {
        _isDead = false;
        _treeRunner.stop = false;
        forceReceiver.isCharacterControllerDisabled = true;
        health.ResetHealth();
        this.gameObject.SetActive(false);
        transform.position = _initialWorldPos;
        transform.rotation = _initialWorldRotation;
        this.gameObject.SetActive(true);
        navmeshAgent.isStopped = true;
    }
    //Animation Events
    public void PlaySFX(int sfxNum)
    {
        _audioSources[0].pitch = _sfxClips[sfxNum].Pitch;
        _audioSources[0].volume = _sfxClips[sfxNum].Volume;
        _audioSources[0].PlayOneShot(_sfxClips[sfxNum].AudioClips[0]);
    }
    public void PlayRandomClipSFX(int sfxNum)
    {
        _audioSources[0].pitch = _sfxClips[sfxNum].Pitch;
        _audioSources[0].volume = _sfxClips[sfxNum].Volume;
        _audioSources[0].PlayOneShot(_sfxClips[sfxNum].AudioClips[Random.Range(0, _sfxClips[sfxNum].AudioClips.Length)]);
    }
}

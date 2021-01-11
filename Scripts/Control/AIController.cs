using UnityEngine;
using UnityEngine.AI;

using ZAM.Movement;
using ZAM.Combat;
using ZAM.Attributes;

namespace ZAM.Control
{
    public class AIController : MonoBehaviour
    {
        // Assigned Values \\
        [Range(0,1)]
        [SerializeField] private float _patrolSpeed = 0.4f;
        [Range(0,1)]
        [SerializeField] private float _chaseSpeed = 0.75f;
        [SerializeField] private float _chaseDistance = 5f;

        [SerializeField] private float _suspicionWaitTime = 5f;
        [SerializeField] private float _patrolPauseTime = 3f;
        [SerializeField] private bool _isPatrol = false;
        [SerializeField] PatrolPath patrolPath = null;

        [SerializeField] private float _shoutRadius = 10;

        // Setup Variables \\
        GameObject playerObject;
        CombatTarget playerTarget;
        NavMeshAgent navMeshAgent;
        Mover aiMover;
        Fight aiFight;
        Health aiHealth;
        
        // Adjustable Variables \\
        private Vector3 _returnPosition;
        private float _returnRotationY;
        private int _currentWaypoint = 0;
        private float waypointTolerance = 1f;
        private float _lastSawPlayer = 0f;
        private float _waypointTimePaused = 0f;

        private bool _calledForHelp = false;

        // Base Methods - Unity \\
        private void Awake()
        {
            _lastSawPlayer = _suspicionWaitTime;
            _returnPosition = transform.position;
            _returnRotationY = transform.rotation.y;

            navMeshAgent = GetComponent<NavMeshAgent>();
            aiMover = GetComponent<Mover>();
            aiFight = GetComponent<Fight>();
            aiHealth = GetComponent<Health>();
        }

        private void Start()
        {
            playerObject = GameObject.FindWithTag("Player");
            playerTarget = playerObject.GetComponent<CombatTarget>();
        }
        
        private void FixedUpdate()
        {
            if (aiHealth.IsDead()) { return; }

            if (ShouldEngage())
            {
                EngagePlayer();
            }
            else if (_lastSawPlayer < _suspicionWaitTime)
            {
                SuspicionTime();
            }
            else
            {
                if (_isPatrol) { PatrolBehaviour(); }
                else { GuardBehaviour(); }
            }

            if (!ShouldEngage())
            {
                _calledForHelp = false;
                _lastSawPlayer += Time.deltaTime;
            }
        }

        // Behaviour Methods \\
        private void EngagePlayer()
        {
            _lastSawPlayer = 0;
            navMeshAgent.speed = _chaseSpeed;
            aiFight.StartAttack(playerTarget);
            
            if (_calledForHelp == false) { AllEngage(); }
            aiFight.SetRetaliate((false));
        }

        private void SuspicionTime()
        {
            _waypointTimePaused = _patrolPauseTime;
            aiFight.Cancel();
        }

        private bool ShouldEngage()
        {
            if (playerTarget.IsDead()) { return false; }
            float distanceToPlayer = Vector3.Distance(playerTarget.transform.position, transform.position);

            return (aiFight.ShouldRetaliate() || distanceToPlayer <= _chaseDistance);
        }

        private void AllEngage()
        {
            RaycastHit[] nearAllies = Physics.SphereCastAll(transform.position, _shoutRadius, Vector3.up, 0);

            foreach (RaycastHit hit in nearAllies)
            {
                AIController ally = hit.collider.GetComponent<AIController>();
                if (ally == null) { continue; }

                ally.aiFight.SetRetaliate(true);
                ally._calledForHelp = true;
            }
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = _returnPosition;

            if (patrolPath != null)
            {
                if (AtWaypoint(_currentWaypoint))
                {
                    _waypointTimePaused += Time.deltaTime;

                    if (_waypointTimePaused >=  _patrolPauseTime) 
                    { 
                        CycleWaypoint(); 
                        _waypointTimePaused = 0;
                    }
                }
                nextPosition = GetCurrentWaypoint();

            }

            aiFight.Cancel();
            aiMover.MoveTo(nextPosition, _patrolSpeed);
        }

        private void GuardBehaviour()
        {
            if (AtWaypoint(_currentWaypoint) && navMeshAgent.velocity == new Vector3(0, 0, 0))
            {
                aiMover.GetLookDirection(patrolPath.GetWaypoint(_currentWaypoint + 1));
            }
            else { aiMover.MoveTo(_returnPosition, _patrolSpeed); }
        }

        // Pathing Methods \\
        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(_currentWaypoint);
        }
        
        private void CycleWaypoint()
        {
            _currentWaypoint = patrolPath.GetNextIndex(_currentWaypoint);
        }

        private bool AtWaypoint(int waypoint)
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return (distanceToWaypoint < waypointTolerance);
        }

        // Unity Editor Methods \\
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, _chaseDistance);
        }
    }
}
using UnityEngine;
using UnityEngine.AI;

using ZAM.Core;
using ZAM.Attributes;
using ZAM.Saving;

namespace ZAM.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        // Assigned Variables \\
        [SerializeField] private float _rotationSpeed = 4f;
        [SerializeField] private float _maxSpeed = 6f;

        // Setup Variables
        NavMeshAgent navMeshAgent;
        Health health;

        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
        }

        private void Update()
        {
            navMeshAgent.enabled = !health.IsDead();
            UpdateMoveAnimator();
        }
        
        public void UpdateMoveAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("Forward Speed", speed);
        }

        public void MoveAction(Vector3 destination, float speedPercent)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedPercent);
        }

        public void MoveTo(Vector3 destination, float speedPercent)
        {
            navMeshAgent.speed = _maxSpeed * Mathf.Clamp01(speedPercent);
            navMeshAgent.destination = destination;
            navMeshAgent.isStopped = false;
        }
        
        public void Cancel()
        {
            navMeshAgent.isStopped = true;
            navMeshAgent.ResetPath();
        }

        public void GetLookDirection(Vector3 lookTarget)
        {
            Quaternion rotation = Quaternion.LookRotation(lookTarget - transform.position);
            rotation.x = 0;
            rotation.z = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * _rotationSpeed);
        }

        public void GetLookDirection(Vector3 lookTarget, float multiplyRotate)
        {
            Quaternion rotation = Quaternion.LookRotation(lookTarget - transform.position);
            rotation.x = 0;
            rotation.z = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * (_rotationSpeed * multiplyRotate));
        }

        // Save Component \\
        [System.Serializable]
        struct MoverSaveData
        {
            public SerializableVector3 position;
            public SerializableVector3 rotation;
        }

        public object CaptureState()
        {
            MoverSaveData data = new MoverSaveData();
            data.position = new SerializableVector3(transform.position);
            data.rotation = new SerializableVector3(transform.eulerAngles);

            return data;
        }

        public void RestoreState(object state)
        {
            MoverSaveData data = (MoverSaveData)state;
            GetComponent<NavMeshAgent>().Warp(data.position.ToVector());
            transform.eulerAngles = data.rotation.ToVector();

            // GetComponent<NavMeshAgent>().enabled = false;
            // transform.position = position.ToVector();
            // GetComponent<NavMeshAgent>().enabled = true;
        }
    } 
}


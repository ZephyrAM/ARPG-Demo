using System;

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

using ZAM.Movement;
using ZAM.Combat;
using ZAM.Attributes;

namespace ZAM.Control
{
    public class PlayerController : MonoBehaviour, InputManager.IPlayerControllerActions
    {
        // Assigned Values \\
        [SerializeField] private float _rotationMultiplier = 100f;
        [SerializeField] private float _baseSpeed = 1f;
        [SerializeField] private bool _isAutoAttack = true;

        [SerializeField] private float maxNavCheckDistance = 1f;
        [SerializeField] private float maxPathLength = 40f;

        [SerializeField] CursorMapping[] cursorMappings = null;

        // Setup Variables \\
        InputManager controlInput;
        NavMeshAgent navMeshAgent;
        Animator animator;

        Mover playerMover;
        Fight playerFight;
        Health playerHealth;
        CombatTarget target;
        CombatTarget heldTarget;
        WeaponPickup weaponPickup;

        enum CursorType
        {
            None,
            Movement,
            Combat,
            UI,
            Interactable
        }

        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        // Adjustable Variables \\
        private bool _stopMove;
        private bool _justStop;
        private bool _mouseDown;
        //private bool _mouseClick;

        //Vector3 clickPoint;
        //Vector3 lookPoint;
        Vector3 moveInput;

        //private bool _rayHitPickup = false;

        struct RaycastCheck
        {
            //public RaycastHit hitRay;
            public RaycastHit[] hitRayAll;
            public bool hitCheck;
            public Vector3 hitPoint;
            public NavMeshHit hitNavPoint;
            public bool hitNavCheck;
        }

        struct AllTargetCheck
        {
            public CombatTarget hitTarget;
            public CombatTarget hitHeldTarget;
            public WeaponPickup hitPickup;
        }

        struct NavTestPath
        {
            public NavMeshPath navPath;
            public NavMeshHit navPathEnd;
            public bool navPathCheck;
            public float navPathDistance;
        }
        
        RaycastCheck testCursor;
        AllTargetCheck testCursorAll;
        NavTestPath testNavPath;


        // ------------------------------------------------------------------


        // Base Methods - Unity \\
        private void Awake()
        {
            controlInput = new InputManager();
            controlInput.PlayerController.SetCallbacks(this);

            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();

            playerMover = GetComponent<Mover>();
            playerFight = GetComponent<Fight>();
            playerHealth = GetComponent<Health>();
        }

        private void OnEnable()
        {
            controlInput.PlayerController.Enable();
        }

        private void OnDisable()
        {
            controlInput.PlayerController.Disable();
        }

        private void FixedUpdate()
        {
            if (CursorLogic()) { return; }
            if (playerHealth.IsDead()) 
            { 
                SetCursor(CursorType.None);
                return; 
            }

            AttackCheck();

            AxisCheck();
            MoveCheck();
        }


        // ------------------------------------------------------------------


        // Cursor & Raycast Methods \\
        private bool CursorLogic()
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) 
            { 
                SetCursor(CursorType.UI);
                return true; 
            }
            else 
            {
                RunAllCursorTests();
                // testCursor = CursorCheck();
                // testCursorAll = CursorTargetCheck();

                if (testCursorAll.hitTarget) { SetCursor(CursorType.Combat); }
                else if (testCursorAll.hitPickup && testCursorAll.hitPickup.transform.GetChild(0).gameObject.activeSelf) { SetCursor(CursorType.Interactable); }
                else if (testCursor.hitNavCheck) { SetCursor(CursorType.Movement); }
                else { SetCursor(CursorType.None); }
                return false;
            }
            
        }

        // private RaycastCheck CursorCheck()
        // {
        //     Vector3 thisPoint;
        //     bool thisHitCheck = RaycastNavMesh(out thisPoint);

        //     RaycastCheck check = new RaycastCheck { hitPoint = thisPoint, hitCheck = thisHitCheck };
        //     return check;
        // }

        // private AllTargetCheck CursorTargetCheck()
        // {
        //     RaycastHit[] hits = RaycastAllSorted();
        //     CombatTarget testCT = null;
        //     CombatTarget testHeldCT = null;
        //     WeaponPickup testWP = null;

        //     AllTargetCheck allCheck = new AllTargetCheck { hitTarget = testCT, hitHeldTarget = testHeldCT };

        //     foreach (RaycastHit hit in hits)
        //     {
        //         NavTestPath testPath = GetNavMeshPath(transform.position, hit.point);    
        //         if (testPath.navPathDistance > maxPathLength) { continue; }

        //         testCT = hit.transform.GetComponent<CombatTarget>();
        //         testWP = hit.transform.GetComponent<WeaponPickup>();

        //         if ((testCT == null || testCT.IsDead()) && testWP == null)
        //         {
        //             continue;
        //         }
        //         else if (testWP == null && testCT.gameObject == this.gameObject)
        //         {
        //             testCT = null;
        //             continue;
        //         }

        //         if (heldTarget == null) testHeldCT = testCT;
        //         allCheck.hitTarget = testCT;
        //         allCheck.hitHeldTarget = testHeldCT;
        //         allCheck.hitPickup = testWP;

        //         return allCheck;
        //     }
        //     allCheck.hitTarget = null;
        //     allCheck.hitHeldTarget = null;
        //     allCheck.hitPickup = null;

        //     return allCheck;
        // }

        private void RunAllCursorTests()
        {
            RaycastHit hitRay;
            testCursor.hitCheck = Physics.Raycast(GetMouseRay(), out hitRay);

            testCursor.hitPoint = hitRay.point;

            testCursor.hitNavCheck = NavMesh.SamplePosition(testCursor.hitPoint, out testCursor.hitNavPoint, maxNavCheckDistance, NavMesh.AllAreas);

            testNavPath = GetNavMeshPath(transform.position, testCursor.hitPoint);
            bool isBlocked = NavMesh.Raycast(transform.position + new Vector3(0, .5f, 0), testCursor.hitPoint, out testNavPath.navPathEnd, NavMesh.AllAreas);

            //if (testNavPath.navPathDistance > maxPathLength) { return; } // Path too long, escape Update()

            testCursor.hitRayAll = RaycastAllSorted();

            RaycastAllCheck();
        }

        private void RaycastAllCheck()
        {
            CombatTarget testCT = null;
            CombatTarget testHeldCT = null;
            WeaponPickup testWP = null;

            testCursorAll.hitTarget = null;
            testCursorAll.hitHeldTarget = null;
            testCursorAll.hitPickup = null;

            foreach (RaycastHit hit in testCursor.hitRayAll)
            {
                testCT = hit.transform.GetComponent<CombatTarget>();
                testWP = hit.transform.GetComponent<WeaponPickup>();

                if ((testCT == null || testCT.IsDead()) && testWP == null)
                {
                    continue;
                }
                else if (testWP == null && testCT.gameObject == this.gameObject)
                {
                    testCT = null;
                    continue;
                }

                if (heldTarget == null) { testHeldCT = testCT; }
                testCursorAll.hitTarget = testCT;
                testCursorAll.hitHeldTarget = testHeldCT;
                testCursorAll.hitPickup = testWP;
            }
        }

        private RaycastHit[] RaycastAllSorted()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            float[] distances = new float[hits.Length];

            for (int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }
            Array.Sort(distances, hits);

            return hits;

        }

        private NavTestPath GetNavMeshPath(Vector3 originPoint, Vector3 targetPoint)
        {
            NavMeshPath testPath = new NavMeshPath();
            bool pathCheck = false;
            float pathDistance = 0;

            pathCheck = NavMesh.CalculatePath(originPoint, targetPoint, NavMesh.AllAreas, testPath);
            pathDistance = GetPathLength(testPath);

            NavTestPath navMeshPath = new NavTestPath { navPath = testPath, navPathCheck = pathCheck, navPathDistance = pathDistance };
            return navMeshPath;
        }

        private float GetPathLength(NavMeshPath pathToCheck)
        {
            float totalDistance = 0;
            int totalCorners = pathToCheck.corners.Length;

            if (totalCorners < 2) { return totalDistance; }
            for (int i = 0; i < totalCorners - 1; i++)
            {
                totalDistance += Vector3.Distance(pathToCheck.corners[i], pathToCheck.corners[i + 1]);
            }

            return totalDistance;
        }

        // private bool RaycastNavMesh(out Vector3 targetPoint)
        // {
        //     targetPoint = new Vector3();

        //     NavMeshHit pathEnd;
        //     bool isBlocked;

        //     RaycastHit thisHit;
        //     bool thisHitCheck = Physics.Raycast(GetMouseRay(), out thisHit);
        //     lookPoint = thisHit.point;
        //     if (!thisHitCheck) 
        //     { 
        //         return false; 
        //     }

        //     NavMeshHit thisNavHit;
        //     bool navHitCheck = NavMesh.SamplePosition(thisHit.point, out thisNavHit, maxNavCheckDistance, NavMesh.AllAreas);
        //     if (!navHitCheck) 
        //     { 
        //         thisHitCheck = false; 
        //         targetPoint = thisHit.point;
        //     } 
        //     else { targetPoint = thisNavHit.position; }

        //     NavTestPath testPath = GetNavMeshPath(transform.position, targetPoint);
        //     if (!testPath.navPathCheck) 
        //     { 
        //         isBlocked = NavMesh.Raycast(transform.position + new Vector3(0, 2, 0), targetPoint, out pathEnd, NavMesh.AllAreas);
        //         targetPoint = pathEnd.position;
        //         // return false; 
        //     }
        //     if (testPath.navPathDistance > maxPathLength) { return false; }

        //     return thisHitCheck;
        // }


        // ------------------------------------------------------------------


        // Interface Checks \\
        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach (CursorMapping mapping in cursorMappings)
            {
                if (mapping.type == type) { return mapping; }
            }

            return cursorMappings[0];
        }

        private void AttackCheck()
        {
            if (target != null && !target.IsDead()) 
            {
                if (IsAttackReady())
                {
                    PrepareAttack(target);
                }
            } else if (_justStop && _mouseDown)
            {
                playerFight.StartAttack(testCursor.hitPoint);
            }
        }

        private void PrepareAttack(CombatTarget newTarget)
        {
            target = newTarget;
            playerMover.GetLookDirection(target.transform.position, _rotationMultiplier);
            playerFight.SetAttackPoint(target.transform.position);
            playerFight.StartAttack(target);
        }

        private bool IsAttackReady()
        {
            return (_isAutoAttack || playerFight.IsClickTarget() || _mouseDown);
        }

        private void TargetCheck()
        {
            //AllTargetCheck allRayCheck = CursorTargetCheck();
            target = testCursorAll.hitTarget;
            heldTarget = testCursorAll.hitHeldTarget;
            weaponPickup = testCursorAll.hitPickup;

            //return allRayCheck;
        }

        private void MoveCheck()
        {
            //RaycastCheck rayCheck = CursorCheck();
            //clickPoint = testCursor.hitPoint;

            if (_mouseDown)
            {
                if (!_justStop)
                {
                    if (testCursor.hitCheck)
                    {
                        if (heldTarget == null) playerMover.MoveAction(testCursor.hitPoint, _baseSpeed);
                    }
                }
                else
                {
                    if (target == null) playerMover.GetLookDirection(testCursor.hitPoint);
                    if (navMeshAgent.hasPath) navMeshAgent.ResetPath();
                }
            }
        }

        private void AxisCheck()
        {
            if (moveInput != Vector3.zero && !heldTarget)
            {
                Vector3 moveDestination = transform.position + moveInput;
                playerMover.MoveAction(moveDestination, _baseSpeed);  
            }
        }

        private Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        }

        private Vector3 GetMouseWorldPoint()
        {
            return Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        }

        private void StopMoveCheck()
        {
            if (_stopMove) _justStop = true;
            playerFight.AttackAnyRange(_justStop);
            if (_justStop && !_mouseDown) playerMover.GetLookDirection(testCursor.hitPoint, _rotationMultiplier);
        }


        // ------------------------------------------------------------------


        // Cross Class calls \\
        public void StopAllControl()
        {
            target = null;
            playerFight.Cancel();
            playerMover.Cancel();
        }


        // ------------------------------------------------------------------


        // Player Controls \\
        private void KeyBinds()
        {
            /* Rebind controls
            - Movement
            - Attack
            - Abilities
            - Menu
            */
        }

        public void OnAxisMove(InputAction.CallbackContext context)
        {
            // Needs further logic, or to be exclusive control type
            Vector2 inputVector = context.ReadValue<Vector2>();
            moveInput = new Vector3(inputVector.x, 0, inputVector.y);
        }

        public void OnMouseClick(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                //Debug.Log("Started");
                TargetCheck();
                if (target != null) { playerFight.SetClickTarget(true); }
                else 
                { 
                    heldTarget = null;
                    playerFight.Cancel();
                }
                if (weaponPickup != null) 
                { 
                    Vector3 pickupCenter = weaponPickup.transform.GetComponentInChildren<Renderer>().bounds.center; 
                    playerMover.MoveAction(pickupCenter, _baseSpeed); 
                }

                StopMoveCheck();
                _mouseDown = true;
            } 
            else if (context.performed)
            {
                //Debug.Log("Performed");     
            } 
            else if (context.canceled)
            {
                //Debug.Log("Canceled");
                _mouseDown = false;
            }    
        }

        public void OnStopMove(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _stopMove = true;
            }
            else if (context.canceled)
            {
                _stopMove = false;
                _justStop = false;
            }
        }
    }
}

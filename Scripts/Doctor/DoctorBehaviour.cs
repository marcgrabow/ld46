using System.Collections;
using System.Collections.Generic;
using Assets;
using Pathfinding;
using UnityEngine;
using Zenject;

public class DoctorBehaviour : MonoBehaviour
{
    public static DoctorBehaviour Instance;
    public SpriteRenderer ToolSpriteRenderer;
    public AILerp _ai;
    public List<Vector3> Targets = new List<Vector3>();
    public GameObject ToolPrefab;
    
    private bool _busy;
    private Collider2D _lastContactedTile;

    private bool _moving;
    
//    public Animator Animator;


    [Inject] private GameEvents _events;

    public bool Busy
    {
        get => _busy;
        set
        {
            _busy = value;
            _ai.canMove = !_busy;
//            Animator.SetBool("IsWorking", _busy);

            if (!_busy) MaybeGoToNextTarget();
        }
    }

    private void Awake()
    {
        _ai = GetComponent<AILerp>();
        Instance = this;
        _events.OnTileClicked.AddListener(OnTileClicked);
        _rb = GetComponent<Rigidbody2D>();

    }

    private void Start()
    {
        _ai.OnTargetReachedEvent.AddListener(OnTargetReached);
        _ai.OnMoveStart.AddListener(OnMoveStart);
    }

//    [EventListener(EventEnum.TileClicked)]
    private void OnTileClicked(Vector3Int arg0)
    {
        Debug.Log("doctor listening to tile clicked");
        Targets.Add(arg0+new Vector3(0.5f,0.5f));

        MaybeGoToNextTarget();
    }

    private void MaybeGoToNextTarget()
    {
        if (_moving) return;
        //if (Animator.GetBool("IsWalking")) return;
        if (Targets.Count == 0) return;
        _currentTarget = Targets[0];
        Targets.RemoveAt(0);

        _moving = true;
        _ai.canMove = true;
        _ai.canSearch = true;
        _ai.destination = _currentTarget;
        _ai.SearchPath();
        //Animator.SetBool("IsWalking", true);
    }


    private Vector3 _currentTarget;
    private Rigidbody2D _rb;

    private void OnMoveStart()
    {
        Debug.Log("OnMoveStart");
        _moving = true;
        //Animator.SetBool("IsWalking", true);
    }

    private void OnTargetReached(Vector3 arg0)
    {
        _moving = false;
        _ai.canMove = false;
        _ai.canSearch = false;
        //_ai.destination = null;

        Debug.Log("OnTargetReached");
//        Animator.SetBool("IsWalking", false);



        // now find out if there is anything closeby that I can work on
        var waitDuration = 0f;
        var closeCollider = Physics2D.OverlapCircle(transform.position, FindToDoRadius, FindTodoMask);
        if (closeCollider!=null)
        {
            Debug.Log("detected "+closeCollider.name);
            StartCoroutine("ProcessInteractableObject", closeCollider);
        }
        else
        {
            MaybeGoToNextTarget();
        }

    }

    private IEnumerator ProcessInteractableObject(Collider2D obj)
    {
        Busy = true;
        Debug.Log("ProcessInteractableObject ");

        // processing tool handling
        var tool = obj.GetComponent<ToolBehaviour>();
        if (tool != null)
        {
            if (currentToolConfig != null)
            {
                ProgressManager.Instance.Show("Putting down "+currentToolConfig.Title, Game.ToolDownDuration, transform.position);
                yield return new WaitForSeconds(Game.ToolDownDuration);
                // actually place that tool
                var g = Instantiate(ToolPrefab, tool.transform.position, Quaternion.identity);
                g.GetComponent<ToolBehaviour>().Config = currentToolConfig;
                ToolSpriteRenderer.sprite = null;
                yield return new WaitForSeconds(0.2f);
            }
            // pick up the new tool
            var c = tool.Config;
            ProgressManager.Instance.Show(c.PickupText, c.PickupDuration, transform.position);
            yield return new WaitForSeconds(c.PickupDuration);
            PickupTool(tool);
        }
        
        // processing treating body parts
        var bodyPart = obj.GetComponent<BodyPartBehaviour>();
        if (bodyPart != null)
        {
            var treatment = bodyPart.FindTreatment(currentToolConfig);
            if (treatment != null)
            {
                ProgressManager.Instance.Show(treatment.Title, treatment.Duration, transform.position);
                yield return new WaitForSeconds(treatment.Duration + 0.1f);
                bodyPart.ApplyTreatment(treatment);
                _events.OnTreatmentComplete.Invoke();
                if (currentToolConfig.OneTimeUse)
                {
                    ToolSpriteRenderer.sprite = null;
                    currentToolConfig = null;
                }
            }
            else
            {
                if (currentToolConfig != null)
                {
                    var msg = new Msg("Can't use " + currentToolConfig.Title + " on " + bodyPart.name + " right now.",
                        transform.position + new Vector3(0, 1));
                    _events.OnShowMsg.Invoke(msg);
                }
            }
        }

        Busy = false;
    }

    public LayerMask FindTodoMask;
    public float FindToDoRadius = 1;
    
    private void PickupTool(ToolBehaviour tool)
    {
        _events.OnToolPickedUp.Invoke();
            
        currentToolConfig = tool.Config;
        ToolSpriteRenderer.sprite = currentToolConfig.Sprite;
        Destroy(tool.gameObject);
    }

    public ToolConfig currentToolConfig;
    
}
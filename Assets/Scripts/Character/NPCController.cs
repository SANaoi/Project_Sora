using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using aoi;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class NPCController : MonoBehaviour
{
    private bool isPutHand;
    private Animator animator;
    private float Radius = 3.0f;
    private Vector3 defaultHeadPosition;
    private PostureStates postureStates;
    private MultiAimConstraint multiAim;
    public Transform LookAtPoint;
    private WeightedTransformArray weightedTransforms;
    public CharacterTaskList_SO characterTaskList;
    public float HeadRotateAngle;
    private PlayerManager playerManager;

    public enum PostureStates
    {
        Stand,
        Talk,
    }

    public enum TaskStates
    {
        normal,
        Accepting,
        Accepted,

    }
    # region 基础方法
    void Awake()
    {
        animator = GetComponent<Animator>();
        InitTransform();
        LookAtPlayer();
    }

    void Start()
    {
        playerManager = FindAnyObjectByType<PlayerManager>();
        // InvokeRepeating("FoundPlyer", 0.25f, 0.25f);
    }

    void Update()
    {
        SwitchStates();
        SwitchAnimation();
        FoundPlyer();
    }
    # endregion

    # region 初始化
    private void InitTransform()
    {
        defaultHeadPosition = transform.forward;
        
        weightedTransforms = new()
        {
            new WeightedTransform(LookAtPoint, 1f)
        };

        foreach (Transform ChildTransform in GetComponentsInChildren<Transform>())
        {
            if (ChildTransform.name == "HeadLookat Rig")
            {
                multiAim = ChildTransform.Find("Head Constraint").GetComponent<MultiAimConstraint>();
            }
        }
    }
    # endregion

    # region 状态切换
    private void SwitchStates()
    {
        SwitchPostureStates();
    }

    private void SwitchPostureStates()
    {
        switch (postureStates)
        {
            case PostureStates.Stand:
            isPutHand = false;
            break;
            case PostureStates.Talk:
            isPutHand = true;
            break;
        }
    }

    private void SwitchAnimation()
    {
        animator.SetBool("putHand", isPutHand);
    }

    # endregion

    # region 主要方法
    public void ShowDialog() // 查找是否有接取任务
    {
        DialogUI DialogBox = UIManager.Instance.OpenPanel(UIConst.DialogBox) as DialogUI;
        foreach (TaskList taskList in  characterTaskList.textAssets)
        {
            
            DialogBox.reloadData(taskList);
            if (!taskList.IsAccepted)
            {
                DialogBox.ShowDialogRows();
                return;
            }
            else if (taskList.IsAccepted && !taskList.IsCompleted)
            {
                TaskDetails taskDetail = GameManager.Instance.GetTaskDetailByID(taskList.taskID);
                if (GameManager.Instance.IsCompleteTask(taskDetail))
                {
                    taskList.IsCompleted = true;
                    DialogBox.ShowDialogRows(true,true);
                    return;
                }
                DialogBox.ShowDialogRows(true);
                return;
            }
        }
        
    }

    private void FoundPlyer()
    {
        var colliders = Physics.OverlapSphere(transform.position, Radius);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                
                LookAtPoint.position = playerManager.LookPoint.transform.position;
                LookAtPoint.rotation = playerManager.LookPoint.transform.rotation;
                postureStates = PostureStates.Talk;

                Quaternion rotationToTarget = Quaternion.FromToRotation(transform.forward, (LookAtPoint.position - transform.position).normalized);
                Vector3 angles = rotationToTarget.eulerAngles;
                float angle = angles.y;
                if (angle > 180)
                {
                    angle -= 360;
                }
                if (Mathf.Abs(angle) >= HeadRotateAngle)
                {
                    multiAim.weight = Mathf.Clamp01(multiAim.weight - 5f * Time.deltaTime);
                }
                else
                {
                    multiAim.weight = Mathf.Clamp01(multiAim.weight + 5f * Time.deltaTime);
                }
                return;
            }
        }
        
        multiAim.weight = Mathf.Clamp01(multiAim.weight - 5f * Time.deltaTime);
        postureStates = PostureStates.Stand;
    }

    private void LookAtPlayer()
    {
        multiAim.data.sourceObjects = weightedTransforms;
    }
    # endregion
}

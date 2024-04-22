using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using aoi;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[RequireComponent(typeof(ExpressionControl))]
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
    // Emoji
    private ExpressionUI expressionUI;
    private Dictionary<string, List<string>> emojiActions;
    private List<string> currentEmoji;
    private EmojiStats emojiStats;

    bool isNormal;
    bool isPending;
    bool islike;

    public enum PostureStates
    {
        Stand,
        Talk,
    }

    public enum EmojiStats
    {
        normal, Pending, InProgress, like
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
        InitTaskLocalData();
        expressionUI =  GetComponent<ExpressionControl>().ExpressionPrefab.GetComponent<ExpressionUI>();
        emojiActions = new Dictionary<string, List<string>>()
        {
            {"无", new List<string>{}},
            {"任务中", new List<string>{UIImage.感叹号}},
            {"接取中", new List<string>{UIImage.问号}},
            {"爱心", new List<string>{UIImage.爱心}},
        };
        emojiStats = EmojiStats.normal;
        isNormal = true;
    }

    void Update()
    {
        SwitchStates();
        SwitchAnimation();
        FoundPlyer();
    }

    void SwitchEmojiStats()
    {
        if (isNormal && !isPending) 
        {
            emojiStats = EmojiStats.normal;
        }
        else if (isPending)
        {
            emojiStats = EmojiStats.Pending;
        }
        if (islike && !isPending)
        {
            emojiStats = EmojiStats.like;
        }
        switch (emojiStats)
        {
            case EmojiStats.normal:
                SetCurrentEmoji(emojiActions["无"]);
                break;
            case EmojiStats.Pending:
                SetCurrentEmoji(emojiActions["任务中"]);
                break;
            case EmojiStats.like:
                SetCurrentEmoji(emojiActions["爱心"]);
                break;
        }
    }

    void SetCurrentEmoji(List<string> stateActions)
    {
        if (currentEmoji == stateActions)
        {
            return;
        }
        currentEmoji = stateActions;
        expressionUI.Refresh(stateActions);
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
        SwitchEmojiStats();
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
                break;
            }
            else if (taskList.IsAccepted && !taskList.IsCompleted)
            {
                isPending = true;
                TaskDetails taskDetail = GameManager.Instance.GetTaskDetailByID(taskList.taskID);
                if (GameManager.Instance.IsCompleteTask(taskDetail))
                {
                    taskList.IsCompleted = true;
                    DialogBox.ShowDialogRows(true,true);

                    GameManager.Instance.CompleteTasksID.Add(taskDetail.taskID);
                    isPending = false;
                    break;
                }
                DialogBox.ShowDialogRows(true);
                break;
            }
        }
        RefreshEmoji();
    }

    private void FoundPlyer()
    {
        var colliders = Physics.OverlapSphere(transform.position, Radius);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                islike = true;
                
                playerManager = FindAnyObjectByType<PlayerManager>();
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
        islike = false;
        multiAim.weight = Mathf.Clamp01(multiAim.weight - 5f * Time.deltaTime);
        postureStates = PostureStates.Stand;
    }

    private void LookAtPlayer()
    {
        multiAim.data.sourceObjects = weightedTransforms;
    }

    public void RefreshEmoji()
    {
        isPending = false;
        foreach (TaskList taskList in  characterTaskList.textAssets)
        {
            if (taskList.taskID == -1)
            {
                return;
            }
            if (!taskList.IsCompleted && taskList.taskID != -1)
            {
                isPending = true;
            }
        }
    }
    
    private void InitTaskLocalData()
    {
        UserData userData = GameManager.Instance.GetUserData();
        foreach (TaskList taskList in  characterTaskList.textAssets)
        {
            if (taskList.taskID == -1)
            {
                break;
            }
            if (userData.completedTasks.Contains(taskList.taskID))
            {
                taskList.IsAccepted = true;
                taskList.IsCompleted = true;
                continue;
            }
            else
            {
                taskList.IsCompleted = false;
            }

            if (userData.currentTasks.Contains(taskList.taskID))
            {
                taskList.IsAccepted = true;
            }
            else
            {
                taskList.IsAccepted = false;
            }
        }
        RefreshEmoji();
        
    }
    # endregion
}

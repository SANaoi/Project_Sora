using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TeamController : MonoBehaviour
{

    public GameObject character_1;
    public GameObject character_2;
    public CharacterStats currentStats;
    private GameObject currentCharacter;
    private Vector2 currentMoveContext;
    public PlayerManager[] charactersList;
    public GetSceneItems getSceneItems;
    private void Awake()
    {
        charactersList = GetComponentsInChildren<PlayerManager>();
        getSceneItems = GetComponent<GetSceneItems>();
    }

    private void OnEnable()
    {
        GameManager.Instance.inputActions.Player.SelectItem.performed += GetSelectItemInput;
        GameManager.Instance.inputActions.Player.PickUp.canceled += GetPickUpInput;
    }

    private void OnDisable()
    {
        GameManager.Instance.inputActions.Player.PickUp.canceled -= GetPickUpInput;
        GameManager.Instance.inputActions.Player.SelectItem.performed -= GetSelectItemInput;
    }

    private void Start()
    {
        InitPlayerMainUI();
        currentCharacter = character_1;
        character_2.SetActive(false);
        character_1.SetActive(true);
        GameManager.Instance.InitPlayerManager();
        UpdatePlayerLocalInfo();
        currentCharacter.GetComponent<PlayerManager>().UpdatePackageLocalData();
        InvokeRepeating("changeActivateCharacter", 0.1f, 1f);

    }
    private void Update()
    {
        if  (character_1.activeSelf && !character_2.activeSelf)
        {
            gameObject.transform.position = character_1.transform.position;
        }
        else if (!character_1.activeSelf && character_2.activeSelf)
        {
            gameObject.transform.position = character_2.transform.position; 
        }
    }


    private void changeActivateCharacter()
    {
        if (Input.GetKey(KeyCode.V))
        {   
            if (character_1.activeSelf && !character_2.activeSelf)
            {
                
                currentMoveContext = character_1.GetComponent<PlayerManager>().playerMoveContext;
                character_2.SetActive(true);
                character_1.SetActive(false);
                currentCharacter = character_2;
                SwitchCurrentParameter(currentCharacter);

            }
            else if (!character_1.activeSelf && character_2.activeSelf)
            {
                currentMoveContext = character_2.GetComponent<PlayerManager>().playerMoveContext;
                character_1.SetActive(true);
                character_2.SetActive(false);
                currentCharacter = character_1;
                SwitchCurrentParameter(currentCharacter);
            }
        } 
    }
    // 切换人物时保存输入数据
    private void SwitchCurrentParameter(GameObject currentCharacter)
    {
        currentStats = currentCharacter.GetComponent<CharacterStats>();
        Vector3 offsetVector3 = gameObject.transform.forward.normalized;
        currentCharacter.transform.position = gameObject.transform.position;
        UIManager.Instance.OpenPanel(UIConst.PlayerMainUI).GetComponent<PlayerMainUI>().UpdatePlayerHealthInfo(currentStats.CurrentHealth, currentStats.MaxHealth);
        currentCharacter.GetComponent<PlayerManager>().playerMoveContext = currentMoveContext;
    }

    void InitPlayerMainUI()
    {
        character_1.GetComponent<CharacterStats>().UpdateHealthBarOnAttack += UIManager.Instance.OpenPanel(UIConst.PlayerMainUI).GetComponent<PlayerMainUI>().UpdatePlayerHealthInfo;
        character_2.GetComponent<CharacterStats>().UpdateHealthBarOnAttack += UIManager.Instance.OpenPanel(UIConst.PlayerMainUI).GetComponent<PlayerMainUI>().UpdatePlayerHealthInfo;
    }

    public void UpdatePlayerLocalInfo()
    {
        foreach (PlayerManager p in charactersList)
        {
            string name = p.gameObject.name;
            int index = GameManager.Instance.GerPlayerIndex(name);
            ShootController shootController = p.GetComponent<ShootController>();
            CharacterStats characterStats = p.GetComponent<CharacterStats>();
            if (characterStats != null)
            {
                characterStats.CurrentHealth = GameManager.Instance.GetUserData().health[index];
            }
            if (shootController != null)
            {
                shootController.MagazineAmmo = GameManager.Instance.GetUserData().magazineAmmo[index];
            }
        }
    }


    private void GetPickUpInput(InputAction.CallbackContext context)
    {
        if (getSceneItems.SelectingID != "" && getSceneItems.gameObjectList.Count != 0)
        {
            foreach (GameObject Item in getSceneItems.gameObjectList)
            {
                ItemCell ItemInfo = Item.GetComponent<ItemCell>();
                if (ItemInfo.uid == getSceneItems.SelectingID && ItemInfo.type == 1) // 1为可拾取的场景物体
                {
                    int ItemNum = UIManager.Instance.OpenPanel(UIConst.ItemsInfo).GetComponent<ItemsInfo>().GetItemNumByUID(getSceneItems.SelectingID);
                    if (ItemNum == UIManager.Instance.OpenPanel(UIConst.ItemsInfo).GetComponent<ItemsInfo>().scrollContent.childCount - 1)
                    {

                        UIManager.Instance.OpenPanel(UIConst.ItemsInfo).GetComponent<ItemsInfo>().UpSelectID();
                        PackageLocalData.Instance.AddPackageLocalItem(ItemInfo);
                        currentCharacter.GetComponent<PlayerManager>().UpdatePackageLocalData();
                        ItemInfo.Destroy();
                        return;
                    }
                    else
                    {
                        UIManager.Instance.OpenPanel(UIConst.ItemsInfo).GetComponent<ItemsInfo>().DownSelectID();
                        PackageLocalData.Instance.AddPackageLocalItem(ItemInfo);
                        currentCharacter.GetComponent<PlayerManager>().UpdatePackageLocalData();
                        ItemInfo.Destroy();
                        return;
                    }
                }
                else if (ItemInfo.uid == getSceneItems.SelectingID && ItemInfo.type == 101)
                {
                    NPCController nPC = Item.GetComponent<NPCController>();
                    nPC.ShowDialog();
                }

                else if (ItemInfo.uid == getSceneItems.SelectingID && ItemInfo.type == 102)
                {
                    TransitionPoint transitionPoint = ItemInfo.transform.gameObject.GetComponent<TransitionPoint>();
                    SceneController.Instance.TransitionToDestination(transitionPoint);
                }
            }
        }
    }

    private void GetSelectItemInput(InputAction.CallbackContext context)
    {
        if (getSceneItems.SelectingID == "" || getSceneItems.gameObjectList.Count == 0)
        {
            return;
        }
        if (context.ReadValueAsButton())
        {
            // 上移
            UIManager.Instance.OpenPanel(UIConst.ItemsInfo).GetComponent<ItemsInfo>().UpSelectID();
        }
        else
        {
            // 下移
            UIManager.Instance.OpenPanel(UIConst.ItemsInfo).GetComponent<ItemsInfo>().DownSelectID();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamController : MonoBehaviour
{
    public GameObject character_1;
    public GameObject character_2;
    public CharacterStats currentStats;
    private GameObject currentCharacter;
    private Vector2 currentMoveContext;
    private void Awake()
    {
        character_2.SetActive(false);
        character_1.SetActive(true);
        InvokeRepeating("changeActivateCharacter", 0.1f, 1f);
    }

    private void Start()
    {
        InitPlayerMainUI();
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
}

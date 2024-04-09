using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamController : MonoBehaviour
{
    public GameObject character_1;
    public GameObject character_2;
    private void Awake()
    {
        character_2.SetActive(false);
        character_1.SetActive(true);
        InvokeRepeating("changeActivateCharacter", 1f, 1f);
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
                character_2.SetActive(true);
                character_1.SetActive(false);
                Vector3 offsetVector3 = character_1.transform.forward.normalized;
                character_2.transform.position = character_1.transform.position + new Vector3(offsetVector3.x, 0f, offsetVector3.z);
            }
            else if (!character_1.activeSelf && character_2.activeSelf)
            {
                character_1.SetActive(true);
                character_2.SetActive(false);
                Vector3 offsetVector3 = character_2.transform.forward.normalized;
                character_1.transform.position = character_2.transform.position + new Vector3(offsetVector3.x, 0f, offsetVector3.z);
            }
        } 
    }
}

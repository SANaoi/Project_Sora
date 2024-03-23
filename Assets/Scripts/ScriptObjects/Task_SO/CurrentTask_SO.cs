using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace aoi
{

[CreateAssetMenu(fileName = "TaskData_SO", menuName = "Data/Task/CurrentTask")]
public class CurrentTask_SO : ScriptableObject
{
    public List<TaskDetails> TaskDetailsList;
}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Welcome : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI name;
    private void OnEnable()
    {
        name.text = GameManager.Instance.playersName;
    }

}

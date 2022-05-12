using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
//using UnityEngine.UI;

public class RegistrationManager : MonoBehaviour
{
    [SerializeField] private GameSessionManager gameSessionManager;
    [SerializeField] private TMP_InputField  nameInput;
    [SerializeField] private TMP_InputField emailInput;    

    // Start is called before the first frame update

    private bool CheckName(string name)
    {
        if(name == "")
        {
            return false;
        }
        return true;
    }

    private bool CheckEmail(string email)
    {
        if(email == "")
        {
            return false;
        }
        return true;
    }



    public void OnClickOK()
    {
        string name = nameInput.text;
        string email = nameInput.text;
        if (CheckName(name) && CheckEmail(email))
        {
            //gameSessionManager.RegisterPlayerInfo(name, email);
            GameManager.Instance.playersName = name;
            GameManager.Instance.playersEmail = email;
            UIManager.Instance.OnRegistrationExit();
        }
    }

}

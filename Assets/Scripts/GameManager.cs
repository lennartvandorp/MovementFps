using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (!instance)
            {
                Debug.LogError("There doesn't seem to be a GameManager in the scene");
                return null;
            }
            return instance;
        }
    }

    private void Start()
    {
        if (!instance)
        {
            instance = this;
        }
        else { Debug.LogError("There seems to be more than one GameManager in the scene"); }
    }


    [SerializeField] public InputSetup setup;

}

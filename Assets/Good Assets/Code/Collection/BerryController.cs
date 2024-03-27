using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class BerryState
{
    public bool isPicked;
}


public class BerryController : MonoBehaviour
{
    public BerryState[] berryStates;
    public static BerryController Instance;

    private void Awake()
    {
        // Singleton pattern to ensure only one instance of GameManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Don't destroy GameManager when loading new scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate GameManager instances
        }
    }

    void Start()
    {
        ClearBerryStates();
        LoadBerryStates();
    }

    public void PickBerry(int index)
    {
        if (index >= 0 && index < berryStates.Length)
        {
            berryStates[index].isPicked = true;
            SaveBerryStates();
        }
    }

    void SaveBerryStates()
    {
        for (int i = 0; i < berryStates.Length; i++)
        {
            PlayerPrefs.SetInt("Berry" + i + "Picked", berryStates[i].isPicked ? 1 : 0);
        }
        PlayerPrefs.Save();
    }

    void LoadBerryStates()
    {
        for (int i = 0; i < berryStates.Length; i++)
        {
            berryStates[i].isPicked = PlayerPrefs.GetInt("Berry" + i + "Picked", 0) == 1;
        }
    }

    void ClearBerryStates()
    {
        for (int i = 0; i < berryStates.Length; i++)
        {
            berryStates[i].isPicked = false;
            PlayerPrefs.DeleteKey("Berry" + i + "Picked");
        }
    }
}
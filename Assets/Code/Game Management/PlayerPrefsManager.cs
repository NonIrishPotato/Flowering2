using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
    public static PlayerPrefsManager instance; // Singleton instance
    public int[] defaultIntArray; // Default array to be used if no saved data is found
    public string playerPrefsKey = "IntArrayData"; // Key to save/load the array data

    void Awake()
    {
        // Ensure only one instance exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist between scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Load the array from PlayerPrefs or use the default array if not found
        int[] loadedArray = LoadIntArray();

        // Example: Print the loaded array to console
        Debug.Log("Loaded Array: " + string.Join(", ", loadedArray));

        // Example: Modify the loaded array and save it back to PlayerPrefs
        // Note: Modify the array as needed
        loadedArray[0] = 100;
        SaveIntArray(loadedArray);
    }

    // Method to load the array from PlayerPrefs
    int[] LoadIntArray()
    {
        int[] intArray = new int[defaultIntArray.Length];

        for (int i = 0; i < defaultIntArray.Length; i++)
        {
            // Load each individual int from PlayerPrefs
            intArray[i] = PlayerPrefs.GetInt(playerPrefsKey + "_" + i, defaultIntArray[i]);
        }

        return intArray;
    }

    // Method to save the array to PlayerPrefs
    void SaveIntArray(int[] arrayToSave)
    {
        for (int i = 0; i < arrayToSave.Length; i++)
        {
            // Save each individual int to PlayerPrefs
            PlayerPrefs.SetInt(playerPrefsKey + "_" + i, arrayToSave[i]);
        }

        PlayerPrefs.Save();
    }
}

using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; set; }
    public SaveState state;
    ///* Set instance and load save file *///
    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        Load("Hypercharge");

    }

   
    /// Save the whole state of this SaveState script to the player pref
    public void Save(string file)
    {
        PlayerPrefs.SetString(file, Helper.Serialize<SaveState>(state));
    }

    /// Load the previous save state from the player pref
    public void Load(string file)
    {
        // Do we already have a save?
        if (PlayerPrefs.HasKey(file))
        {
            state = Helper.Deserialize<SaveState>(PlayerPrefs.GetString(file));
        }
        else
        {
            state = new SaveState();
            Save(file);
            // No save file found, creating a new one!
        }
    }


    ///* Check if thing is owned *///
    /*public bool IsThingOwned(int index)
    {
        // Check if bit is set, if so the thing is owned
        return (state.thingsOwned & (1 << index)) != 0;
    }*/

    ///* Unlock thing *///
    /*public void UnlockThing(int index)
    {
        // Toggle on the bit at the index
        state.thingsOwned |= 1 << index;
    }*/

    ///* Resets and loads SaveState file live *///
    public void ResetSave(string file)
    {
        PlayerPrefs.DeleteKey(file);
        Load("Hypercharge");
        Save("Hypercharge");
    }
}

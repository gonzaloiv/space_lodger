﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class DataManager {

    #region Fields

    public static Leaderboard Leaderboard { get { return leaderboard; } }

    private static Leaderboard leaderboard;
    private static string dataPath;

    #endregion

    #region Events

    public delegate void DataLoadedEventHandler (DataLoadedEventArgs dataLoadedEventArgs);
    public static event DataLoadedEventHandler DataLoadedEvent = delegate {};

    #endregion

    #region Public Behaviour

    public static void Init () {
        dataPath = Application.persistentDataPath;
        Debug.Log("Data: " + Application.persistentDataPath);
        LoadData();
    }

    public static void SetNewScore (int newScore) { 
        for (int i = 0; i < leaderboard.Scores.Length; i++) {
            if (newScore > leaderboard.Scores[i]) {
                for (int j = leaderboard.Scores.Length; j < i; i--)
                    leaderboard.Scores[j] = leaderboard.Scores[j - 1];
                leaderboard.Scores[i] = newScore;
                leaderboard.Dates[i] = DateTime.Now;
                break;
            }
        }
        SaveData();
    }

    public static void SetHasBeenTutorialPlayed () {
        leaderboard.IsFirstPlay = false;
        SaveData();
    }

    #endregion

    #region Private Behaviour

    private static void LoadData () {
        leaderboard = new Leaderboard();
        try {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream saveFile = File.Open(dataPath + "/leaderboard.binary", FileMode.Open);
            leaderboard = (Leaderboard) formatter.Deserialize(saveFile);
            saveFile.Close();
        } catch (Exception exception) {
            Debug.Log(exception.Message);
        }
        DataLoadedEvent.Invoke(new DataLoadedEventArgs(leaderboard));
    }

    private static void SaveData () {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream saveFile = File.Create(dataPath + "/leaderboard.binary");
        formatter.Serialize(saveFile, leaderboard);
        saveFile.Close();
    }

    #endregion

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class StartMenuManager : MonoBehaviour
{
    public TextMeshProUGUI bestScoreNameText;
    public InputField NameInput;
    public int bestScore = 0;
    public string bestScoreName;
    public string lastPlayerName;
    public static StartMenuManager Instance;
    // Start is called before the first frame update
    private void Awake()
    {
        // start of new code
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // end of new code

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadBestScore();
    }
    [System.Serializable]
    class SaveData
    {
        public int bestScore;
        public string bestScoreName;
        public string lastPlayerName;
    }
    public void SaveBestScore()
    {
        SaveData data = new SaveData();
        data.bestScore = bestScore;
        data.bestScoreName = bestScoreName;
        data.lastPlayerName = NameInput.text;

        string json = JsonUtility.ToJson(data);
    
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadBestScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            bestScore = data.bestScore;
            bestScoreName = data.bestScoreName;
            // lastPlayerName = data.lastPlayerName;
            bestScoreNameText.text = "Best Score: " + bestScoreName + " : " + bestScore;
            NameInput.text = data.lastPlayerName;
        }else{
            bestScore = 0;
            bestScoreName = "Nobody";
            bestScoreNameText.text = "Best Score: Nobody";
        }
    }
    public void StartNewGame()
    {
        if(NameInput.text == "")
        {
            NameInput.text = "anonymous";
        }
        SceneManager.LoadScene(1);
        SaveBestScore();
    }
    public void Exit()
    {
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit(); // original code to quit Unity player
        #endif
        SaveBestScore();
    }
}

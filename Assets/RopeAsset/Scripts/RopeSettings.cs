using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "RopeSettings", menuName = "Rope/RopeSettings")]
public class RopeSettings : SingletonScriptableObject<RopeSettings>
{
    public string SceneName => SceneManager.GetActiveScene().name;
    //public string CurrentSceneDataPath => DataFolderPath + "/" + SceneManager.GetActiveScene().name;
    //public string DataFolderPath
    //{
    //    get
    //    {
    //        string path = AssetDatabase.GetAssetPath(Instance);
    //        path = path.Replace(Instance.name + ".asset", DataFolderName);
    //        return path;
    //    }
    //}
    public string DataFolderName => "RopeDatas";

    public string GetPath(string id)
    {
        return DataFolderName + "/" + SceneName + "/" + id;
    }

    [Header("Collision")]
    public float CollisionCheckThreshold = 0.25f;
    public float CollisionDistance = 0.1f;
    public float CollisionEndDuration = 1f;
}

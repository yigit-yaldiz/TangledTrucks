using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
public class RopeSave : EditorWindow
{
    public static string CurrentSceneDataPath => DataFolderPath + "/" + SceneManager.GetActiveScene().name;
    public static string DataFolderPath
    {
        get
        {
            string path = AssetDatabase.GetAssetPath(RopeSettings.Instance);
            path = path.Replace(RopeSettings.Instance.name + ".asset", RopeSettings.Instance.DataFolderName);
            return path;
        }
    }

    [MenuItem("Rope/Load Ropes")]
    public static void Load()
    {
        RopeHandler[] ropes = FindObjectsOfType<RopeHandler>();

        for (int i = 0; i < ropes.Length; i++)
        {
            RopeHandler rope = ropes[i];
            RopeData data = Resources.Load(RopeSettings.Instance.GetPath(rope.Id)) as RopeData;

            if (data == null)
            {
                Debug.LogWarning(RopeSettings.Instance.GetPath(rope.Id) + " not found.");
                return;
            }
            for (int j = 0; j < data.AttachmentPositions.Count; j++)
            {
                rope.Attachments[j].target.position = data.AttachmentPositions[j];
            }

            for (int j = 0; j < data.Particles.Length; j++)
            {
                int solverIndex = rope.Actor.solverIndices[j];
                rope.Actor.solver.positions[solverIndex] = data.Positions[j];
                rope.Actor.solver.velocities[solverIndex] = Vector4.zero;
            }
        }
    }

    [MenuItem("Rope/Save Ropes")]
    public static void Save()
    {
        if (!Application.isPlaying)
        {
            Debug.LogWarning("Can be save in play mode!");
            return;
        }

        if (!AssetDatabase.IsValidFolder(DataFolderPath))
        {
            string newPath = DataFolderPath.Replace("/" + RopeSettings.Instance.DataFolderName, "");
            AssetDatabase.CreateFolder(newPath, RopeSettings.Instance.DataFolderName);
        }

        AssetDatabase.DeleteAsset(CurrentSceneDataPath);

        string newDataPath = CurrentSceneDataPath.Replace("/" + RopeSettings.Instance.SceneName, "");
        AssetDatabase.CreateFolder(newDataPath, RopeSettings.Instance.SceneName);

        RopeHandler[] ropes = FindObjectsOfType<RopeHandler>();

        for (int i = 0; i < ropes.Length; i++)
        {
            RopeData data = CreateInstance<RopeData>();
            data.Init(ropes[i]);
            string ropeId = ropes[i].Id;
            string filePath = CurrentSceneDataPath + "/" + ropeId + ".asset";
            AssetDatabase.CreateAsset(data, filePath);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Ropes saved at : " + CurrentSceneDataPath);
    }
}
#endif

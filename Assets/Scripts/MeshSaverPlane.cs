//https://unitycoder.com/blog/2013/01/26/save-mesh-created-by-script-in-editor-playmode/

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

// Usage: Attach to gameobject, assign target gameobject (from where the mesh is taken), Run, Press savekey

public class MeshSaverPlane : MonoBehaviour
{

    public KeyCode saveKey = KeyCode.G;
    public string saveName = "SavedPlaneMesh";
    public Transform selectedGameObject;

    void Update()
    {
        if (Input.GetKeyDown(saveKey))
        {
            SaveAsset();
        }
    }

    void SaveAsset()
    {
        var mf = selectedGameObject.GetComponent<MeshFilter>();
        if (mf)
        {
            var savePath = "Assets/" + saveName + ".asset";
            Debug.Log("Saved Mesh to:" + savePath);
            AssetDatabase.CreateAsset(mf.mesh, savePath);
        }
    }
}
#endif

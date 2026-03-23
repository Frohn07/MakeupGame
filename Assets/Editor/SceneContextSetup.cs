using MakeupGame.Installers;
using UnityEditor;
using UnityEngine;
using Zenject;

/// <summary>
/// Editor utility: wires GameInstaller into the SceneContext Installers list.
/// Run once via Tools → Setup SceneContext Installer.
/// </summary>
public class SceneContextSetup
{
    [MenuItem("Tools/Setup SceneContext Installer")]
    public static void Setup()
    {
        var sceneContext  = Object.FindObjectOfType<SceneContext>();
        var gameInstaller = Object.FindObjectOfType<GameInstaller>();

        if (sceneContext == null)
        {
            Debug.LogError("[Setup] SceneContext not found in scene.");
            return;
        }

        if (gameInstaller == null)
        {
            Debug.LogError("[Setup] GameInstaller not found in scene.");
            return;
        }

        var so       = new SerializedObject(sceneContext);
        // Zenject serializes installers as "_monoInstallers" (formerly "_installers")
        var instProp = so.FindProperty("_monoInstallers");

        if (instProp == null)
        {
            Debug.LogError("[Setup] Could not find Installers property on SceneContext.");
            return;
        }

        // Check if already added
        for (int i = 0; i < instProp.arraySize; i++)
        {
            if (instProp.GetArrayElementAtIndex(i).objectReferenceValue == gameInstaller)
            {
                Debug.Log("[Setup] GameInstaller already in Installers list.");
                so.Dispose();
                return;
            }
        }

        instProp.arraySize++;
        instProp.GetArrayElementAtIndex(instProp.arraySize - 1).objectReferenceValue = gameInstaller;

        so.ApplyModifiedProperties();
        EditorUtility.SetDirty(sceneContext);
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(sceneContext.gameObject.scene);

        Debug.Log("[Setup] GameInstaller successfully added to SceneContext.Installers.");
    }
}

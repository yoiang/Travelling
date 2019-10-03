#if UNITY_EDITOR
/* This wizard will replace a selection with an object or prefab.
 * Scene objects will be cloned (destroying their prefab links).
 * Based on code by 'yesfish'
 * Based on code by Dave A 
 */
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ReplaceSelection : ScriptableWizard
{
    static GameObject replacement = null;
    static bool keep = false;
 
    public GameObject ReplacementObject = null;
    public bool KeepOriginals = false;
 
    [MenuItem("GameObject/Replace Selection...")]
    static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard("Replace Selection", typeof(ReplaceSelection), "Replace");
    }
 
    public ReplaceSelection()
    {
        ReplacementObject = replacement;
        KeepOriginals = keep;
    }
 
    void OnWizardUpdate()
    {
        replacement = ReplacementObject;
        keep = KeepOriginals;
    }
 
    void OnWizardCreate()
    {
        if (replacement == null) {
            EditorUtility.DisplayDialog("Error: Replace Selection", "No replacement selected", "Ok");
            return;
        }
 
        Transform[] transforms = Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.OnlyUserModifiable);
        Debug.Log("Replacing " + transforms.Length, this);

        Undo.SetCurrentGroupName("Replace Selection");
        int group = Undo.GetCurrentGroup();

        PrefabAssetType prefabAssetType = PrefabUtility.GetPrefabAssetType(replacement);
        PrefabInstanceStatus prefabInstanceStatus = PrefabUtility.GetPrefabInstanceStatus(replacement);

        List<GameObject> allCreated = new List<GameObject>();
        foreach (Transform t in transforms)
        {
            GameObject created;
 
            if (prefabAssetType == PrefabAssetType.NotAPrefab)
            {
                created = (GameObject)Editor.Instantiate(replacement);
            }
            else
            {
                if (prefabInstanceStatus == PrefabInstanceStatus.NotAPrefab) {
                    created = (GameObject)PrefabUtility.InstantiatePrefab(replacement, t.parent);
                } else {
                    GameObject prefabParent = PrefabUtility.GetCorrespondingObjectFromSource(replacement) as GameObject;
                    created = (GameObject)PrefabUtility.InstantiatePrefab(prefabParent);
                    PrefabUtility.SetPropertyModifications(created, PrefabUtility.GetPropertyModifications(replacement));
                }
            }
            Undo.RegisterCreatedObjectUndo(created, "Replacement instance");
 
            created.name = replacement.name;
            created.transform.parent = t.parent;
            created.transform.localPosition = t.localPosition;
            created.transform.localScale = t.localScale;
            created.transform.localRotation = t.localRotation;

            allCreated.Add(created);
        }
 
        if (!keep)
        {
            foreach (GameObject g in Selection.gameObjects)
            {
                Undo.DestroyObjectImmediate(g);
            }
        }

        Selection.objects = allCreated.ToArray();

        Undo.CollapseUndoOperations( group );
    }
}
#endif
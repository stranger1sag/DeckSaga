using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class MigrationValidator
{
    public static void Validate()
    {
        var issues = new List<string>();
        const string scenePath = "Assets/Scenes/SampleScene.unity";
        EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(scenePath, true) };
        ValidateScene(scenePath, issues);
        ValidateAssets("t:Prefab", new[] { "Assets/Prefabs" }, issues);
        ValidateAssets("t:ScriptableObject", new[] { "Assets/Data" }, issues);

        foreach (var issue in issues)
        {
            Debug.LogError(issue);
        }

        if (issues.Count > 0)
        {
            throw new InvalidOperationException($"Migration validation found {issues.Count} issue(s).");
        }

        Debug.Log("MIGRATION_VALIDATION_OK: scene, prefabs, and data assets have no missing scripts or broken object references.");
    }

    private static void ValidateScene(string path, ICollection<string> issues)
    {
        var scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
        foreach (var root in scene.GetRootGameObjects())
        {
            ValidateGameObject(root, path, issues);
        }
    }

    private static void ValidateAssets(string filter, string[] folders, ICollection<string> issues)
    {
        foreach (var guid in AssetDatabase.FindAssets(filter, folders))
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var asset = AssetDatabase.LoadMainAssetAtPath(path);
            if (asset == null)
            {
                issues.Add($"Could not load asset: {path}");
                continue;
            }

            if (asset is GameObject gameObject)
            {
                ValidateGameObject(gameObject, path, issues);
            }

            ValidateSerializedReferences(asset, path, issues);
        }
    }

    private static void ValidateGameObject(GameObject gameObject, string path, ICollection<string> issues)
    {
        foreach (var component in gameObject.GetComponentsInChildren<Component>(true))
        {
            if (component == null)
            {
                issues.Add($"Missing script under {path}: {GetHierarchyPath(gameObject)}");
                continue;
            }

            ValidateSerializedReferences(component, path, issues);
        }
    }

    private static void ValidateSerializedReferences(UnityEngine.Object target, string path, ICollection<string> issues)
    {
        var property = new SerializedObject(target).GetIterator();
        while (property.Next(true))
        {
            if (property.propertyType == SerializedPropertyType.ObjectReference
                && property.objectReferenceValue == null
                && property.objectReferenceInstanceIDValue != 0)
            {
                issues.Add($"Broken reference in {path}: {target.name}.{property.propertyPath}");
            }
        }
    }

    private static string GetHierarchyPath(GameObject gameObject)
    {
        var path = gameObject.name;
        var current = gameObject.transform.parent;
        while (current != null)
        {
            path = current.name + "/" + path;
            current = current.parent;
        }

        return path;
    }
}

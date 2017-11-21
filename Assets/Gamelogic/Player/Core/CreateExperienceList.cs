using UnityEngine;
using System.Collections;
using UnityEditor;
namespace Assets.Gamelogic.Player.Core
{
    public class CreateExperienceList
    {
        [MenuItem("Assets/Create/Inventory Item List")]
        public static ExperienceList Create(string fileName)
        {
            ExperienceList asset = ScriptableObject.CreateInstance<ExperienceList>();

            AssetDatabase.CreateAsset(asset, "Assets/Gamelogic/Player/Core/DB/" + fileName + ".asset");
            AssetDatabase.SaveAssets();
            return asset;
        }
    }
}
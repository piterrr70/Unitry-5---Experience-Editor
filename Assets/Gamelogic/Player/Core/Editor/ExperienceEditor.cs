using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Gamelogic.Player.Core;

namespace Assets.Gamelogic.Player.Core
{
    public class ExperienceEditor : EditorWindow
    {

        public ExperienceEditor instance;
        public ExperienceList expierienceList;
        private int viewIndex = 1;

        public string Listname;
        private int health_min;
        private int health_max;
        private int energy_min;
        private int energy_max;

        [MenuItem("PROJECT/Experience Editor %#e")]
        static void Init()
        {
            EditorWindow.GetWindow(typeof(ExperienceEditor));
        }

        void OnEnable()
        {
            if (EditorPrefs.HasKey("ObjectPath"))
            {
                string objectPath = EditorPrefs.GetString("ObjectPath");
                expierienceList = AssetDatabase.LoadAssetAtPath(objectPath, typeof(ExperienceList)) as ExperienceList;
            }

        }

        void OnGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Experience Editor", EditorStyles.boldLabel);
            if (expierienceList != null)
            {
                if (GUILayout.Button("Show Exp List"))
                {
                    EditorUtility.FocusProjectWindow();
                    Selection.activeObject = expierienceList;
                }
            }
            if (GUILayout.Button("Open Exp List"))
            {
                OpenItemList();
            }
            //if (GUILayout.Button("New Exp List"))
            //{
            //    EditorUtility.FocusProjectWindow();
            //    Selection.activeObject = expierienceList;
            //}
            GUILayout.EndHorizontal();

            if (expierienceList != null)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Current Experience Base:", EditorStyles.boldLabel);
                GUILayout.Space(10);
                GUILayout.Label(expierienceList.name,EditorStyles.boldLabel);
                GUILayout.EndHorizontal();
            }

            if (expierienceList == null)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(10);

                BeginHorizontalField("New List Name", Screen.width / 3f);
                Listname = EditorGUILayout.TextField(Listname);
                EndHorizontal();

                GUILayout.Space(5);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();

                if (Listname == null || Listname.Length < 1)
                {
                    EditorGUILayout.HelpBox("This list needs a [Name] before it can be created.", MessageType.Warning);
                }
                else
                {
                    if (GUILayout.Button("Create New Exp List", GUILayout.ExpandWidth(false)))
                    {
                        CreateNewItemList(Listname);
                    }

                }
                
                if (GUILayout.Button("Open Existing Exp List", GUILayout.ExpandWidth(false)))
                {
                    OpenItemList();
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.BeginHorizontal();
            if (expierienceList.expirienceList.Count == 0)
            {
                EditorGUILayout.HelpBox("First add first level and add values.", MessageType.Info);
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(20);

            if (expierienceList != null)
            {
                GUILayout.BeginHorizontal();

                GUILayout.Space(10);
                if (expierienceList.expirienceList.Count > 0)
                {
                    if (GUILayout.Button("Prev", GUILayout.ExpandWidth(false)))
                    {
                        if (viewIndex > 1)
                            viewIndex--;
                    }
                    GUILayout.Space(5);
                    if (GUILayout.Button("Next", GUILayout.ExpandWidth(false)))
                    {
                        if (viewIndex < expierienceList.expirienceList.Count)
                        {
                            viewIndex++;
                        }
                    }
                }

                GUILayout.Space(60);

                if (GUILayout.Button("Add Exp", GUILayout.ExpandWidth(false)))
                {
                    AddItem();
                }
                if (expierienceList.expirienceList.Count > 0)
                {
                    if (GUILayout.Button("Delete Exp", GUILayout.ExpandWidth(false)))
                    {
                        DeleteItem(viewIndex - 1);
                    }
                }

                GUILayout.EndHorizontal();
                if (expierienceList.expirienceList == null)
                    Debug.Log("wtf");
                if (expierienceList.expirienceList.Count > 0)
                {
                    GUILayout.BeginHorizontal();
                    viewIndex = Mathf.Clamp(EditorGUILayout.IntField("Current Item", viewIndex, GUILayout.ExpandWidth(false)), 1, expierienceList.expirienceList.Count);
                    //Mathf.Clamp (viewIndex, 1, inventoryItemList.itemList.Count);
                    EditorGUILayout.LabelField("of   " + expierienceList.expirienceList.Count.ToString() + "  items", "", GUILayout.ExpandWidth(false));
                    GUILayout.EndHorizontal();

                    expierienceList.expirienceList[viewIndex - 1].level = EditorGUILayout.IntField("Level", expierienceList.expirienceList[viewIndex - 1].level);
                    expierienceList.expirienceList[viewIndex - 1].health = EditorGUILayout.IntField("Health", expierienceList.expirienceList[viewIndex - 1].health);
                    expierienceList.expirienceList[viewIndex - 1].energy = EditorGUILayout.IntField("Energy", expierienceList.expirienceList[viewIndex - 1].energy);

                    GUILayout.Space(10);

                }
                else
                {
                    GUILayout.Label("This Experience List is Empty.");
                }

                if (expierienceList.expirienceList.Count > 0 && expierienceList.expirienceList.Count <= 107)
                {
                    GUILayout.Label("Random List Generator", EditorStyles.boldLabel);
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(10);

                    BeginHorizontalField("", Screen.width / 3f);
                    GUILayout.Label("Min value");
                    GUILayout.Space(5);
                    GUILayout.Label("Max value");
                    EndHorizontal();

                    GUILayout.Space(5);
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Space(10);

                    BeginHorizontalField("Health Random Values", Screen.width / 3f);
                    health_min = EditorGUILayout.IntField(health_min);
                    GUILayout.Space(5);
                    health_max = EditorGUILayout.IntField(health_max);
                    EndHorizontal();

                    GUILayout.Space(5);
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Space(10);

                    BeginHorizontalField("Energy Random Values", Screen.width / 3f);
                    energy_min = EditorGUILayout.IntField(energy_min);
                    GUILayout.Space(5);
                    energy_max = EditorGUILayout.IntField(energy_max);
                    EndHorizontal();

                    GUILayout.Space(5);
                    GUILayout.EndHorizontal();

                    if (expierienceList.expirienceList.Count > 0 && expierienceList.expirienceList[viewIndex -1].health > 0 && expierienceList.expirienceList[viewIndex - 1].energy > 0 && health_min != 0 && health_max != 0 && energy_max != 0 && energy_min != 0)
                    {
                        GUILayout.BeginHorizontal();
                        if (GUILayout.Button("Generate List", GUILayout.ExpandWidth(false)))
                        {
                            GenerateDefault();
                        }

                        GUILayout.Space(5);
                        GUILayout.EndHorizontal();

                    }
                    else
                    {
                        EditorGUILayout.HelpBox("To generate random list you need to past all need information", MessageType.Warning);
                    }

                    if (expierienceList.expirienceList.Count > 1)
                    {
                        GUILayout.BeginHorizontal();
                        if (GUILayout.Button("Delete Generated List", GUILayout.ExpandWidth(false)))
                        {
                            DeleteRandomList();
                        }
                        GUILayout.EndHorizontal();
                    }
                    GUILayout.Space(5);
                    
                }
            }
            

            if (expierienceList != null)
            {
                GUILayout.BeginHorizontal();

                GUILayout.Label("------------------------------------------------------------------------------");

                GUILayout.Space(10);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();

                BeginHorizontalField("New List Name", Screen.width / 3f);
                Listname = EditorGUILayout.TextField(Listname);
                EndHorizontal();

                GUILayout.Space(5);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal(GUILayout.ExpandHeight(true));

                if (Listname == null || Listname.Length < 1)
                {
                    EditorGUILayout.HelpBox("This list needs a [Name] before it can be created.", MessageType.Warning);
                }
                else
                {
                    if (GUILayout.Button("Create New Exp List", GUILayout.ExpandWidth(false)))
                    {
                        CreateNewItemList(Listname);
                        Listname = "";
                    }

                }
                GUILayout.Space(10);
                GUILayout.EndHorizontal();
            }



            if (GUI.changed)
            {
                EditorUtility.SetDirty(expierienceList);
            }
        }

        void BeginHorizontalField(string title, float titleWidth)
        {
            GUILayout.Space(5);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(5);
            GUILayout.Label(title, GUILayout.Width(titleWidth));
        }

        void EndHorizontal()
        {
            GUILayout.Space(5);
            EditorGUILayout.EndHorizontal();
        }

        void CreateNewItemList(string fileName)
        {
            // There is no overwrite protection here!
            // There is No "Are you sure you want to overwrite your existing object?" if it exists.
            // This should probably get a string from the user to create a new name and pass it ...
            viewIndex = 1;
            expierienceList = CreateExperienceList.Create(fileName);
            if (expierienceList)
            {
                expierienceList.expirienceList = new List<Experience>();
                string relPath = AssetDatabase.GetAssetPath(expierienceList);
                EditorPrefs.SetString("ObjectPath", relPath);
            }
        }

        void OpenItemList()
        {
            string absPath = EditorUtility.OpenFilePanel("Select Experience List", "/Assets/", "asset");
            if (absPath.StartsWith(Application.dataPath))
            {
                string relPath = absPath.Substring(Application.dataPath.Length - "Assets".Length);
                expierienceList = AssetDatabase.LoadAssetAtPath(relPath, typeof(ExperienceList)) as ExperienceList;
                if (expierienceList.expirienceList == null)
                    expierienceList.expirienceList = new List<Experience>();
                if (expierienceList)
                {
                    EditorPrefs.SetString("ObjectPath", relPath);
                }
            }
        }

        void AddItem()
        {
            Experience newItem = new Experience();
            newItem.level = expierienceList.expirienceList.Count+1;
            var exp = expierienceList.expirienceList.LastOrDefault();
            if (exp != null)
            {
                newItem.health = exp.health + Random.Range(health_min, health_max);
                newItem.energy = exp.energy + Random.Range(energy_min, energy_max);
            }
            expierienceList.expirienceList.Add(newItem);
            viewIndex = expierienceList.expirienceList.Count;
        }

        void GenerateDefault()
        {
            int last = 106;
            for(int i = 1; i <= last; i++)
            {
                AddItem();
            }
        }

        void DeleteRandomList()
        {
            if (expierienceList.expirienceList.Count > 1)
            {
                var lastItem = expierienceList.expirienceList.LastOrDefault();
                expierienceList.expirienceList.Remove(lastItem);
                DeleteRandomList();
            }
        }
        void DeleteItem(int index)
        {
            expierienceList.expirienceList.RemoveAt(index);
        }
    }
}
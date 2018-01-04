using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using Assets.Scripts.EnvironmentDestruction;
using Newtonsoft.Json;

namespace Assets.Editor
{

    class LoadChunkData
    {
        [MenuItem("CrystalReign/Load Chunks")]
        static void LoadChunks()
        {
            string done_json = File.ReadAllText("Assets\\Resources\\done.json");
            List<string> done = JsonConvert.DeserializeObject<List<string>>(done_json);
            string path = File.ReadAllText("Assets\\Resources\\path_to_chunks.txt");
            string[] dirs = Directory.GetDirectories(path);
            if (!Directory.Exists("Assets/Resources/Chunks"))
            {
                Directory.CreateDirectory("Assets/Resources/Chunks");
            }
            int i = 0;
            foreach (string dir in dirs)
            {
                string obj_name = Path.GetFileName(dir);
                if (done.Contains(obj_name)) continue;
                Debug.Log("Importing " + obj_name);
                GameObject obj = GameObject.Find(obj_name);
                DestructableObject desobj = obj.GetComponent<DestructableObject>();
                if (desobj == null) desobj = obj.AddComponent<DestructableObject>();
                desobj.mat = obj.GetComponent<Renderer>().sharedMaterial;
                if (Directory.Exists(Path.Combine(dir, "json")) && Directory.Exists(Path.Combine(dir, "fbx")))
                {
                    string[] jsons = Directory.GetFiles(Path.Combine(dir, "json"));
                    foreach (string j in jsons)
                    {
                        File.Copy(j, Path.Combine("Assets/Resources/Chunks", Path.GetFileName(j)), true);
                    }
                    string[] fbxs = Directory.GetFiles(Path.Combine(dir, "fbx"));
                    foreach (string f in fbxs)
                    {
                        if (Path.GetExtension(f) == ".fbx")
                        {
                            GameObject objAsset = AssetDatabase.LoadAssetAtPath(f, typeof(GameObject)) as GameObject;
                            string prefab_path = Path.Combine("Assets/Resources/Chunks", Path.GetFileName(f)).Replace("\\", "/");
                            prefab_path = Path.ChangeExtension(prefab_path, ".prefab");
                            UnityEngine.Object prefab = PrefabUtility.CreateEmptyPrefab(prefab_path);
                            PrefabUtility.ReplacePrefab(objAsset, prefab, ReplacePrefabOptions.ConnectToPrefab);

                        }
                    }

                }
                done.Add(obj_name);
                if (i % 1 == 0)
                {
                    File.WriteAllText("Assets\\Resources\\done.json", JsonConvert.SerializeObject(done));
                }
                i++;
            }
            File.WriteAllText("Assets\\Resources\\done.json", JsonConvert.SerializeObject(done));
        }
    }
}

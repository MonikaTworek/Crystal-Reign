using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using Assets.Scripts.EnvironmentDestruction;

namespace Assets.Editor
{

    class LoadChunkData
    {
        [MenuItem("CrystalReign/Load Chunks")]
        static void LoadChunks()
        {
            string path = "";
            using (StreamReader sr = new StreamReader("D:\\University\\CrystalReign\\CrystalReign\\Assets\\Resources\\path_to_chunks.txt"))
            {
                path = sr.ReadToEnd();
            }
            string[] dirs = Directory.GetDirectories(path);
            if (!Directory.Exists("Assets/Resources/Chunks"))
            {
                Directory.CreateDirectory("Assets/Resources/Chunks");
            }
            foreach (string dir in dirs)
            {
                GameObject obj = GameObject.Find(Path.GetFileName(dir));
                DestructableObject desobj = obj.GetComponent<DestructableObject>();
                if (desobj == null) desobj = obj.AddComponent<DestructableObject>();
                desobj.mat = obj.GetComponent<Renderer>().material;
                desobj.level_name = "Chunks";
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
            }
        }
    }
}

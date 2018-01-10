using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class SoundsMaterial
{
    public List<AudioSource> audio;
    public Material mat;
}


public class DestructionAudioSetter : MonoBehaviour {

    public List<SoundsMaterial> soundsMats;
    public TextAsset objectNamesList;
    private Dictionary<Material, List<AudioSource>> dict = new Dictionary<Material, List<AudioSource>>();


    private void UpdateDict()
    {
        dict.Clear();
        foreach (SoundsMaterial sm in soundsMats)
        {
            dict[sm.mat] = sm.audio;
        }
    }

    public void Set()
    {
        UpdateDict();
        List<string> objNames = JsonConvert.DeserializeObject<List<string>>(objectNamesList.text);
        List<GameObject> objs = objNames.Select(x => GameObject.Find(x)).ToList();
        foreach(GameObject obj in objs)
        {
            Material m = obj.GetComponent<Renderer>().sharedMaterial;
            if (dict[m] != null)
            {
                if (obj.GetComponent<RandomSoundPlayer>() == null) obj.AddComponent<RandomSoundPlayer>();
                foreach(AudioSource audiosrc in obj.GetComponents<AudioSource>())
                {
                    DestroyImmediate(audiosrc);
                }
                foreach(AudioSource audiosrc in dict[m])
                {
                    AudioSource audio = obj.AddComponent<AudioSource>();
                    audio.playOnAwake = false;
                    audio.clip = audiosrc.clip;
                    audio.spatialBlend = audiosrc.spatialBlend;
                    audio.maxDistance = audiosrc.maxDistance;
                    audio.rolloffMode = AudioRolloffMode.Custom;
                    audio.SetCustomCurve(AudioSourceCurveType.CustomRolloff, audiosrc.GetCustomCurve(AudioSourceCurveType.CustomRolloff));
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShoot : MonoBehaviour {

    public RenderTexture rt;

    public void TakeScreenShoot()
    {
        RenderTexture.active = rt;
        Texture2D virtualPhoto =
            new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
        virtualPhoto.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);

        RenderTexture.active = null; //can help avoid errors 

        byte[] bytes;
        bytes = virtualPhoto.EncodeToPNG();

        System.IO.File.WriteAllBytes(
            OurTempSquareImageLocation(), bytes);
    }

    private string OurTempSquareImageLocation()
    {
        string r = Application.persistentDataPath + "/prnt_scrn.png";
        return r;
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

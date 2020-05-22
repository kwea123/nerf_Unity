using System;
using System.IO;
using UnityEngine;
using UnityEditor.Experimental.AssetImporters;

[ScriptedImporter(1, "vol")]
public class LoadVolume : ScriptedImporter
{
    public int size=512;
    // Start is called before the first frame update
    Texture3D Load(string path)
    {
        byte[] volumeData = File.ReadAllBytes(path);
        uint[] vF = new uint[volumeData.Length / 4];
        Buffer.BlockCopy(volumeData, 0, vF, 0, volumeData.Length);
        Texture3D tex = new Texture3D(size, size, size, TextureFormat.RGBA32, false);
        Color[] colors = new Color[size*size*size];
        tex.SetPixels(colors, 0); // all colors to (0, 0, 0, 0) first
        for (int c = 0; c < vF.Length; c += 2)
        {
            int x = (int)vF[c];
            int i = x / size / size;
            int j = (x - size * size * i) / size;
            int k = x % size;
            uint col = vF[c + 1];
            float r = ((col & 0xFF000000) >> 24) / 255.0f;
            float g = ((col & 0x00FF0000) >> 16) / 255.0f;
            float b = ((col & 0x0000FF00) >> 8) / 255.0f;
            float a = (col & 0x000000FF) / 255.0f;

            Color color = new Color(r, g, b, a);
            tex.SetPixel(k, j, i, color);
        }
        return tex;
    }

    public override void OnImportAsset(AssetImportContext ctx)
    {
        try
        {
            var tex3d = Load(ctx.assetPath);
            ctx.AddObjectToAsset("Volume", tex3d);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
}

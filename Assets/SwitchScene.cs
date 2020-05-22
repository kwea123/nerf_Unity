using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchScene : MonoBehaviour
{
    public Camera cam;
    public List<Texture3D> texs;

    private Renderer rend;
    private Vector3 origCamPos;
    private Quaternion origCamRot;
    private float xmin=0, xmax=1, ymin=0, ymax=1, zmin=0, zmax=1;
    private bool dissolve;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        origCamPos = cam.transform.position;
        origCamRot = cam.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnGUI()
    {
        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button)
        {
            fontSize = 20
        };
        for (int i = 0; i < texs.Count; i++)
        {
            
            if (GUI.Button(new Rect(10, 10 + 50 * i, 120, 40), texs[i].ToString().Split(' ')[0], buttonStyle))
                rend.material.SetTexture("_Volume", texs[i]);
        }
        GUIStyle toggleStyle = new GUIStyle(GUI.skin.toggle)
        {
            fontSize = 20
        };
        toggleStyle.normal.textColor = Color.black;
        toggleStyle.hover.textColor = Color.black;
        toggleStyle.onNormal.textColor = Color.black;
        toggleStyle.onHover.textColor = Color.black;

        toggleStyle.border = new RectOffset(14, 0, 14, 0);
        toggleStyle.padding = new RectOffset(15, 0, 0, 0);
        dissolve = GUI.Toggle(new Rect(10, 10 + 50 * texs.Count, 250, 50), dissolve, "Dissolve effect", toggleStyle);
        rend.material.SetFloat("_Dissolve", dissolve?1:0);

        GUIStyle textStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 20
        };
        textStyle.normal.textColor = Color.black;
        //GUI.Label(new Rect(10, 200, 150, 150), "Right click:\nRotate\nMiddle click:\nMove\nWheel:\nZoom", textStyle);

        //GUI.Label(new Rect(740, 30, 150, 50), "Display ranges", textStyle);
        //xmin = GUI.HorizontalSlider(new Rect(740, 80, 120, 20), xmin, 0, 1);
        //rend.material.SetFloat("_MinX", xmin);
        //GUI.Label(new Rect(880, 75, 150, 150), "X min : "+string.Format("{0:0.00}", xmin));
        //xmax = GUI.HorizontalSlider(new Rect(740, 110, 120, 20), xmax, 0, 1);
        //rend.material.SetFloat("_MaxX", xmax);
        //GUI.Label(new Rect(880, 105, 150, 150), "X max : " + string.Format("{0:0.00}", xmax));
        //ymin = GUI.HorizontalSlider(new Rect(740, 140, 120, 20), ymin, 0, 1);
        //rend.material.SetFloat("_MinY", ymin);
        //GUI.Label(new Rect(880, 135, 150, 150), "Y min : " + string.Format("{0:0.00}", ymin));
        //ymax = GUI.HorizontalSlider(new Rect(740, 170, 120, 20), ymax, 0, 1);
        //rend.material.SetFloat("_MaxY", ymax);
        //GUI.Label(new Rect(880, 165, 150, 150), "Y max : " + string.Format("{0:0.00}", ymax));
        //zmin = GUI.HorizontalSlider(new Rect(740, 200, 120, 20), zmin, 0, 1);
        //rend.material.SetFloat("_MinZ", zmin);
        //GUI.Label(new Rect(880, 195, 150, 150), "Z min : " + string.Format("{0:0.00}", zmin));
        //zmax = GUI.HorizontalSlider(new Rect(740, 230, 120, 20), zmax, 0, 1);
        //rend.material.SetFloat("_MaxZ", zmax);
        //GUI.Label(new Rect(880, 225, 150, 150), "Z max : " + string.Format("{0:0.00}", zmax));

        //if (GUI.Button(new Rect(10, 350, 150, 40), "Reset camera", buttonStyle))
        //{
        //    cam.transform.SetPositionAndRotation(origCamPos, origCamRot);
        //}
            
    }
}

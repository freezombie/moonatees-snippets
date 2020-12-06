using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fader : MonoBehaviour
{
    float teleportTimeMarker = -1;
    float alpha = 0;
    public float teleportFadeDuration = 0.2f;
    public Material fadeMaterial;
    Material fadeMaterialInstance; // not 100% sure that this is needed but included it anyway. It might be so that we don't modify the original material and lose it's settings or something.
    bool teleporting = false;
    bool fadeOut = false;
    bool fadeIn = false;
    bool fading = false;
    int materialFadeID;
    Mesh planeMesh;
    AreaBounds currentAreaBounds;
    GameManager gm;
    string flag = null;

    void Start ()
    {        
        if (fadeMaterial != null)
            fadeMaterialInstance = new Material(fadeMaterial);
        materialFadeID = Shader.PropertyToID("_Fade");
        planeMesh = new Mesh();
        Vector3[] verts = new Vector3[]
        {
            new Vector3(-1, -1, 0),
            new Vector3(-1, 1, 0),
            new Vector3(1, 1, 0),
            new Vector3(1, -1, 0)
        };
        int[] elts = new int[] { 0, 1, 2, 0, 2, 3 };
        planeMesh.vertices = verts;
        planeMesh.triangles = elts;
        planeMesh.RecalculateBounds();
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }	
	
	void Update ()
    {        
        if (fading && Time.time - teleportTimeMarker >= teleportFadeDuration / 2)
        {
            if(fadeIn)
            {
                fading = false;
            }
            else
            {
                if (teleporting)
                {
//                    Debug.Log("Going to teleport if");

                    currentAreaBounds.Teleport();
                }
                else if (flag != null)
                {
                    switch (flag)
                    {
                        case "LoadCockpit":
//                            Debug.Log("Cockpit should be loading now");
                            gm.loadCockpitScene(true);
                            flag = null;
                            break;
                        default:
                            break;
                    }
                }
            }

            teleportTimeMarker = Time.time;
            fadeIn = !fadeIn;
        }
    }

    void OnPostRender()
    {        
        if (fading)
        {
            alpha = Mathf.Clamp01((Time.time - teleportTimeMarker) / (teleportFadeDuration / 2));
            if (fadeIn)
                alpha = 1 - alpha;
            // Perform the fading in/fading out animation, if we are teleporting.  This is essentially a triangle wave
            // in/out, and the user teleports when it is fully black.          


            Matrix4x4 local = Matrix4x4.TRS(Vector3.forward * 0.3f, Quaternion.identity, Vector3.one);
            fadeMaterialInstance.SetPass(0);
            fadeMaterialInstance.SetFloat(materialFadeID, alpha);
            Graphics.DrawMeshNow(planeMesh, transform.localToWorldMatrix * local);
        }
    }

    public void SetTeleporting(bool boolean, AreaBounds ab)
    {
        currentAreaBounds = ab;
        fading = boolean;
        teleporting = boolean;
    }

    public void Fade(float teleportFadeDuration, string flag)
    {
        this.teleportFadeDuration = teleportFadeDuration;
        fading = true;
        if (flag != null)
        {
            teleporting = false;
            this.flag = flag;
        }
    }
}

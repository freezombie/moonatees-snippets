using UnityEngine;
using System;
using System.Runtime;
using System.Collections;


public class PrintscreenButtonScript : MonoBehaviour
{
    private string d;
    private string d2;
    private string d3;
    private string dd;
    private string dd1;

    //this fo counting pictures
    private int i = 0;
  

   

    void OnGUI()
    {
        

        if (Event.current.type == EventType.keyUp && Event.current.keyCode == KeyCode.SysReq)
        {
            d3 = (string)DateTime.Now.Minute.ToString();
            d2 = (string)DateTime.Now.Hour.ToString();
            d = (string)DateTime.Now.Day.ToString();
            dd = (string)DateTime.Now.Month.ToString();
            dd1 = (string)DateTime.Now.Year.ToString();


            Application.CaptureScreenshot( dd1+"."+dd + "." + d + "_" + d2 + "." + d3 + "_" + i + ".jpg");
            i++;
            Debug.Log(i);

        }
        
       
       
           

        

    }
}
    


    

    
        
    





using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class userdata 
{
    public string fname;
    public string lname;
    public long contactno;

    public userdata(loginscript newlogin)
    {
        fname=newlogin.fname;
        lname=newlogin.lname;
        contactno=newlogin.contactno;

    }
}
   
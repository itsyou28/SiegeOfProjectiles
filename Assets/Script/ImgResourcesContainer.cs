using UnityEngine;
using System.Collections;

public class ImgResourcesContainer : MonoBehaviour {

    static ImgResourcesContainer instance = null;
    public static ImgResourcesContainer Inst
    {
        get
        {
            if (instance == null)
                instance = GameObject.FindObjectOfType<ImgResourcesContainer>();

            return instance;
        }
    }

    public Sprite enemyHP;
    public Sprite enemyShield;
    public Sprite reduceBar;
}

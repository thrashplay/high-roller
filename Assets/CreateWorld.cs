using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateWorld : MonoBehaviour
{
    public GameObject tile;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 500;
        QualitySettings.vSyncCount = 0;

        for (var z = 0; z < 160; z++)
        {
            for (var x = 0; x < 32; x++)
            {
                var height = Random.Range(-2F, 0F);
                var instance = Instantiate(tile, new Vector3(x - 16, height - z, -z), Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

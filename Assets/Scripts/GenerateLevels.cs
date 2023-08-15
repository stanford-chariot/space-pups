using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateLevels : MonoBehaviour
{
    public GameObject[] section;

    public int zPos = 8;

    public bool creatingSection = false;

    public int sectionNum;

    // Update is called once per frame
    void Update()
    {
        if (creatingSection == false)
        {
            creatingSection = true;
            StartCoroutine(GenerateSection());
        }
    }
    // Infinite generate level
    IEnumerator GenerateSection()
    {
        sectionNum = Random.Range(0, 7);
        Instantiate(section[sectionNum], new Vector3(0, 0, zPos), Quaternion.identity);
        zPos += 50;
        yield return new WaitForSeconds(3);
        creatingSection = false;
    }
}

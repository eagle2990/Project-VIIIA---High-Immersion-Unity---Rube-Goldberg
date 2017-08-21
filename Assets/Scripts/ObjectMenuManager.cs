using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectMenuManager : MonoBehaviour
{
    private List<GameObject> objectList;
    public List<GameObject> objectPrefabList;
    public Text uiText;
    private int currentObject = 0;

    void Start()
    {
        currentObject = 0;
        objectList = new List<GameObject>();
        foreach (Transform child in transform)
        {
            if (child.gameObject.CompareTag(Constants.ObjectsTags.MENU))
                objectList.Add(child.gameObject);
        }
        uiText.text = objectList[currentObject].name;

    }

    public void MenuLeft()
    {
        objectList[currentObject].SetActive(false);
        currentObject--;
        if (currentObject < 0)
        {
            currentObject = objectList.Count - 1;
        }
        objectList[currentObject].SetActive(true);
        uiText.text = objectList[currentObject].name;
    }

    public void MenuRight()
    {
        objectList[currentObject].SetActive(false);
        currentObject++;
        if (currentObject > objectList.Count - 1)
        {
            currentObject = 0;
        }
        objectList[currentObject].SetActive(true);
        uiText.text = objectList[currentObject].name;
    }

    public void SpawnCurrentObject(Vector3 position)
    {
        Quaternion thumbnailRotation = objectList[currentObject].transform.rotation;
        Instantiate(objectPrefabList[currentObject],
            position,
            thumbnailRotation);
    }

    public void SpawnCurrentObject()
    {
        SpawnCurrentObject(objectPrefabList[currentObject].transform.position);
    }
}

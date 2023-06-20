using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class OrganCollision : MonoBehaviour
{
    private static List<string> activeGameObjects = new List<string>();
    private static TextMeshProUGUI organTextBox;
    private Material whiteMaterial; // Declare the material variable at the class level

    void Awake()
    {
        // Load the white material in Awake instead of the instance field initializer
        whiteMaterial = Resources.Load<Material>("Materials/white");
    }

    void Start()
    {

        if (organTextBox == null)
        {
            GameObject canvas = GameObject.Find("CanvasOrgan");
            organTextBox = canvas.transform.Find("Organ text box").GetComponent<TextMeshProUGUI>();
            MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
            meshRenderer.material = whiteMaterial;

        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Enter");
            AddMaterial();
            //gameObject.GetComponent<MeshRenderer>().enabled = true;
            AddGameObjectName(gameObject.name);
            UpdateTextBox();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Exit");
            MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
            meshRenderer.material = whiteMaterial;
            RemoveGameObjectName(gameObject.name);
            UpdateTextBox();
        }
    }

    void AddGameObjectName(string gameObjectName)
    {
        if (!activeGameObjects.Contains(gameObjectName))
        {
            activeGameObjects.Add(gameObjectName);
        }
    }

    void RemoveGameObjectName(string gameObjectName)
    {
        activeGameObjects.Remove(gameObjectName);
    }

    void UpdateTextBox()
    {
        organTextBox.text = string.Join("\n", activeGameObjects);
    }

    // Function to add the material to each model taking into account the name. 
    private void AddMaterial()
    {
        // Load the materials from the Resources folder
        Material veinMaterial = Resources.Load<Material>("Materials/vein");
        Material arteryMaterial = Resources.Load<Material>("Materials/artery");
        Material kidneyMaterial = Resources.Load<Material>("Materials/kidney");
        Material liverMaterial = Resources.Load<Material>("Materials/liver");
        Material skinMaterial = Resources.Load<Material>("Materials/skin");
        Material ureterMaterial = Resources.Load<Material>("Materials/ureter");
        Material spleenMaterial = Resources.Load<Material>("Materials/spleen");
        Material bloodVesselsMaterial = Resources.Load<Material>("Materials/blood_vessels");
        Material heartMaterial = Resources.Load<Material>("Materials/heart");
        Material uterusMaterial = Resources.Load<Material>("Materials/uterus_vagina");
        Material bladderMaterial = Resources.Load<Material>("Materials/bladder");
        Material boneMaterial = Resources.Load<Material>("Materials/bone");
        Material urachusMaterial = Resources.Load<Material>("Materials/urachus");
        Material ligamentMaterial = Resources.Load<Material>("Materials/ligament");

        
        string childName = gameObject.name.ToLower();

        if (childName.Contains("ve"))
        {
            // Assign the vein material
            MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
            meshRenderer.material = veinMaterial;

        }
        else if (childName.Contains("arter"))
        {
            // Assign the artery material
            MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
            meshRenderer.material = arteryMaterial;
        }
        else if (childName.Contains("kidney"))
        {
            // Assign the kidney material
            MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
            meshRenderer.material = kidneyMaterial;

        }
        else if (childName.Contains("liver"))
        {
            // Assign the liver material
            MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
            meshRenderer.material = liverMaterial;

        }
        else if (childName.Contains("back"))
        {
            // Assign the skin material
            MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
            meshRenderer.material = skinMaterial;

        }
        else if (childName.Contains("ureter"))
        {
            // Assign the ureter material
            MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
            meshRenderer.material = ureterMaterial;

        }
        else if (childName.Contains("spleen"))
        {
            // Assign the spleen material
            MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
            meshRenderer.material = spleenMaterial;

        }
        else if (childName.Contains("urachus"))
        {
            // Assign the blood vessels material
            MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
            meshRenderer.material = urachusMaterial;

        }
        else if (childName.Contains("ligament"))
        {
            // Assign the blood vessels material
            MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
            meshRenderer.material = ligamentMaterial;

        }
        else if (childName.Contains("heart"))
        {
            // Assign the heart material
            MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
            meshRenderer.material = heartMaterial;

        }
        else if (childName.Contains("uterus"))
        {
            // Assign the uterus material
            MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
            meshRenderer.material = uterusMaterial;

        }
        else if (childName.Contains("bladder"))
        {
            // Assign the uterus_vagina material
            MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
            meshRenderer.material = bladderMaterial;

        }
        else if (childName.Contains("iliac") || childName.Contains("femur") || childName.Contains("sacrum") || childName.Contains("cord"))
        {
            MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
            meshRenderer.material = boneMaterial;

        }
    }
    
}

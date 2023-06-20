using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.EventSystems;




public class ModelLoader : MonoBehaviour
{
    public string modelsFolderPath; // The path to the models folder inside Resources folder
    public string SagittalModelsFolderPath; // The alternative path to the models folder
    public string CoronalModelsFolderPath;
    public GameObject coronalGameObject;
    private string[] desiredNames = {
        "left iliac vein", "left iliac vein", "cava and right iliac vein", "cava and renal vein", "right gonadal vein",
        "left gonadal vein", "cava and right iliac vein", "portal venous system", "right gonadal artery", "right iliac artery",
        "aorta and left iliac artery", "celiac arterial trunk", "bladder", "right kidney", "left kidney",
        "left ureter", "right ureter", "posterior back", "left iliac bone", "spinal cord and ribs",
        "sacrum", "right femur", "left femur", "right iliac bone", "uterus and vagina",
        "heart", "spleen", "liver", "urachus", "left umbilical ligament",
        "right umbilical ligament"
    }; // Example array of desired names: REMOVE ANTERIOR WALL FROM MODELS FOLDER


    public GameObject modelsParent; // Parent GameObject to hold the instantiated models
    public string cameraName;
    //public GameObject tipParent; // Parent GameObject to hold the instantiated tool tip
    // New variables for the button and its states
    private GameObject changeViewButton;
    // Reference to the "NEW CAMERA"
    public Camera newCamera;

    private void Start()

    {   // Call the method to load the models by ascending order number
        LoadModels();

        // Call the method to add the specific material for each model
        //AddMaterial();

        // Call the method to display Unity screen in two halves
        DisplayScreenInTwoHalves();

    }
    private void Update()

    {
    }

    // Function for loading all the models and the components and scripts attached to them.
    private void LoadModels()
    {
        // Load all models from the specified folder
        GameObject[] models = Resources.LoadAll<GameObject>(modelsFolderPath);

        // Sort the models based on the number in their name in ascending order
        List<GameObject> sortedModels = models.OrderBy(model =>
        {
            int number = ExtractNumber(model.name);
            return number;
        }).ToList();

        // Create a parent GameObject if it doesn't exist
        if (modelsParent == null)
        {
            modelsParent = new GameObject("ModelsParent");
        }

        // Instantiate the models as child objects under the parent GameObject
        for (int i = 0; i < sortedModels.Count; i++)
        {
            GameObject instantiatedModel = Instantiate(sortedModels[i], modelsParent.transform);

            // Add MeshFilter component
            MeshFilter meshFilter = instantiatedModel.GetComponent<MeshFilter>();
            if (meshFilter == null)
            {
                meshFilter = instantiatedModel.AddComponent<MeshFilter>();
            }

            // Add MeshCollider component
            MeshCollider meshCollider = instantiatedModel.GetComponent<MeshCollider>();
            if (meshCollider == null)
            {
                meshCollider = instantiatedModel.AddComponent<MeshCollider>();
            }

            // Add MeshRenderer component
            MeshRenderer meshRenderer = instantiatedModel.AddComponent<MeshRenderer>();


            // Find and assign the mesh to the MeshFilter based on the original model's name
            string originalName = sortedModels[i].name;
            string prefabPath = Path.Combine(modelsFolderPath, originalName);
            GameObject prefab = Resources.Load<GameObject>(prefabPath);
            if (prefab != null)
            {
                // Instantiate the prefab to get access to its mesh
                GameObject prefabInstance = Instantiate(prefab);
                Mesh prefabMesh = prefabInstance.GetComponentInChildren<MeshFilter>().sharedMesh;

                if (prefabMesh != null)
                {
                    // Assign the prefab's mesh to the instantiated model's MeshFilter and MeshCollider
                    meshFilter.sharedMesh = prefabMesh;
                    meshCollider.sharedMesh = prefabMesh;
                }

                // Destroy the instantiated prefab
                Destroy(prefabInstance);
            }

            // Change the game object name to the desired name from the array
            if (i < desiredNames.Length)
            {
                instantiatedModel.name = desiredNames[i];
            }
            else
            {
                // Handle the case where there are more models than names in the array
                instantiatedModel.name = "ARSupportPiece" + i.ToString();
            }

            // Add the "OrganCollision" script to the models. 
            OrganCollision organCollision = instantiatedModel.gameObject.AddComponent<OrganCollision>();
           
        }
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

        // Get all child objects of the models parent
        Transform[] modelTransforms = modelsParent.GetComponentsInChildren<Transform>();

        // Loop through each child object and check its name for a match
        foreach (Transform childTransform in modelTransforms)
        {
            string childName = childTransform.gameObject.name.ToLower();

            if (childName.Contains("ve"))
            {
                // Assign the vein material
                MeshRenderer meshRenderer = childTransform.GetComponent<MeshRenderer>();
                meshRenderer.material = veinMaterial;

            }
            else if (childName.Contains("arter"))
            {
                // Assign the artery material
                MeshRenderer meshRenderer = childTransform.GetComponent<MeshRenderer>();
                meshRenderer.material = arteryMaterial;
            }
            else if (childName.Contains("kidney"))
            {
                // Assign the kidney material
                MeshRenderer meshRenderer = childTransform.GetComponent<MeshRenderer>();
                meshRenderer.material = kidneyMaterial;

            }
            else if (childName.Contains("liver"))
            {
                // Assign the liver material
                MeshRenderer meshRenderer = childTransform.GetComponent<MeshRenderer>();
                meshRenderer.material = liverMaterial;

            }
            else if (childName.Contains("back"))
            {
                // Assign the skin material
                MeshRenderer meshRenderer = childTransform.GetComponent<MeshRenderer>();
                meshRenderer.material = skinMaterial;

            }
            else if (childName.Contains("ureter"))
            {
                // Assign the ureter material
                MeshRenderer meshRenderer = childTransform.GetComponent<MeshRenderer>();
                meshRenderer.material = ureterMaterial;

            }
            else if (childName.Contains("spleen"))
            {
                // Assign the spleen material
                MeshRenderer meshRenderer = childTransform.GetComponent<MeshRenderer>();
                meshRenderer.material = spleenMaterial;

            }
            else if (childName.Contains("urachus"))
            {
                // Assign the blood vessels material
                MeshRenderer meshRenderer = childTransform.GetComponent<MeshRenderer>();
                meshRenderer.material = urachusMaterial;

            }
            else if (childName.Contains("ligament"))
            {
                // Assign the blood vessels material
                MeshRenderer meshRenderer = childTransform.GetComponent<MeshRenderer>();
                meshRenderer.material = ligamentMaterial;

            }
            else if (childName.Contains("heart"))
            {
                // Assign the heart material
                MeshRenderer meshRenderer = childTransform.GetComponent<MeshRenderer>();
                meshRenderer.material = heartMaterial;

            }
            else if (childName.Contains("uterus"))
            {
                // Assign the uterus material
                MeshRenderer meshRenderer = childTransform.GetComponent<MeshRenderer>();
                meshRenderer.material = uterusMaterial;

            }
            else if (childName.Contains("bladder"))
            {
                // Assign the uterus_vagina material
                MeshRenderer meshRenderer = childTransform.GetComponent<MeshRenderer>();
                meshRenderer.material = bladderMaterial;

            }
            else if (childName.Contains("iliac") || childName.Contains("femur") || childName.Contains("sacrum") || childName.Contains("cord"))
            {
                MeshRenderer meshRenderer = childTransform.GetComponent<MeshRenderer>();
                meshRenderer.material = boneMaterial;

            }
        }
    }


    // Function for extracting the number in the original model name.
    private int ExtractNumber(string name)
    {
        Match match = Regex.Match(name, @"\d+");
        if (match.Success)
        {
            return int.Parse(match.Value);
        }
        return int.MaxValue; // Return a very large value if no number is found
    }



    private void DisplayScreenInTwoHalves()
    {
        
        // Create a Canvas GameObject
        GameObject canvasObject = new GameObject("Canvas");
        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObject.AddComponent<CanvasScaler>();
        canvasObject.AddComponent<GraphicRaycaster>();

        // Set the Canvas dimensions to cover the entire screen
        RectTransform canvasRectTransform = canvas.GetComponent<RectTransform>();
        canvasRectTransform.anchorMin = Vector2.zero;
        canvasRectTransform.anchorMax = Vector2.one;
        canvasRectTransform.offsetMin = Vector2.zero;
        canvasRectTransform.offsetMax = Vector2.zero;

        // Create a RawImage GameObject as a child of the Canvas
        GameObject rawImageObject = new GameObject("RawImage");
        rawImageObject.transform.SetParent(canvasObject.transform, false);
        RawImage rawImage = rawImageObject.AddComponent<RawImage>();

        // Set the RawImage dimensions to cover the right half of the screen
        RectTransform rawImageRectTransform = rawImage.GetComponent<RectTransform>();
        rawImageRectTransform.anchorMin = new Vector2(0.5f, 0f);
        rawImageRectTransform.anchorMax = Vector2.one;
        rawImageRectTransform.pivot = new Vector2(0.5f, 0.5f);
        rawImageRectTransform.offsetMin = Vector2.zero;
        rawImageRectTransform.offsetMax = Vector2.zero;

        // Create an empty GameObject and add a VideoPlayer component and CameraSelector script
        GameObject cameraObject = new GameObject("CameraObject");
        VideoPlayer videoPlayer = cameraObject.AddComponent<VideoPlayer>();
        CameraSelector cameraSelector = cameraObject.AddComponent<CameraSelector>();

        // Assign the RawImage to the CameraSelector script
        cameraSelector.rawImage = rawImage;

        // Set the name of the external USB camera to connect
        cameraSelector.cameraName = cameraName;

        // Create a RawImage GameObject for the "NEW CAMERA" as a child of the Canvas
        GameObject newCameraRawImageObject = new GameObject("NewCameraRawImage");
        newCameraRawImageObject.transform.SetParent(canvasObject.transform, false);
        RawImage newCameraRawImage = newCameraRawImageObject.AddComponent<RawImage>();

        // Set the RawImage dimensions to cover the right half of the screen
        RectTransform newCameraRawImageRectTransform = newCameraRawImage.GetComponent<RectTransform>();
        newCameraRawImageRectTransform.anchorMin = new Vector2(0f, 0f);
        newCameraRawImageRectTransform.anchorMax = new Vector2(0.5f, 1f);
        newCameraRawImageRectTransform.pivot = new Vector2(1f,1f);
        newCameraRawImageRectTransform.offsetMin = Vector2.zero;
        newCameraRawImageRectTransform.offsetMax = Vector2.zero;

        // Set the "NEW CAMERA" target texture to the Raw Image
        RenderTexture newCameraRenderTexture = new RenderTexture(Screen.width / 2, Screen.height, 0);
        newCamera.targetTexture = newCameraRenderTexture;
        newCameraRawImage.texture = newCameraRenderTexture;
    }


}
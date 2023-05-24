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
    private string[] desiredNames = {
        "right iliac artery", "right iliac artery", "left iliac artery", "aorta and renal artery", "blood_vess_1",
        "blood_vess_2", "left iliac artery", "pulmonary artery", "blood_vess_3", "left iliac vein",
        "right iliac vein", "inferior cava vein", "bladder", "right kidney", "left kidney",
        "left ureter", "right ureter", "back", "left hip", "spinal cord and ribs",
        "sacrum", "right femur", "left femur", "right hip", "uterus and vagina",
        "heart", "spleen", "liver", "central triad", "right triad", 
        "left triad"
    }; // Example array of desired names: REMOVE ANTERIOR SKIN FROM MODELS FOLDER

    public GameObject modelsParent; // Parent GameObject to hold the instantiated models
    public string cameraName;
    public GameObject tipParent; // Parent GameObject to hold the instantiated tool tip
    // New variables for the button and its states
    private GameObject mainButton;
    private bool areModelsShown = true;

    private void Start()

    {   // Call the method to load the models by ascending order number
        LoadModels();

        // Call the method to add the specific material for each model
        AddMaterial();

        // Call the method to display Unity screen in two halves
        DisplayScreenInTwoHalves();

        // Call the method to create a tool tip
        ToolTip();

        //Call the method to create main button for show or hide all organs
        MainButton();


    }
    private void Update()

    {

       // CheckOrganCollision();
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
                instantiatedModel.name = "NewName" + i.ToString();
            }
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

        // Get all child objects of the models parent
        Transform[] modelTransforms = modelsParent.GetComponentsInChildren<Transform>();

        // Loop through each child object and check its name for a match
        foreach (Transform childTransform in modelTransforms)
        {
            string childName = childTransform.gameObject.name.ToLower();

            if (childName.Contains("vein"))
            {
                // Assign the vein material
                MeshRenderer meshRenderer = childTransform.GetComponent<MeshRenderer>();
                meshRenderer.material = veinMaterial;

            }
            else if (childName.Contains("artery"))
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
            else if (childName.Contains("blood_vess"))
            {
                // Assign the blood vessels material
                MeshRenderer meshRenderer = childTransform.GetComponent<MeshRenderer>();
                meshRenderer.material = bloodVesselsMaterial;

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
            else if (childName.Contains("hip") || childName.Contains("femur") || childName.Contains("sacrum") || childName.Contains("cord"))
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

    // Function for splitting the canvas screen in two halves.
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
    }



    // Function for creating the tool tip and assign it as child of Image marker. 
    private void ToolTip()
    {
        // Create a parent GameObject if it doesn't exist
        if (tipParent == null)
        {
            tipParent = new GameObject("ToolTipParent");
        }

        // Create a sphere GameObject
        GameObject tip = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        tip.name = "Tip";
        // Set the tag for collisions
        tip.gameObject.tag = "Player";
        tip.transform.localScale = Vector3.one * 10f;

        // Set the position of the tip GameObject
        tip.transform.position = new Vector3(43.6f, 0f, 27.9f);

        // Add Rigidbody component without gravity
        Rigidbody rigidbody = tip.AddComponent<Rigidbody>();
        rigidbody.useGravity = false;


        // Add SphereCollider component and set it as a trigger
        SphereCollider sphereCollider = tip.GetComponent<SphereCollider>();
        if (sphereCollider == null)
        {
            sphereCollider = tip.AddComponent<SphereCollider>();
        }

        // Check the isTrigger condition
        sphereCollider.isTrigger = true;

        // Set the parent of the sphere GameObject
        tip.transform.SetParent(tipParent.transform);
    }

    private void MainButton()
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

        // Create a Button GameObject as a child of the Canvas
        GameObject buttonObject = new GameObject("MainButton");
        buttonObject.transform.SetParent(canvasObject.transform, false);
        RectTransform buttonRectTransform = buttonObject.AddComponent<RectTransform>();

        // Set the position and size of the button
        buttonRectTransform.anchorMin = new Vector2(0f, 1f);
        buttonRectTransform.anchorMax = new Vector2(0f, 1f);
        buttonRectTransform.pivot = new Vector2(0f, 1f);
        buttonRectTransform.anchoredPosition = Vector2.zero;
        buttonRectTransform.sizeDelta = new Vector2(100f, 50f);

        // Add the Button component to the button GameObject
        Button buttonComponent = buttonObject.AddComponent<Button>();

        // Add a Text component to the button GameObject and set its properties
        GameObject textObject = new GameObject("Text");
        textObject.transform.SetParent(buttonObject.transform, false);
        RectTransform textRectTransform = textObject.AddComponent<RectTransform>();
        Text textComponent = textObject.AddComponent<Text>();
        textComponent.text = "Hide";
        textComponent.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        textComponent.alignment = TextAnchor.MiddleCenter;
        textComponent.color = Color.black;

        // Set the position and size of the text
        textRectTransform.anchorMin = Vector2.zero;
        textRectTransform.anchorMax = Vector2.one;
        textRectTransform.pivot = new Vector2(0.5f, 0.5f);
        textRectTransform.anchoredPosition = Vector2.zero;
        textRectTransform.sizeDelta = Vector2.zero;

        // Set the text font size based on the button size
        float fontSize = buttonRectTransform.sizeDelta.y * 0.5f;
        textComponent.fontSize = Mathf.FloorToInt(fontSize);

        // Add a yellow Image component to the button GameObject
        Image buttonImage = buttonObject.AddComponent<Image>();
        buttonImage.color = Color.yellow;

        // Add an onClick event to the button to call the ToggleModels function
        buttonComponent.onClick.AddListener(ToggleModels);

        // Add an event handler for PointerEnter to change button color to pink
        EventTrigger eventTrigger = buttonObject.AddComponent<EventTrigger>();
        EventTrigger.Entry pointerEnterEntry = new EventTrigger.Entry();
        pointerEnterEntry.eventID = EventTriggerType.PointerEnter;
        pointerEnterEntry.callback.AddListener((data) => { buttonImage.color = Color.magenta; });
        eventTrigger.triggers.Add(pointerEnterEntry);

        // Assign the MainButton to the buttonObject
        mainButton = buttonObject;

        // Add an event handler for PointerExit to restore the original button color
        EventTrigger.Entry pointerExitEntry = new EventTrigger.Entry();
        pointerExitEntry.eventID = EventTriggerType.PointerExit;
        pointerExitEntry.callback.AddListener((data) => { buttonImage.color = Color.yellow; });
        eventTrigger.triggers.Add(pointerExitEntry);

        // Set the parent of the button GameObject
        buttonObject.transform.SetParent(canvasObject.transform, false);
    }


    // Function for toggling the visibility of the models
    private void ToggleModels()
    {
        // Toggle the visibility of the models
        areModelsShown = !areModelsShown;

        // Get all child objects of the models parent
        Transform[] modelTransforms = modelsParent.GetComponentsInChildren<Transform>();

        // Loop through each child object and toggle the MeshRenderer component
        foreach (Transform childTransform in modelTransforms)
        {
            MeshRenderer meshRenderer = childTransform.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {   
                string modelName = childTransform.name;
                if (modelName.Contains("marker"))
                {
                    meshRenderer.enabled = true;
                }
                else
                {   // If the organs are hidden
                    if (areModelsShown == false)
                    {
                        meshRenderer.enabled = areModelsShown;

                        // Add the "OrganCollision" script to the models. 
                        OrganCollision organCollision = childTransform.gameObject.AddComponent<OrganCollision>();
                    }
                    else
                    {   // If the organs are shown, disable the collider. 
                        meshRenderer.enabled = areModelsShown;

                        // Remove the "OrganCollision" script from the models if it exists
                        OrganCollision organCollision = childTransform.gameObject.GetComponent<OrganCollision>();
                        if (organCollision != null)
                        {
                            Destroy(organCollision);
                        }
                    }
                }
            }
        }

        // Update the button text and color
        Text buttonText = mainButton.GetComponentInChildren<Text>();
        buttonText.text = areModelsShown ? "Hide" : "Show";

        Image buttonImage = mainButton.GetComponent<Image>();
        if (areModelsShown)
        {
            buttonImage.color = Color.yellow;
        }
        else
        {
            buttonImage.color = Color.green;
        }

    }

}


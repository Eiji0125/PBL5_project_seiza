using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ColorManager : MonoBehaviour {


 	public GameObject menuPanel;
	public InputActionReference openMenuAction;
	

    public static ColorManager instance = null;
    public Color color = Color.blue;
    public string cloudLabel = "";

    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        //DontDestroyOnLoad(gameObject);

        //cloudLabel = Instantiate(Resources.Load("CloudLabel", typeof(GameObject)), new Vector3(100.0f, 100.0f, 100.0f), Quaternion.identity) as GameObject;

	openMenuAction.action.Enable();
	openMenuAction.action.performed += ToggleMenu;
	InputSystem.onDeviceChange += OnDeviceChange;
    }

    public void OnDestroy()
    {
        openMenuAction.action.Disable();
	    openMenuAction.action.performed -= ToggleMenu;
	    InputSystem.onDeviceChange += OnDeviceChange;
    }

    private void ToggleMenu(InputAction.CallbackContext context)
    {
        menuPanel.SetActive(!menuPanel.activeSelf);
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        switch (change)
        {
           case InputDeviceChange.Disconnected:
                openMenuAction.action.Disable();
                openMenuAction.action.performed -= ToggleMenu;
                break;
            case InputDeviceChange.Reconnected:
                openMenuAction.action.Enable();
                openMenuAction.action.performed += ToggleMenu;
                break;
        }
    }


    public static ColorManager Instance
    {
        get
        {
            return instance;
        }
    }
    
	
	// Update is called once per frame
	void OnColorChange (HSBColor color) {
        this.color = color.ToColor();
	}

   

}

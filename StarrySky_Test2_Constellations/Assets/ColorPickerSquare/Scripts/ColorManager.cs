using UnityEngine;

public class ColorManager : MonoBehaviour
{
    public static ColorManager instance = null;
    public Color color = Color.blue;
    public string cloudLabel = "";

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    public static ColorManager Instance
    {
        get
        {
            return instance;
        }
    }

    public void OnColorChange(HSBColor color)
    {
        this.color = color.ToColor();
    }
}

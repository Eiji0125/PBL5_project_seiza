using System.Collections.Generic;
using UnityEngine;

public class StarField : MonoBehaviour {
  [Range(0, 100)]
  [SerializeField] private float starSizeMin = 0f;
  [Range(0, 100)]
  [SerializeField] private float starSizeMax = 5f;
  private List<StarDataLoader.Star> stars;
  private List<GameObject> starObjects;
  private Dictionary<int, GameObject> constellationVisible = new();

  private readonly int starFieldScale = 400;

  void Start() {
    // Read in the star data.
    StarDataLoader sdl = new();
    stars = sdl.LoadData();
    starObjects = new();
    foreach (StarDataLoader.Star star in stars) {
      // Create star game objects.
      GameObject stargo = GameObject.CreatePrimitive(PrimitiveType.Quad);
      stargo.transform.parent = transform;
      stargo.name = $"HR {star.catalog_number}";
      stargo.transform.localPosition = star.position * starFieldScale;
      //stargo.transform.localScale = Vector3.one * Mathf.Lerp(starSizeMin, starSizeMax, star.size);
      stargo.transform.LookAt(transform.position);
      stargo.transform.Rotate(0, 180, 0);
      Material material = stargo.GetComponent<MeshRenderer>().material;
      material.shader = Shader.Find("Unlit/StarShader");
      material.SetFloat("_Size", Mathf.Lerp(starSizeMin, starSizeMax, star.size));
      material.color = star.colour;
      starObjects.Add(stargo);
    }
  }

  // Could also do in Update with Time.deltatime scaling.
  private void FixedUpdate() {
    if (Input.GetKey(KeyCode.Mouse1)) {
      Camera.main.transform.RotateAround(Camera.main.transform.position, Camera.main.transform.right, Input.GetAxis("Mouse Y"));
      Camera.main.transform.RotateAround(Camera.main.transform.position, Vector3.up, -Input.GetAxis("Mouse X"));
    }
    return;
  }

  private void OnValidate() {
    if (starObjects != null) {
      for (int i = 0; i < starObjects.Count; i++) {
        // Update the size set in the shader.
        Material material = starObjects[i].GetComponent<MeshRenderer>().material;
        material.SetFloat("_Size", Mathf.Lerp(starSizeMin, starSizeMax, stars[i].size));
      }
    }
  }

    // A constellation is a tuple of the stars and the lines that join them.
    private readonly List<(int[], int[])> constellations = new()
    {
        // 2024-7-1 0:00 Shinjuku Area

        //East Side
        // Cygnus (ÇÕÇ≠ÇøÇÂÇ§ç¿) Press 0

        (new int[] { 7924, 7796, 7949, 8115, 7615, 7417, 7528, 7420 },
    new int[] {7924, 7796, 7796, 7949, 7949, 8115, 7796, 7528,
               7528, 7420, 7796, 7615, 7615, 7417}),


        //Pisces(Ç§Ç®ç¿) Press 1 
        (new int[] { 352, 383, 360, 510, 596, 549, 489, 224, 9072, 8969, 8984, 8911, 8852, 8916 },
    new int[] { 352,383,383,360,360,352,360,510,510,596,596,549,549,489,489,224,224,
                9072,9072,8969,8969,8984,8984,8911,8911,8852,8852,8916,8916,8969}),
        
        //Triangulum(Ç≥ÇÒÇ©Ç≠ç¿) Press 2
        (new int[] { 664, 622, 544 },
    new int[] { 664, 622, 622, 544, 544, 664 }),


        //South Side
        // Aquila (ÇÌÇµç¿) Press 3
        (new int[] { 7610, 7525, 7557, 7602, 7710, 7429, 7377, 7236, 7235, 7176 },
    new int[] { 7610, 7525, 7525, 7557, 7525, 7235, 7557, 7602,7557,
               7429, 7602, 7710,7377, 7429, 7236, 7377, 7176, 7235,7602,7710}),


        //Ara(Ç≥Ç¢ÇæÇÒç¿) Press 4 Displayed in lower side
        (new int[] { 6510, 6461, 6462, 6500, 6229, 6285, 6295 },
    new int[] { 6510, 6461, 6461, 6462, 6462, 6500, 6500, 6229, 6229, 6285, 6285, 6295, 6295, 6510 }),


        //Scutum(ÇΩÇƒç¿) Press 5
        (new int[] { 7063, 7032, 7020, 6930, 6973 },
    new int[] { 7063, 7032, 7032, 7020, 7020, 6930, 6930, 6973, 6973, 7063 }),
        //North side


        // Ursa Major (Ç®Ç®ÇÆÇ‹ç¿) Press 6
        (new int[] { 3569, 3594, 3775, 3888, 3323, 3757, 4301, 4295, 4554, 4660,
                4905, 5054, 5191, 4518, 4335, 4069, 4033, 4377, 4375 },
    new int[] { 3569, 3594, 3594, 3775, 3775, 3888, 3888, 3323, 3323, 3757,
                3757, 3888, 3757, 4301, 4301, 4295, 4295, 3888, 4295, 4554,
                4554, 4660, 4660, 4301, 4660, 4905, 4905, 5054, 5054, 5191,
                4554, 4518, 4518, 4335, 4335, 4069, 4069, 4033, 4518, 4377, 4377, 4375 }),

        // Leo (ÇµÇµç¿) Press7
        (new int[] { 3982, 4534, 4057, 4357, 3873, 4031, 4359, 3975, 4399, 4386, 3905, 3773, 3731 },
    new int[] { 4534, 4357, 4534, 4359, 4357, 4359, 4357, 4057, 4057, 4031,
                4057, 3975, 3975, 3982, 3975, 4359, 4359, 4399, 4399, 4386,
                4031, 3905, 3905, 3873, 3873, 3975, 3873, 3773, 3773, 3731, 3731, 3905 }),


        // Leo Minor (Ç±Ç∂Çµç¿) Press 8
        (new int[] { 3800, 3974, 4100, 4247, 4090 },
    new int[] { 3800, 3974, 3974, 4100, 4100, 4247, 4247, 4090, 4090, 3974 }),


        // Cancer (Ç©Ç…ç¿) Press 9
        (new int[] { 3475, 3449, 3461, 3572, 3249 },
    new int[] { 3475, 3449, 3449, 3461, 3461, 3572, 3461, 3249 }),


        //West Side

        // Scorpius (Ç≥ÇªÇËç¿) Press a
        (new int[] { 6508, 6580, 6615, 6553, 6380, 6271, 6252, 6241, 6165, 6134, 6084, 5953, 5984, 5944, 5928 },
    new int[] {6508,6580,6580,6615,6615,6553,6553,6380,6380,6271,6271,6252,6252,6241,
               6241,6165,6165,6134,6134,6084,6084,5953,5953,5984,5953,5944,5944,5928}),


        // Corona Borealis (Ç©ÇÒÇﬁÇËç¿) Press b
        (new int[] { 5971, 5947, 5889, 5849, 5793, 5747, 5778 },
    new int[] { 5971, 5947, 5947, 5889, 5889, 5849, 5849, 5793, 5793, 5747, 5747, 5778 }),



    };

    private void Update()
    {
        // Check for numeric presses and toggle the constellation highlighting.
        for (int i = 0; i < 10; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i))
            {
                ToggleConstellation(i);
            }
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            int specialConstellationIndex = 10; // Define the index for the special constellation.
            ToggleConstellation(specialConstellationIndex);
        }
        // Check for the 'b' key press and toggle another specific constellation.
        if (Input.GetKeyDown(KeyCode.B))
        {
            int specialConstellationIndexB = 11; // Define the index for the special constellation 'b'.
            ToggleConstellation(specialConstellationIndexB);
        }


        // Check for the 'c' key press and toggle another specific constellation.
        if (Input.GetKeyDown(KeyCode.C))
        {
            int specialConstellationIndexC = 12; // Define the index for the special constellation 'c'.
            ToggleConstellation(specialConstellationIndexC);
        }
    }


    void ToggleConstellation(int index) {
    // Safety check the index is valid.
    if ((index < 0) || (index >= constellations.Count)) {
      return;
    }

    // Toggle on or off.
    if (constellationVisible.ContainsKey(index)) {
      RemoveConstellation(index);
    } else {
      CreateConstellation(index);
    }
  }

  void CreateConstellation(int index) {
    int[] constellation = constellations[index].Item1;
    int[] lines = constellations[index].Item2;

    // Change the colours of the stars
    foreach (int catalogNumber in constellation) {
      // Remember list is 0-up catalog numbers are 1-up.
      starObjects[catalogNumber - 1].GetComponent<MeshRenderer>().material.color = Color.white;
    }

    GameObject constellationHolder = new($"Constellation {index}");
    constellationHolder.transform.parent = transform;
    constellationVisible[index] = constellationHolder;

    // Draw the constellation lines.
    for (int i = 0; i < lines.Length; i += 2) {
      // Parent it to our constellation object so we can delete them all later.
      GameObject line = new("Line");
      line.transform.parent = constellationHolder.transform;
      // Defaults to white and width 1 which works for us.
      LineRenderer lineRenderer = line.AddComponent<LineRenderer>();
      // Doesn't get assigned a material so just dig out one that works.
      lineRenderer.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
      // Disable useWorldSpace so it will track the parent game object.
      lineRenderer.useWorldSpace = false;
      Vector3 pos1 = starObjects[lines[i] - 1].transform.position;
      Vector3 pos2 = starObjects[lines[i + 1] - 1].transform.position;
      // Offset them so they don't occlude the stars, 3 chosen by trial and error.
      Vector3 dir = (pos2 - pos1).normalized * 3;
      lineRenderer.positionCount = 2;
      lineRenderer.SetPosition(0, pos1 + dir);
      lineRenderer.SetPosition(1, pos2 - dir);
    }
  }

  void RemoveConstellation(int index) {
    int[] constallation = constellations[index].Item1;

    // Toggling off set the stars back to the original colour.
    foreach (int catalogNumber in constallation) {
      // Remember list is 0-up catalog numbers are 1-up.
      starObjects[catalogNumber - 1].GetComponent<MeshRenderer>().material.color = stars[catalogNumber - 1].colour;
    }
    // Remove the constellation lines.
    Destroy(constellationVisible[index]);
    // Remove from our dictionary as it's now off.
    constellationVisible.Remove(index);
  }

}


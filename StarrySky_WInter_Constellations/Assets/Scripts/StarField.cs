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
        // 2024-1-1 0:00 Shinjuku Area


        // Camelopardalis (きりん座)
        (new int[] { 2527, 2209, 1542, 1148, 1155, 1129, 1603, 1568 },
      new int[] { 2527, 2209, 2209, 1542, 1542, 1148, 1542, 1603, 1148, 1155, 1155, 1129, 1603, 1568 }),



        //Ursa Minor(こぐま座)
        (new int[] { 424, 6789, 6322, 5903, 5563, 5735, 6116 },
      new int[] { 424, 6789, 6789, 6322, 6322, 5903, 5903, 5563, 5563, 5735, 5735, 6116, 6116, 5903 }),

        // Lynx (やまねこ座)
        (new int[] { 3705, 3690, 3612, 3579, 3275, 2818, 2560, 2238 },
      new int[] { 3705, 3690, 3690, 3612, 3612, 3579, 3579, 3275, 3275, 2818,
                2818, 2560, 2560, 2238 }),

        // Auriga (ぎょしゃ座)
        (new int[] { 2077, 2088, 2095, 1791, 1577, 1708, 1605, 1612 },
      new int[] { 2077, 2088, 2088, 2095, 2095, 1791, 1791, 1577, 1577, 1708, 1708, 2077, 1708, 1605, 1605, 1612 }),


        // Aries (おひつじ座)
        (new int[] { 838, 617, 553, 546 },
      new int[] { 838, 617, 617, 553, 553, 546 }),

        // Canis Minor (こいぬ座)
        (new int[] { 2943, 2845 },
      new int[] { 2943, 2845 }),


        // Monoceros (いっかくじゅう座)
        (new int[] { 2970, 3188, 2714, 2356, 2227, 2506, 2298, 2385, 2456, 2479 },
      new int[] { 2970, 3188, 3188, 2714, 2714, 2356, 2356, 2227, 2714, 2506,
                2506, 2298, 2298, 2385, 2385, 2456, 2479, 2506, 2479, 2385 }),


        // Orion (オリオン座)
        (new int[] { 1948, 1903, 1852, 2004, 1713, 2061, 1790, 1907, 2124,
                2199, 2135, 2047, 2159, 1543, 1544, 1570, 1552, 1567 },
      new int[] { 1713, 2004, 1713, 1852, 1852, 1790, 1852, 1903, 1903, 1948,
                1948, 2061, 1948, 2004, 1790, 1907, 1907, 2061, 2061, 2124,
                2124, 2199, 2199, 2135, 2199, 2159, 2159, 2047, 1790, 1543,
                1543, 1544, 1544, 1570, 1543, 1552, 1552, 1567, 2135, 2047 }),

        //Pegasus(ペガスス座)  Press 
        (new int[] { 39, 15, 8775, 8781, 8450, 8308, 8667, 8315, 8454 },
    new int[] { 39, 15, 15, 8775, 8775, 8781, 8781, 39, 8781, 8450, 8450, 8308, 8775, 8667, 8667, 8315, 8775, 8454 }),

        //Bootes(うしかい座) Press 
        (new int[] { 5404, 5329, 5351, 5435, 5602, 5681, 5429, 5340, 5478, 5235 },
    new int[] { 5404, 5329, 5329, 5351, 5351, 5404, 5351, 5435, 5435, 5602, 5602, 5681, 5681, 5340, 5429, 5340, 5435, 5429, 5340, 5478, 5340, 5235 }),




        // Scorpius (さそり座) Press
        (new int[] { 6508, 6580, 6615, 6553, 6380, 6271, 6252, 6241, 6165, 6134, 6084, 5953, 5984, 5944, 5928 },
    new int[] {6508,6580,6580,6615,6615,6553,6553,6380,6380,6271,6271,6252,6252,6241,
 6241,6165,6165,6134,6134,6084,6084,5953,5953,5984,5953,5944,5944,5928}),

        //Libra (てんびん座) Press
        (new int[] { 5685, 5787, 5531, 5794, 5603 },
    new int[] { 5685, 5787, 5787, 5531, 5531, 5685, 5787, 5794, 5603, 5531 }),

        //Draco(りゅう座）
        (new int[] { 4434, 4787, 5291, 5744, 5986, 6132, 6396, 6920, 7310, 6688, 6705, 6536, 6555 },
      new int[] { 4434, 4787, 4787, 5291, 5291, 5744, 5744, 5986, 5986, 6132, 6132, 6396, 6396, 6920, 6920, 7310, 7310, 6688, 6688, 6705, 6705, 6536, 6536, 6555, 6555, 6688 }),
        //Cetus(くじら座）
        (new int[] { 813, 896, 911, 804, 718, 779, 681, 539, 740, 509, 585, 188, 74, 334, 402 },
      new int[] { 813, 896, 896, 911, 911, 804, 804, 718, 718, 813, 804, 779, 779, 681, 681, 539, 539, 740, 539, 509, 509, 585, 509, 188, 188, 74, 74, 334, 334, 402, 402, 539 }),
        //Canis Major (おおいぬ座)
        (new int[] { 2657, 2596, 2574, 2491, 2294, 2429, 2580, 2618, 2693, 2827 },
      new int[] { 2657, 2596, 2596, 2574, 2574, 2657, 2596, 2491, 2491, 2294, 2294, 2429, 2429, 2580, 2580, 2618, 2618, 2693, 2693, 2827, 2693, 2491 }),
        //Perseus(ペルセウス座）Press 
        (new int[] { 1261, 1303, 1273, 1122, 1220, 1228, 1203, 1131, 1017, 915, 834, 854, 937, 936, 921, 799, 496 },
      new int[] { 1261, 1303, 1303, 1273, 1273, 1122, 1220, 1122, 1017, 1122, 1220, 1228, 1228, 1203, 1203, 1131, 1017, 915,
                  915, 834, 834, 854, 854, 937, 937, 936, 936, 921, 799, 496, 1017, 937, 937, 799, 1220, 936 }),

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
        if (Input.GetKeyDown(KeyCode.Q))
        {
            int specialConstellationIndex = 10; // Define the index for the special constellation.
            ToggleConstellation(specialConstellationIndex);
        }
        // Check for the 'b' key press and toggle another specific constellation.
        if (Input.GetKeyDown(KeyCode.W))
        {
            int specialConstellationIndexW = 11; // Define the index for the special constellation 'b'.
            ToggleConstellation(specialConstellationIndexW);
        }

        // Check for the 'c' key press and toggle another specific constellation.
        if (Input.GetKeyDown(KeyCode.E))
        {
            int specialConstellationIndexE = 12; // Define the index for the special constellation 'c'.
            ToggleConstellation(specialConstellationIndexE);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            int specialConstellationIndexR = 13; // Define the index for the special constellation 'c'.
            ToggleConstellation(specialConstellationIndexR);
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


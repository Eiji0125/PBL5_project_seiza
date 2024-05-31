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


        //North Side
        // Camelopardalis (Ç´ÇËÇÒç¿)
        (new int[] { 2527, 2209, 1542, 1148, 1155, 1129, 1603, 1568 },
      new int[] { 2527, 2209, 2209, 1542, 1542, 1148, 1542, 1603, 1148, 1155, 1155, 1129, 1603, 1568 }),


        // Lynx (Ç‚Ç‹ÇÀÇ±ç¿)
        (new int[] { 3705, 3690, 3612, 3579, 3275, 2818, 2560, 2238 },
      new int[] { 3705, 3690, 3690, 3612, 3612, 3579, 3579, 3275, 3275, 2818,
                2818, 2560, 2560, 2238 }),
        //Ursa Minor(Ç±ÇÆÇ‹ç¿)
        (new int[] { 424, 6789, 6322, 5903, 5563, 5735, 6116 },
      new int[] { 424, 6789, 6789, 6322, 6322, 5903, 5903, 5563, 5563, 5735, 5735, 6116, 6116, 5903 }),


        //West Side


        // Auriga (Ç¨ÇÂÇµÇ·ç¿)
        (new int[] { 2077, 2088, 2095, 1791, 1577, 1708, 1605, 1612 },
      new int[] { 2077, 2088, 2088, 2095, 2095, 1791, 1577, 1577, 1708, 1708, 2077, 1708, 1605, 1605, 1612 }),


        // Aries (Ç®Ç–Ç¬Ç∂ç¿)
        (new int[] { 838, 617, 553, 546 },
      new int[] { 838, 617, 617, 553, 553, 546 }),


        //South Side


        // Canis Minor (Ç±Ç¢Ç ç¿)
        (new int[] { 2943, 2845 },
      new int[] { 2943, 2845 }),


        // Monoceros (Ç¢Ç¡Ç©Ç≠Ç∂Ç„Ç§ç¿)
        (new int[] { 2970, 3188, 2714, 2356, 2227, 2506, 2298, 2385, 2456, 2479 },
      new int[] { 2970, 3188, 3188, 2714, 2714, 2356, 2356, 2227, 2714, 2506,
                2506, 2298, 2298, 2385, 2385, 2456, 2479, 2506, 2479, 2385 }),


        // Orion (ÉIÉäÉIÉìç¿)
        (new int[] { 1948, 1903, 1852, 2004, 1713, 2061, 1790, 1907, 2124,
                2199, 2135, 2047, 2159, 1543, 1544, 1570, 1552, 1567 },
      new int[] { 1713, 2004, 1713, 1852, 1852, 1790, 1852, 1903, 1903, 1948,
                1948, 2061, 1948, 2004, 1790, 1907, 1907, 2061, 2061, 2124,
                2124, 2199, 2199, 2135, 2199, 2159, 2159, 2047, 1790, 1543,
                1543, 1544, 1544, 1570, 1543, 1552, 1552, 1567, 2135, 2047 }),

    };

  private void Update() {
    // Check for numeric presses and toggle the constellation highlighting.
    for (int i = 0; i < 10; i++) {
      if (Input.GetKeyDown(KeyCode.Alpha0 + i)) {
        ToggleConstellation(i);
      }
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


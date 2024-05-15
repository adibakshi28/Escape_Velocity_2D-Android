using UnityEngine;
using System.Collections;

public class TestureScrolling : MonoBehaviour {

	public GameObject movingDustorbit,movingDustNonOrbit;
	[HideInInspector]
	public bool scroll = true;

	public float scrollSpeed = 0.5F;
	public bool x, y;

	private Renderer rend;

	void Start() {
		rend = GetComponent<Renderer>();
	}
	void Update() {
		if (scroll) {
			float offset = Time.time * scrollSpeed;
			if (x) {
				rend.material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
			}
			if (y) {
				rend.material.SetTextureOffset("_MainTex", new Vector2(0, offset));
			}
			if ((x) & (y)) {
				rend.material.SetTextureOffset("_MainTex", new Vector2(offset, offset));
			}
		}
		movingDustNonOrbit.SetActive (scroll);
		movingDustorbit.SetActive (!scroll);
	}
}

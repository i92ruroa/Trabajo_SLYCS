using UnityEngine;
using System.Collections;

public class Box : MonoBehaviour {

	// La curva
	public AnimationCurve curve;

	// El objeto colocador
	public GameObject spawnPrefab;

	// La siguiente Caja
	public GameObject nextPrefab;

	IEnumerator sample() {
		// Posición inicial
		Vector2 pos = transform.position;

		// movimiento de la cuerva
		for (float t=0; t<curve.keys[curve.length-1].time; t+=Time.deltaTime) {
			
			transform.position = new Vector2(pos.x, pos.y + curve.Evaluate(t));

			// pasamos a la siguiente curva
			yield return null;
		}

		// Coloca la moneda
		if (spawnPrefab)
			Instantiate(spawnPrefab, transform.position + Vector3.up, Quaternion.identity);

		// Destruye la caja
		if (nextPrefab)
			Instantiate(nextPrefab, transform.position, Quaternion.identity);
		Destroy(gameObject);
	}

	void OnCollisionEnter2D(Collision2D coll) {
		// golpea desde abajo?
		if (coll.contacts[0].point.y < transform.position.y)
			StartCoroutine("sample");
	}
}

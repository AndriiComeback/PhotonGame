using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 moveup = new Vector2(5, 1);
    Vector2 movedown = new Vector2(5, -1);
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start() {
        GameObject[] list = GameObject.FindGameObjectsWithTag("Ghost");
        if (list.Length > 1) {
            Destroy(list[0]);
        }
        StartCoroutine(Move());
    }
    IEnumerator Move() {
        while (true) {
            rb.velocity = moveup;
            yield return new WaitForSeconds(2f);
            rb.velocity = movedown;
            yield return new WaitForSeconds(2f);
        }
    }
}

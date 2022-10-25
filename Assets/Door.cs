using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private GameObject door;
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            door.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -1);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            door.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 1);
        }
    }
    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            door.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 1);
        }
    }
}

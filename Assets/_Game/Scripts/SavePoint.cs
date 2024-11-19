using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {

        if (collision.tag == "Player") {
            Debug.Log("Touched Player");
            Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
            if (player != null) {
                player.SavePoint();
            } else {
                Debug.LogError("Player component not found!");
            }
        }
    }
}

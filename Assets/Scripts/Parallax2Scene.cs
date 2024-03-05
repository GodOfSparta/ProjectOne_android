using UnityEngine;

public class Parallax2Scene : MonoBehaviour
{
    Transform player;
    Material material;
    Vector2 offset = Vector2.zero;
    [SerializeField] float scale = 1.0f;
    void Start()
    {
        player = transform.root;
        material = GetComponent<SpriteRenderer>().material;
    }

    void Update()
    {
        offset = new Vector2(player.position.x / 100f / scale, 0f);
        material.mainTextureOffset = offset;
    }
}
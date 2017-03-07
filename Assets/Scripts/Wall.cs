using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {

	public Sprite dmgSprite;
	public int hp = 4;
	public AudioClip chopSound1;
	public AudioClip chopSound2;
    public GameObject[] foodTiles;
	private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Awake () {
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	public void DamageWall(int loss){
		spriteRenderer.sprite = dmgSprite;
		hp-=loss;
		SoundManager.instance.RandomizeSfx(chopSound1, chopSound2);
        if (hp <= 0)
        {
            gameObject.SetActive(false);
            
            if(Random.Range(0f, 1f) >= .5f)
            {
                Instantiate(foodTiles[(int)Random.Range(0f, 1.5f)], transform.position, Quaternion.identity);
            }
            
        }
			
	}
	
}

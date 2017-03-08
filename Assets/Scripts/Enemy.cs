using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Enemy : MovingObject {

	public int playerDamage;
	public AudioClip enemyAttack1;
	public AudioClip enemyAttack2;
    public AudioClip chopSound1;
    public AudioClip chopSound2;
    public GameObject[] foodTiles;
    public Text enemyText;

    private Animator animator;
	private Transform target;
	private bool skipMove;
    private int hp = 20;

    // Use this for initialization
    protected override void Start () {
		GameManager.instance.AddEnemyToList(this);
		animator = GetComponent<Animator>();
		target = GameObject.FindGameObjectWithTag("Player").transform;
		base.Start();
	}

	protected override void AttemptMove<T>(int xDir, int yDir){
		if (skipMove){
			skipMove = false;
			return;
		}
		base.AttemptMove<T>(xDir, yDir);
		skipMove = true;
	}

	public void MoveEnemy(){
		int xDir = 0;
		int yDir = 0;

		if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
			yDir = target.position.y > transform.position.y ? 1 : -1;
		else
			xDir = target.position.x > transform.position.x ? 1 : -1;
		
		AttemptMove<Player>(xDir, yDir);
	}

	protected override void OnCantMove<T>(T component){
		Player hitPlayer = component as Player;

		animator.SetTrigger("EnemyAttack");

		hitPlayer.LoseFood(playerDamage);
		SoundManager.instance.RandomizeSfx(enemyAttack1, enemyAttack2);
	}

    public void DamageEnemy(int loss)
    {
        //spriteRenderer.sprite = dmgSprite;
        hp -= loss;
        SoundManager.instance.RandomizeSfx(chopSound1, chopSound2);
        enemyText.text = "Enemy Health: " + hp;
        if (hp <= 0)
        {
            gameObject.SetActive(false);

            if (Random.Range(0f, 1f) >= .3f)
            {
                Instantiate(foodTiles[(int)Random.Range(0f, 1.5f)], transform.position, Quaternion.identity);
            }

        }

    }
}

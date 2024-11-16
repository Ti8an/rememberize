using System.Collections;
using UnityEngine;

public class CardScript : MonoBehaviour
{
    [SerializeField] private SpriteRenderer img;
    private Animator animator;
    public int id = -1;
    private GameManager gameManager;
    private AudioSource audioSource;

    public void Init(Sprite sprite, int id)
    {
        audioSource = GetComponent<AudioSource>();
        img.sprite = sprite;
        this.id = id;
        animator = GetComponent<Animator>();
        gameManager = GameManager.Instance;
    }

    private void OnMouseDown()
    {
        if(this.GetComponent<Rigidbody2D>())return;
        if(gameManager.selectCard_1 == this) return;
        if(gameManager.selectCard_2 == this) return;

        if(gameManager.selectCard_1 == null)
        {
            gameManager.selectCard_1 = this;
            Anim_open();
            gameManager.MotionCount++;
            gameManager.audio_Click.Play();
            return;
        }

        if (gameManager.selectCard_2 == null)
        {
            gameManager.selectCard_2 = this;
            Anim_open();
            gameManager.MotionCount++;
            gameManager.Logic();
            gameManager.audio_Click.Play();
            return;
        }
    }

    public void Anim_open() => animator.SetTrigger("open");

    public void Anim_close()
    {
        animator.SetTrigger("close");
        audioSource.Play();
    }

    public void Anim_destroy() => animator.SetTrigger("des");

    public IEnumerator Anim_delayAndClose(float t)
    {
        yield return new WaitForSeconds(t);
        gameManager.selectCard_1 = null;
        gameManager.selectCard_2 = null;
        Anim_close();
    }

}

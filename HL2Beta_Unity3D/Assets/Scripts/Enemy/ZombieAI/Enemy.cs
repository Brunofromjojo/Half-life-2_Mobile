using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [Header("Settings")]
    [Header("Animation component")]
    [SerializeField] public Animator m_ani;

    [Header("Pathfinding component")]
    [SerializeField] UnityEngine.AI.NavMeshAgent m_agent;

    [Header("Main character")]
    [SerializeField] public GameObject m_player;
    [SerializeField] public LayerMask groundLayer, playerLayer;
    [SerializeField] public FP_Health playerhp;

    [Header("Enemy movement speed")]
    [SerializeField] public float m_movSpeed = 0.5f;

    [Header("Enemy rotation speed")]
    [SerializeField] public float m_rotSpeed = 30;

    [Header("Enemy attack range")]
    [SerializeField] public float attackRange = 1.5f;
    [SerializeField] public float sightRange = 10.0f;
    [SerializeField] float Range = 10f;
    [SerializeField] FieldOfView EnemyFOV;


    [Header("Timer")]
    [SerializeField] float m_timer = 2f;

    [Header("Enemy State")]
    //Bool to know whether the enemy is activated or not
    public bool isProvoked = false;

    [Header("Attack Reload")]
    [SerializeField] float AtkReload = 2f;

    [Header("Enemy health points")]
    [SerializeField] public int m_life = 15;
    [SerializeField] public Actor EnemyHP;

    [Header("Sounds")]
    //SoundEffects
    [SerializeField] public AudioSource Sound;
    [SerializeField] public AudioClip[] AlertSounds;
    [SerializeField] public AudioClip[] IdleSounds;
    [SerializeField] public AudioClip[] DeathSounds;
    [SerializeField] public AudioClip[] attackSounds;
    [SerializeField] public AudioClip[] StrikeSounds;


    // Use this for initialization
    void Start()
    {
        // Get component
        m_ani = this.GetComponent<Animator>();
        EnemyHP = this.GetComponent<Actor>();
        m_agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        m_ani.SetBool("idle", true);
        Sound.PlayOneShot(IdleSounds[UnityEngine.Random.Range(0, IdleSounds.Length)]);

    }

    void Update()
    {

        var distanceToTarget = Vector3.Distance(m_player.transform.position, transform.position);
        if (distanceToTarget <= Range)
        {
            if (EnemyFOV.canSeePlayer == true)
            {
                //if the target entered the range then activate
                isProvoked = true;
            }
        }

        // If the player life is 0, do nothing
        if (playerhp.m_life <= 0)
            return;

        if (EnemyHP.maxHealth > EnemyHP.currentHealth)
        {
            //if the target entered the range then activate
            isProvoked = true;
        }

        if (isProvoked == true)
        {
            // Get the current animation status
            AnimatorStateInfo stateInfo = m_ani.GetCurrentAnimatorStateInfo(0);

            // Get the current animation status
            if (stateInfo.fullPathHash == Animator.StringToHash("Base Layer.idle") && !m_ani.IsInTransition(0))
            {
                m_ani.SetBool("idle", false);

                // Standby for a certain period of time
                m_timer -= Time.deltaTime;
                if (m_timer > 0)
                    return;

                // If the distance from the player is less than attack range, it will enter the attack animation state.
                if (Vector3.Distance(transform.position, m_player.transform.position) < attackRange)
                {
                    Sound.PlayOneShot(attackSounds[UnityEngine.Random.Range(0, attackSounds.Length)]);
                    m_ani.SetBool("attack", true);

                }
                else
                {
                    // Reset timer
                    m_timer = 2f;

                    // Set pathfinding target points
                    m_agent.SetDestination(m_player.transform.position);

                    // Enter running animation state
                    m_ani.SetBool("run", true);
                    Sound.PlayOneShot(AlertSounds[UnityEngine.Random.Range(0, AlertSounds.Length)]);
                }
            }

            // If running
            if (stateInfo.fullPathHash == Animator.StringToHash("Base Layer.run") && !m_ani.IsInTransition(0))
            {

                m_ani.SetBool("run", false);


                // Reposition the player every second
                m_timer -= Time.deltaTime;
                if (m_timer < 0)
                {
                    m_agent.SetDestination(m_player.transform.position);

                    m_timer = 1;

                }
                //Chase the player
                MoveTo();

                //If the distance is less than attack range from the player, attack towards the player
                if (Vector3.Distance(transform.position, m_player.transform.position) <= attackRange)
                {
                    //Reset pathfinding
                    m_agent.ResetPath();
                    // Enter attack state
                    m_ani.SetBool("attack", true);
                }
            }

            // If under attack
            if (stateInfo.fullPathHash == Animator.StringToHash("Base Layer.attack") && !m_ani.IsInTransition(0))
            {

                // Facing the player
                RotateTo();

                m_ani.SetBool("attack", false);
                // If the attack animation ends, it will re-enter the idle state.
                if (stateInfo.normalizedTime >= AtkReload)
                {
                    if (Vector3.Distance(transform.position, m_player.transform.position) <= (attackRange + 1.1f))
                    {
                        playerhp.OnDamage(25);
                        Sound.PlayOneShot(StrikeSounds[UnityEngine.Random.Range(0, StrikeSounds.Length)]);
                    }

                    //Enter idle state.
                    m_ani.SetBool("idle", true);
                    Sound.PlayOneShot(IdleSounds[UnityEngine.Random.Range(0, IdleSounds.Length)]);

                    // Reset timer
                    m_timer = 0.5f;
                }
            }
            //if in death state
            if (stateInfo.fullPathHash == Animator.StringToHash("Base Layer.death") && !m_ani.IsInTransition(0))
            {
                //Death animation when playback is complete
                if (stateInfo.normalizedTime >= AtkReload)
                {
                    //Update enemy count
                    //    m_spawn.m_enemyCount--;
                    //Add points
                    //    GameManager.Instance.SetScore(100);
                    //destroy itself
                    Destroy(this.gameObject);
                }
            }
        }
    }

    // Turn to target point
    void RotateTo()
            {
                // Get target direction
                Vector3 targetdir = m_player.transform.position - transform.position;
                // Calculate new direction
                Vector3 newDir = Vector3.RotateTowards(transform.forward, targetdir, m_rotSpeed * Time.deltaTime, 0.0f);
                // Rotate to new direction
                transform.rotation = Quaternion.LookRotation(newDir);
            }

            // Pathfinding
            void MoveTo()
            {
                float speed = m_movSpeed * Time.deltaTime;
                m_agent.Move(transform.TransformDirection((new Vector3(0, 0, speed))));

            }

            //gizmos to give a visual representation of range for debugging process
            //for altering the range for a better gameplay.
            private void OnDrawGizmosSelected()
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, Range);
            }

            //Damage function
            //   public void OnDamage(int damage){
            //		m_life -= damage;
            //       //If the health points is 0, enter the death state
            //        if (m_life <= 0) {
            //			m_ani.SetBool("death",true);
            //
            //            Sound.PlayOneShot(DeathSounds[UnityEngine.Random.Range(0, DeathSounds.Length)]);
            //       }
            //	}
        }

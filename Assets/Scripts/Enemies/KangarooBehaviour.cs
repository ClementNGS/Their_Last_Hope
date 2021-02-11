using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KangarooBehaviour : MonoBehaviour
{
    public float jumpForce;
    public float jumpTime;
    public float raycastOffset;
    public bool drawDebug;

    Rigidbody2D _rb;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        StartCoroutine(JumpAfterWait(jumpTime));
    }

    // Update is called once per frame
    void Update()
    {
        var pos = transform.position;
        pos.x -= 2 * Time.deltaTime;
        pos.x -= Runner.RunnerSpeed;
        transform.position = pos;

        if (transform.position.x < -10) Destroy(gameObject);
    }

    bool IsOnFloor()
    {
        var pos = transform.position + new Vector3(0, raycastOffset);

        RaycastHit2D hit = Physics2D.Raycast(pos, new Vector2(0, -1), 0.2f);
        if (drawDebug)
            Debug.DrawRay(pos, -0.2f * Vector3.up, Color.red);
        return hit.collider != null && hit.collider.gameObject.tag == "Ground";
    }

    IEnumerator JumpAfterWait(float time)
    {
        // J'attends 1 seconde, ou 2, ou 3...
        yield return new WaitForSeconds(time);
        yield return new WaitUntil(() => IsOnFloor());

        _rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => IsOnFloor());
        yield return JumpAfterWait(time);
    }
}

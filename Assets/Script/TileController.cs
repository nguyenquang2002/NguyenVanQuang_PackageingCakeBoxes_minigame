using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{

    public TileCell cell { get; set; }

    public TileCell startCell;

    // Start is called before the first frame update
    void Start()
    {
        if(startCell != null)
        {
            cell = startCell;
        }
    }

    public void Spawn(TileCell cell)
    {
        if (this.cell != null)
        {
            this.cell.tileController = null;
        }
        this.cell = cell;
        this.cell.tileController = this;
        
    }
    public void MoveTo(TileCell cell)
    {
        if(this.cell != null)
        {
            this.cell.tileController = null;
        }
        this.cell = cell;
        this.cell.tileController = this;
        

        Vector2 newPosition = new Vector2((cell.address.x - 1) * 320 + 540, (1 - cell.address.y) * 320 + 960);

        StartCoroutine(Animate(newPosition));
    }

    private IEnumerator Animate(Vector3 to)
    {
        float timer = 0f;
        float duration = 0.1f;
        Vector3 from = transform.position;
        while (timer < duration)
        {
            transform.position = Vector3.Lerp(from, to, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }
        transform.position = to;
    }

}

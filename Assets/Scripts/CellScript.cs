using UnityEngine;

public class CellScript : MonoBehaviour
{
    public void AnimateCell() {
        FindObjectOfType<MyGameManager>().AnimateCell();
    }
}

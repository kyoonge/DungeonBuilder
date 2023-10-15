using UnityEngine;
namespace Model
{
    public class Tile : MonoBehaviour
    {
        public int x { get; private set; }
        public int y { get; private set; }

        [SerializeField]
        public bool isBlock = false;
        private bool isMouseOver = false;
        private Color originalColor;
        public Transform Visual;
        public bool isTrack = false;

        public void Set(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        private void Start()
        {
            Visual = transform.Find("Visual");
            originalColor = Visual.GetComponent<SpriteRenderer>().material.color;
        }

        private void OnMouseEnter()
        {
            if (GridManager.Instance.isTileMode)
            {
                Visual.GetComponent<SpriteRenderer>().color = Color.green;
                Tiles.Instance.curX = x;
                Tiles.Instance.curY = y;
            }
            if (Tiles.Instance.isDrag)
            {

            }
        }

        private void OnMouseExit()
        {
            if (GridManager.Instance.isTileMode)
                Visual.GetComponent<SpriteRenderer>().color = Color.white;
        }

    }
}

namespace Model
{
    public class Character : BaseUnit
    {
        public void UpdateTile(Tile tile)
        {
            if (origin != tile)
                origin = tile;
        }
    }
}


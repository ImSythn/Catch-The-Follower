using System.Drawing;

namespace CatchTheFollower
{
    public class Block : IObject, IUnmoveable 
    {
        public Image Sprite { get; set; }

        private Tile tile;

        public Block(Tile tile)
        {
            Sprite = Image.FromFile(@"..\..\Images\block.png");
            this.tile = tile;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatchTheFollower
{
    public class Portal: IObject, IUnmoveable
    {
        public Image Sprite { get; set; }
        public Tile Source { get; private set; }
        public Tile Target { get; private set; }

        public Portal(Tile sourceTile, Tile targetTile)
        {
            Sprite = Image.FromFile(@"..\..\Images\portal.png");
            Source = sourceTile;
            Target = targetTile;
        }
        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Arcadus
{
    class Background:Entity {

        public Background(Vector2 pos, String asset, ContentManager content)
            :base(pos, new Vector2 (0.0f),asset, content)
        {
        }

    
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGame.Exceptions
{
    public class GameDontExistsException:Exception
    {
        public GameDontExistsException(): base("This game don't exists")
        {

        }
    }
}

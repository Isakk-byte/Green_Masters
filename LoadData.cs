using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Green_Masters.Gamestate;

namespace Green_Masters
{
    public class LoadData
    {
        public void LoadMethod(out gameStates activateState)
        {
            Thread.Sleep(5000); //Låtsasladda i 5 sekunder
            activateState = Gamestate.gameStates.startMenu;
        }
    }
}

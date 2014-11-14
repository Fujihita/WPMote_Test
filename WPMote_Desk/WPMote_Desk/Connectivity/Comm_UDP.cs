using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPMote_Desk.Connectivity
{
    class Comm_UDP
    {
        #region "Class properties"
        int intPort = 8045;

        public int Port
        {
            get
            {
                return intPort;
            }
            set
            {
                if (value > 0) intPort = value;
            }
        }

        #endregion

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneQueryNGUI.RuntimeUtilityBean
{
    public class RuntimeDataHandler
    {
        private static RuntimeDataHandler handler = null;

        /*
         * Determin if a logged in user is admin or not.
         */
        private bool isAdmin;
        public bool IsAdmin
        {
            get
            {
                return isAdmin;
            }
            set
            {
                isAdmin = value;
            }
        }
        
        /*
         * Empty constructor.
         */
        private RuntimeDataHandler()
        {

        }

        public static RuntimeDataHandler getInstance()
        {
            if (handler == null)
            {
                handler = new RuntimeDataHandler();
            }
            return handler;
        }
    }
}

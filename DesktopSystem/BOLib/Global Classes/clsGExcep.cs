using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOLib
{
    public class GExcep        
    {
        public class FactoryException : System.Exception
        {
            string _msgID = string.Empty;

            public FactoryException(string msgID)
            {
                this._msgID = msgID;
            }

            public override string Message
            {
                get
                {
                    return this._msgID;
                }
            }
        }                  
    }

   
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class MISAException : Exception
    {
        public string MISAMessage { get; set; }
        public IDictionary MISAErrorData { get; set; }
        public MISAException(string msg, IDictionary? data = null)
        {
            MISAMessage = msg;
            MISAErrorData = data;
        }
        public override string Message => MISAMessage;
        public override IDictionary Data => MISAErrorData;
    }
}

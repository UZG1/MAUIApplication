using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUIApplication
{
    public interface IWebActionCallback
    {
        public void SendAnswerToHtml<T>(T answer);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPA.DAL.Models;

namespace UPA.BLL.Interface
{
    public interface ISetting
    {
        public IEnumerable<Setting> GetAll();
    }
}

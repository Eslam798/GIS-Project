using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPA.BLL.Interface;
using UPA.DAl;
using UPA.DAL.Models;

namespace UPA.BLL.Repository
{
    public class SettingService: ISetting
    {
        private UPAModel _context;

        public SettingService(UPAModel context)
        {
            _context = context;
        }


        public IEnumerable<Setting> GetAll()
        {
            return _context.Settings.ToList();
        }
    }
}

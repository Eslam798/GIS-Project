using UPA.DAL.Models;
using UPA.ViewModels.ViewModels.SupplierVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPA.BLL.Interfaces
{
    public interface ISupplierService
    {
        IEnumerable<IndexSupplierVM.GetData> ListSuppliers();
        EditSupplierVM GetById(int id);
        int Add(CreateSupplierVM supplierObj);
        int Update(EditSupplierVM supplierObj);
        int CountSuppliers();
        int Delete(int id);

        int CreateSupplierAttachment(SupplierAttachment attachObj);
        List<SupplierAttachment> GetSupplierAttachmentsBySupplierId(int supplierId);

        GenerateSupplierCodeVM GenerateSupplierCode();
    }

}

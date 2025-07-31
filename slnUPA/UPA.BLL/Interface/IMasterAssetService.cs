using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPA.BLL.Specifications;
using UPA.DAL.Models;
using UPA.ViewModels.ViewModels.HospitalVM;
using UPA.ViewModels.ViewModels.MasterAssetVM;

namespace UPA.BLL.Interfaces
{
    public interface IMasterAssetService
    {

        public IEnumerable<IndexMasterAssetVM.GetData> GetAll();
        IndexMasterAssetVM ListMasterAsset();
        IndexMasterAssetVM ListMasterAsset(CountOfAssetsSpecParams @params);
        IEnumerable<IndexMasterAssetVM.GetData> AutoCompleteMasterAssetName(string name, int hospitalId);
        IEnumerable<IndexMasterAssetVM.GetData> AutoCompleteMasterAssetName(string name);
        IEnumerable<MasterAsset> DistinctAutoCompleteMasterAssetName(string name);
        IEnumerable<MasterAsset> GetAllMasterAssetsByHospitalId(int hospitalId);
        EditMasterAssetVM GetById(int id);
        ViewMasterAssetVM ViewMasterAsset(int id);
        int Add(CreateMasterAssetVM masterAssetObj);
        int Update(EditMasterAssetVM masterAssetObj);
        int UpdateMasterAssetImageAfterInsert(CreateMasterAssetVM masterAssetObj);
        int Delete(int id);
        int CreateMasterAssetDocuments(MasterAssetAttachment attachObj);
        IEnumerable<MasterAssetAttachment> GetAttachmentByMasterAssetId(int assetId);
        int DeleteMasterAssetAttachment(int id);

        MasterAssetAttachment GetLastDocumentForMsterAssetId(int masterId);

        public GeneratedMasterAssetCodeVM GenerateMasterAssetCode();
    }
}

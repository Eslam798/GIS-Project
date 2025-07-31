﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPA.ViewModels.ViewModels.AssetPeriorityVM;

namespace UPA.BLL.Interfaces
{
  public  interface IAssetPeriorityService
    {
        IEnumerable<IndexAssetPeriorityVM.GetData> GetAll();
        EditAssetPeriorityVM GetById(int id);
        int Add(CreateAssetPeriorityVM assetPeriorityObj);
        int Update(EditAssetPeriorityVM assetPeriorityObj);
        int Delete(int id);
    }
}

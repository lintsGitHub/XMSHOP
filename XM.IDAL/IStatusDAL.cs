﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XM.Model;

namespace XM.IDAL
{
    public interface IStatusDAL
    {
        int AddStatus(StatusEntity status);
        bool DeleteStatus(string id);
        bool EditStatus(StatusEntity status);
        IEnumerable<T> QryStatus<T>(Dictionary<string, object> paras, out int iCount);
        int Save(Dictionary<string, object> paras);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using BladderChange.Web.Data.Model.Entities;
using BladderChange.Web.Data.Model.Facades;


namespace BladderChange.Web.Controllers
{
    public class BladderDataController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<BladderChangeInfo> Get()
        {
            var facade = new BladderChangeInfoFacade();
            var list = facade.GetLastestBladderChangeInfoList();
            return list;
        }

    }
}
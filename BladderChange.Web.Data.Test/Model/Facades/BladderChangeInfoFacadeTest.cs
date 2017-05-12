using Microsoft.VisualStudio.TestTools.UnitTesting;
using BladderChange.Web.Data.Model.Facades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BladderChange.Service.Data.Test
{
    [TestClass()]
    public class BladderChangeInfoFacadeTest
    {
        [TestMethod()]
        public void GetLastestBladderChangeInfoListTest()
        {
            var facade = new BladderChangeInfoFacade();
            var list = facade.GetLastestBladderChangeInfoList();
            Assert.AreEqual(25, list.Count);
        }
    }
}
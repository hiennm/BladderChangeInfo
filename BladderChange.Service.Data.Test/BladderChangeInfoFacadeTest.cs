using BladderChange.Service.Data.Model.Facades;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BladderChange.Service.Data.Test {
    using Data.Model.Entities;
    using Data.Model.Facades;

    [TestClass]
    public class BladderChangeInfoFacadeTest
    {
        [TestMethod]
        public void CreateMachineList()
        {
            var facade = new BladderChangeInfoFacade();
            var machineCount = facade.CreateMachineList();

            Assert.AreEqual(91, machineCount);
        }

        [TestMethod]
        public void GetActiveMachineListTest()
        {
            var facade = new BladderChangeInfoFacade();
            var list = facade.GetActiveMachineList();
            Assert.IsNotNull(list);
            Assert.AreEqual(8, list.Count);
        }

        [TestMethod]
        public void UpdateBladderChangeInfoTest()
        {
            var facade = new BladderChangeInfoFacade();
            var list = facade.GetActiveMachineList();

            var info = list[0];            
            info.IsModified = true;

            int count = facade.UpdateBladderChangeInfo(list);
            Assert.AreEqual(1, count);
        }

        [TestMethod()]
        public void GetLastestBladderChangeInfoTest()
        {
            var facade = new BladderChangeInfoFacade();
            var list = facade.GetActiveMachineList();

            facade.GetLastestBladderChangeInfo(list);

            Assert.AreEqual("MDMVA", list[0].Size);
            Assert.AreEqual(380, list[0].BladderLimitRight);
        }
    }
}

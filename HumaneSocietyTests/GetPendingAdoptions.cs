using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HumaneSociety;

namespace HumaneSocietyTests
{
    [TestClass]
    public class GetPendingAdoptions
    {
        [TestMethod]
        public void TwoIndex_GetPendingAdoptions()
        {
            var adoptionList = Query.GetPendingAdoptions();
            // Assert
            foreach(Adoption a in adoptionList)
            {
                //Assert.IsInstanceOfType();
            }
        }
    }
}

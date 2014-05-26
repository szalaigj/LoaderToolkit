using System;
using System.Collections;
using System.Data;
using System.Data.SqlTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DecodersForSQLServerDB.Test
{
    [TestClass]
    public class UserDefinedFunctionsTest
    {
        [TestMethod]
        public void DetermineInDelTest()
        {
            SqlString indel = new SqlString("30-TCTC	47-T	59+A	74-C	");
            SqlInt64 posStart = new SqlInt64(47152571);
            IEnumerable resultList = UserDefinedFunctions.DetermineInDel(posStart, indel);
            Assert.AreEqual(4, ((ArrayList)resultList).Count);
        }
    }
}

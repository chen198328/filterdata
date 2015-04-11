using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data;
using System.IO;
using ESIPaperFilter.Code;
namespace FilterTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            //IReader ireader = new EIReader();
            //using (StreamReader reader = new StreamReader("ei.txt"))
            //{
            //    string copyright = ireader.ReadCopyRight(reader);
            //    List<Paper> paperlist = ireader.Read(reader);
            //}
            string path = @"C:\Program Files\feiq\Recv Files\SCIE\中国地质大学CPCI-S（1-500）.ciw";
            List<Paper> paperlist = Paper.Read(path);
            Assert.AreEqual(paperlist.Count, 500);

        }
    }
}

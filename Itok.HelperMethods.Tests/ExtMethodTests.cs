using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Itok.HelperMethods.Tests
{
    [TestClass]
    public class ExtMethodTests
    {
        [TestMethod]
        public void TestDeserializeJsonArrayFromStream()
        {
            using (var fs = File.OpenRead("sample.json"))
            {
                var samples = fs.DeserializeFromStream<SampleClass>().ToList();
                foreach (var sample in samples)
                {
                    Console.WriteLine(sample.Category);
                }
            }
        }

        [TestMethod]
        public void TestEnumerableIsNullOrEmpty()
        {
            var list = new List<int>();
            List<string> nullList = null;
            Assert.IsTrue(list.IsNullOrEmpty());
            Assert.IsTrue(nullList.IsNullOrEmpty());
            list.Add(1);
            Assert.IsFalse(list.IsNullOrEmpty());
        }

        [TestMethod]
        public void TestSplitListByCount()
        {
            var list = Enumerable.Range(0, 100).ToList();
            var split = list.SplitListByCount(10);
            Assert.IsTrue(split.All(s => s.Count <= 10));
        }
    }

    public class SampleClass
    {
        public string Type { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public IList<InnerItem> InnerArray { get; set; }
    }

    public class InnerItem
    {
        public int Id { get; set; }
    }
}
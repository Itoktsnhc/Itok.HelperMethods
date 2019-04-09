using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Itok.HelperMethods.Sql;
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

        [TestMethod]
        public void TestGetDeterministicHashCode()
        {
            const string str = "Hello";
            const int hashCodeValue = 60463434;
            Assert.AreEqual(hashCodeValue, str.GetDeterministicHashCode());
        }

        [TestMethod]
        public void TestCreateDataTable()
        {
            var data = Enumerable.Range(0, 1000).Select(i => new Person
            {
                Name = $"Name {i}",
                Age = i,
                Number = i % 2 == 0 ? default(int?) : 1
            });
            var dt = data.CreateDataTable(nameof(Person));
        }

        [TestMethod]
        public void TestMapByPropName()
        {
            var objB = new ClassB()
            {
                Name = "This is A"
            };
            var objA = objB.MapByPropName<ClassB, ClassA>();
            Assert.AreEqual(objB.Name, objA.Name);
        }
    }

    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public int? Number { get; set; }
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

    #region MapByPropTestClass
    public class ClassA
    {
        public string Name { get; set; }
        public int? Age { get; set; }
    }

    public class ClassB
    {
        public string Name { get; set; }
    }
    #endregion
}
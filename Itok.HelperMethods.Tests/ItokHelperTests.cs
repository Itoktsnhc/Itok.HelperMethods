using System;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Itok.HelperMethods.Tests
{
    [TestClass]
    public class ItokHelperTests
    {
        [TestMethod]
        public void TestCompareVersion()
        {
            var res = ItokHelper.CompareVersion("0.1", "0.11");
            Assert.AreEqual(-1, res);
            res = ItokHelper.CompareVersion("1.1.0.1.1", "1.1.0.1.1");
            Assert.AreEqual(0, res);
            res = ItokHelper.CompareVersion("2.5.2", "2.3.2");
            Assert.AreEqual(1, res);
            res = ItokHelper.CompareVersion("2.5", "2.3.2");
            Assert.AreEqual(1, res);
            res = ItokHelper.CompareVersion("2.5.2", "2.3");
            Assert.AreEqual(1, res);
        }

        [TestMethod]
        public void TestGZipStr()
        {
            // ReSharper disable once StringLiteralTypo
            const string str = "asdasdadsadad";
            Assert.AreEqual(str, ItokHelper.DecompressGZipString(ItokHelper.CompressGZipString(str, Encoding.UTF8)));
        }

        [TestMethod]
        public void TestGZipStr2()
        {
            // ReSharper disable once StringLiteralTypo
            const string str = "asdasdadsadad";
            Assert.AreEqual(str,
                ItokHelper.DecompressGZipString(ItokHelper.CompressGZipString(str, Encoding.ASCII), Encoding.ASCII));
        }

        [TestMethod]
        public void TestUnzip()
        {
            var targetFolder = ItokHelper.UnZipFile("sample.zip");
            Assert.IsTrue(File.Exists(Path.Combine(targetFolder, "sample.json")));
            Directory.Delete(targetFolder, true);
        }

        [TestMethod]
        public void TestUnzipNotFound()
        {
            try
            {
                ItokHelper.UnZipFile("asd.zip");
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e, typeof(FileNotFoundException));
            }
        }

        [TestMethod]
        public void TestEnsureDirExist()
        {
            const string dir = "123";
            Directory.CreateDirectory(dir);
            ItokHelper.EnsureDirExist(dir);
            ItokHelper.EnsureDirExist(dir, true);
            Directory.Delete(dir);
        }

        [TestMethod]
        public void TestEnsureDirExist2()
        {
            const string dir = "1234";
            ItokHelper.EnsureDirExist(dir);
            Directory.Delete(dir);
        }

        [TestMethod]
        public void TestGetMd5()
        {
            Assert.AreEqual("900150983CD24FB0D6963F7D28E17F72", ItokHelper.GetMd5("abc"));
        }

        [TestMethod]
        public void TestAllIndexesOf()
        {
            const string subStr = "abc";
            // ReSharper disable once StringLiteralTypo
            const string originalString = "abcdsadavvzxcabc";
            var result = originalString.AllIndexesOf(subStr).ToList();
            Assert.IsTrue(result.Count == 2 && result[0] == 0 && result[1] == 13);
        }

        [TestMethod]
        public void TestAllIndexesOf1()
        {
            try
            {
                const string subStr = null;
                // ReSharper disable once StringLiteralTypo
                const string originalString = "abcdsadavvzxcabc";
                // ReSharper disable once ExpressionIsAlwaysNull
                var list = originalString.AllIndexesOf(subStr).ToList();
                Console.Write(list.Count);
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e, typeof(ArgumentException));
            }
        }

        [TestMethod]
        public void TestAllIndexesOf2()
        {
            try
            {
                const string subStr = "abc";
                // ReSharper disable once StringLiteralTypo
                const string originalString = null;
                // ReSharper disable once ExpressionIsAlwaysNull
                var list = originalString.AllIndexesOf(subStr).ToList();
                Console.Write(list.Count);
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e, typeof(ArgumentException));
            }
        }

        [TestMethod]
        public void TestStringConvertToObj1()
        {
            const string str = "";
            var res = ItokHelper.ConvertToObj<ToDo>(str);
            Assert.AreEqual(null, res);
        }

        [TestMethod]
        public void TestStringConvertToObj2()
        {
            const string str = null;
            var res = ItokHelper.ConvertToObj<ToDo>(str);
            Assert.AreEqual(null, res);
        }

        [TestMethod]
        public void TestStringConvertToObj3()
        {
            var todo = new ToDo("This is Name", "This is Description");

            var str = JsonConvert.SerializeObject(todo);
            var res = ItokHelper.ConvertToObj<ToDo>(str);
            Assert.AreEqual(res, todo);
        }
    }

    public class ToDo
    {
        public ToDo(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public string Name { get; set; }
        public string Description { get; set; }

        public override bool Equals(object obj)
        {
            return obj is ToDo todo && todo.Name == Name && todo.Description == Description;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Description);
        }
    }
}
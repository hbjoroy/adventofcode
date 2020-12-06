using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System;

namespace _1
{
    [TestClass]
    public class UnitTest3
    {
        class SlopeLine
        {
            enum Biome { Open='.', Tree='#'}
            List<Biome> _slopeLine=new List<Biome>();
            public SlopeLine(string raw)
            {
                _slopeLine = (from a in raw.ToArray() select (Biome)a).ToList();
            }

            public bool HasTree(int pos)
            {
                return _slopeLine[pos % _slopeLine.Count()]==Biome.Tree;
            }
        }
        Int64 Trees(int yStep, int xStep)
        {
            return input
                .Where((val,index) => index % yStep==0)
                .Where((test, index) => test.HasTree(index*xStep))
                .Count();
        }
        List<SlopeLine> input=new List<SlopeLine>();

        [TestInitialize]     
        public void Initialize()
        {
            using (var reader=File.OpenText("input3.txt"))
            {
                while (!reader.EndOfStream)
                {
                    input.Add(new SlopeLine(reader.ReadLine()));
                }
            }
        }

        [TestMethod]
        public void TestMethod1()
        {
            var y=Trees(1,3);
            Assert.AreEqual(259,y);
        }

        [TestMethod]
        public void TestMethod2()
        {
            var x=(new [] { (1,1),(3,1),(5,1),(7,1),(1,2)})
                .Select( setup => Trees(setup.Item2, setup.Item1))
                .Aggregate((val1,val2)=>val1*val2);

            Assert.AreEqual(2224913600,x);
        }
        
    }
}

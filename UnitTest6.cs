using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace _1
{
    [TestClass]
    public class UnitTest6
    {
   
        class Declaration
        {
            string _raw;
            List<string> _answers;
            public Declaration(string raw)
            {
                _raw=raw;

                _answers=raw.Split(' ').ToList();
            }

            public uint GroupCount
            {
                get {
                    return (uint)_answers.Aggregate(
                        new List<char>(), 
                        (acc, answer ) => acc.Union(answer.ToList()).ToList()
                    ).Count();
                }
            }
            public uint GroupCountSnitt
            {
                get {
                    return (uint)_answers.Aggregate(
                        _answers[0].ToList(), 
                        (acc, answer ) => acc.Intersect(answer.ToList()).ToList()
                    ).Count();
                }
            }

        }
        public static string ReadOne(TextReader reader)
        {
            var raw="";
            var line="";
            do
            {
                line=reader.ReadLine();
                raw+=$" {line}";
            } while (line!="" && line!=null);

            return raw.Trim();
        }
        List<Declaration> input=new List<Declaration>();

        [TestInitialize]     
        public void Initialize()
        {
            using (var reader=File.OpenText("input6.txt"))
            {
                do 
                {
                    input.Add(new Declaration(ReadOne(reader)));
                } while (!reader.EndOfStream);
            }
        }

        [TestMethod]
        public void Part1()
        { 
            var count=input.Sum( declaration => declaration.GroupCount );
            Assert.AreEqual(6335u, count);
        }

        [TestMethod]
        public void Part2()
        {
            var count=input.Sum( declaration => declaration.GroupCountSnitt );
            Assert.AreEqual(3392u, count);
        }

        [TestMethod]
        public void ReadOne_should_collect()
        {
            using (var r=new StringReader(
                "kcopqbt\ntpkudorbc\npmtwogjb\n\n"+
                "uxkfthszqdbelomwri\ntlusezbomwfqrdxhki\nrsdfkbteoqilzmxhuw\nbwkqmdsxhrfulietoz\nizkwomsdlbfxetqhru\n\n"+
                "kzhwyodlvpt\nkvfhldwpo\nlvhypokdw\nopwjhvdkcl\nkpahtdolvw"
            ))
            {        
                var first=ReadOne(r);
                Assert.AreEqual("kcopqbt tpkudorbc pmtwogjb",first);
                ReadOne(r);
                ReadOne(r);
                ReadOne(r);
            }
        }
        [TestMethod]
        public void GroupCount_should_be_calculated()
        {
            var d=new Declaration("ab ac");
            Assert.AreEqual(3u, d.GroupCount);
        }

        [TestMethod]
        public void GroupCountSnitt_should_be_calculated()
        {
            var d=new Declaration("ab ac");
            Assert.AreEqual(1u, d.GroupCountSnitt);
        }
        
    }
}

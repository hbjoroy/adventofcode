using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System;

namespace _1
{
    [TestClass]
    public class UnitTest4
    {
        class Passport
        {
            public enum Fields { byr, iyr, eyr, hgt, hcl, ecl, pid, cid }
            List<(Fields,string)> _passport=new List<(Fields,string)>();
            string _raw;
            public Passport(string raw)
            {

                _passport=raw.Split(" ").Select( 
                    s => {
                        var f=s.Split(':');
                        return (Enum.Parse<Fields>(f[0]),f[1]);
                    }
                ).ToList();

                _raw=raw;
            }
            public Fields[] required= new [] { 
                Fields.byr, Fields.iyr, Fields.eyr, Fields.hgt, Fields.hcl, Fields.ecl, Fields.pid 
            };
            public static Dictionary<Fields,Func<string, bool>> dataRules = new Dictionary<Fields, Func<string,bool>> {
                { Fields.byr, data => data.Length==4 && int.Parse(data) >= 1920 && int.Parse(data)<= 2002 },
                { Fields.iyr, data => data.Length==4 && int.Parse(data) >= 2010 && int.Parse(data)<= 2020 },
                { Fields.eyr, data => data.Length==4 && int.Parse(data) >= 2020 && int.Parse(data)<= 2030 },
                { Fields.hgt, data => 
                    {
                        if (!new Regex("^\\d{1,}(?:cm|in)$").IsMatch(data))
                            return false;

                        var s=(ReadOnlySpan<char>)data;
                        var num=int.Parse(s.Slice(0,s.Length-2));
                        var unit=s.Slice(s.Length-2);
                        switch (unit.ToString())
                        {
                            case "cm": 
                                return num>=150 && num <=193;                            
                            case "in":
                                return num>=59 && num <=76;
                            default:
                                throw new Exception();
                        }
                    }                     
                },
                { Fields.hcl, data => new Regex("^#(?:[0-9]|[a-f]){6}$").IsMatch(data) },
                { Fields.ecl, data => new Regex("^amb|blu|brn|gry|grn|hzl|oth$").IsMatch(data) },
                { Fields.pid, data => new Regex("^\\d{9}$").IsMatch(data) },
                { Fields.cid, data => true },
            };
            public bool Valid {
                get {
                    var f=_passport.Select(p=>p.Item1).ToList();
                    return required.All( fld => f.Contains(fld) );
                }
            }

            public bool DataValid {
                get {
                    return _passport.All( element => dataRules[element.Item1](element.Item2));
                }
            }

        }
        List<Passport> input=new List<Passport>();

        [TestInitialize]     
        public void Initialize()
        {
            using (var reader=File.OpenText("input4.txt"))
            {
                while (!reader.EndOfStream)
                {
                    var r="";
                    var raw="";
                    do
                    {
                        r=reader.ReadLine();
                        raw=$"{raw} {r}";
                    } while (r!="" && !reader.EndOfStream);

                    input.Add(new Passport(raw.Trim()));
                }
            }
        }

        [TestMethod]
        public void TestMethod1()
        { 
            var count=input.Where(i=>i.Valid).Count();
            Assert.IsTrue(input.Count > count);
            Assert.AreEqual(190, count);
        }

        [TestMethod]
        public void TestMethod2()
        {
            var relevant=input.Where(i=>i.Valid);
            var actual=relevant.Where(i=>i.DataValid);
            var count=actual.Count();
            
            Assert.IsTrue(input.Count > count);
            Assert.AreEqual(190, count);
        }

        [TestMethod]
        public void ByrTest()
        {
            var valid=new [] { "1920", "2002", "1993" };
            var invalid=new [] { "1919", "2003", "2015", "36" };

            Assert.IsTrue(valid.All(value => Passport.dataRules[Passport.Fields.byr](value)));
            Assert.IsFalse(invalid.Any(value => Passport.dataRules[Passport.Fields.byr](value)));
        }
        [TestMethod]
        public void IyrTest()
        {
            var valid=new [] { "2010", "2020", "2015" };
            var invalid=new [] { "2009", "2021", "1980", "36","AA" };

            Assert.IsTrue(valid.All(value => Passport.dataRules[Passport.Fields.iyr](value)));
            Assert.IsFalse(invalid.Any(value => Passport.dataRules[Passport.Fields.iyr](value)));
        }
        [TestMethod]
        public void EyrTest()
        {
            var valid=new [] { "2020", "2030", "2025" };
            var invalid=new [] { "2019", "2031", "1980", "36","AA" };

            Assert.IsTrue(valid.All(value => Passport.dataRules[Passport.Fields.eyr](value)));
            Assert.IsFalse(invalid.Any(value => Passport.dataRules[Passport.Fields.eyr](value)));
        }
        [TestMethod]
        public void HgtTest()
        {
            var valid=new [] { "150cm", "193cm", "170cm", "59in", "76in", "65in" };
            var invalid=new [] { "149cm", "194cm", "130cm", "58in","77in", "79in", "aa", "173" };

            Assert.IsTrue(valid.All(value => Passport.dataRules[Passport.Fields.hgt](value)));
            Assert.IsFalse(invalid.Any(value => Passport.dataRules[Passport.Fields.hgt](value)));
        }
        [TestMethod]
        public void HclTest()
        {
            var valid=new [] { "#123456", "#abcdef", "#a1b2c3", "#1a2a3a" };
            var invalid=new [] { "#abcdefg","#1234ag", "123456", "abcdef", "invalid","#12345", "#aa", "173" };

            Assert.IsTrue(valid.All(value => Passport.dataRules[Passport.Fields.hcl](value)));
            Assert.IsFalse(invalid.Any(value => Passport.dataRules[Passport.Fields.hcl](value)));            
        }
        
        
    }
}

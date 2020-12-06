using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System;

namespace _1
{
    [TestClass]
    public class UnitTest2
    {
        class PasswordWithPolicy
        {
            public int min;
            public int max;
            public char c;
            public string Password;

            public PasswordWithPolicy(string raw)
            {
                var lineSplit=raw.Split(": ");
                var policySplit=lineSplit[0].Split(' ');
                var limitSplit=policySplit[0].Split('-');

                min=int.Parse(limitSplit[0]);
                max=int.Parse(limitSplit[1]);
                c=policySplit[1][0];
                Password=lineSplit[1];
            }

            public bool Valid 
            {
                get {
                    var count=Password.ToArray().Where(a => a==c ).Count();
                    return min<=count && count<=max;
                }
            }
            public bool ValidV2 
            {
                get {
                    var arr=Password.ToArray();
                    var c1=arr[min-1];
                    var c2=arr[max-1];

                    return (c1==c || c2==c) && (c1!=c2);
                }
            }
        }
        List<PasswordWithPolicy> input=new List<PasswordWithPolicy>();

        [TestInitialize]     
        public void Initialize()
        {
            using (var reader=File.OpenText("input2.txt"))
            {
                while (!reader.EndOfStream)
                {
                    input.Add(new PasswordWithPolicy(reader.ReadLine()));
                }
            }
        }

        [TestMethod]
        public void TestMethod1()
        {

            var x=input.Where( test => test.Valid).Count();
            var y=x;
        }

        [TestMethod]
        public void TestMethod2()
        {

            var x=input.Where( test => test.ValidV2).Count();
            var y=x;
        }
    }
}

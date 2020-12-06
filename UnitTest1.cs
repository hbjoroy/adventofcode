using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System;

namespace _1
{
    [TestClass]
    public class UnitTest1
    {
        List<int> inputNumbers=new List<int>();

        [TestInitialize]     
        public void Initialize()
        {
            using (var reader=File.OpenText("input.txt"))
            {
                while (!reader.EndOfStream)
                {
                    inputNumbers.Add(int.Parse(reader.ReadLine()));
                }
            }
        }

        [TestMethod]
        public void TestMethod1()
        {
            (int,int) theValues=(0,0);

            for (int i=0;i<inputNumbers.Count-1;i++)
            {
                var num1=inputNumbers[i];
                var rest=inputNumbers.Skip(i+1);
                foreach (var num2 in rest)
                {
                    if (num1+num2==2020)
                    {
                        theValues=(num1,num2);
                    }
                }
            }
            var mul=theValues.Item1*theValues.Item2;

            Console.WriteLine($"Val1: {theValues.Item1} Val2: {theValues.Item2} Multiplied: {mul}");
        }
        [TestMethod]
        public void TestMethod2()
        {
            (int,int,int) theValues=(0,0,0);

            for (int i=0;i<inputNumbers.Count-2;i++)
            {
                var num1=inputNumbers[i];
                var rest1=inputNumbers.Skip(i+1).ToList();

                for (int j=0;j<rest1.Count-1;j++)
                {
                    var num2=rest1[j];
                    var rest2=rest1.Skip(j+1);
                    foreach (var num3 in rest2)
                    {
                        if (num1+num2+num3==2020)
                        {
                            theValues=(num1,num2,num3);                            
                        }
                    }
                }
            }
            var mul=theValues.Item1*theValues.Item2*theValues.Item3;
        }
    }
}

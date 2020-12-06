using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace _1
{
    public static class UintExtensions {
        public static uint ParseBinary(this string str)
        {
            var r=0u;
            var m=new Regex("^0b((?:0|1){1,})$").Match(str);
            if (m.Groups.Count!=2)
            {
                throw new FormatException("Data not binary - only 0 and 1, starting with 0b");
            }

            foreach (var c in m.Groups[1].Captures[0].ToString())
            {
                r<<=1;
                r+=uint.Parse(c.ToString());
            }
            return r;
        }
    }     
    [TestClass]
    public class UnitTest5
    {
   
        class Ticket
        {
            string _raw;
            uint _seatId;
            public Ticket(string raw)
            {
                _raw=raw.ToString();
                _seatId=Parse(raw);
            }
            public (uint,uint) Seat { get { return (_seatId/8, _seatId%8); }}
            public uint SeatId { get { return _seatId; }}

            public static uint Parse(string raw)
            {
                var proc=$"0b{raw.Replace('R','1').Replace('L','0').Replace('B','1').Replace('F','0')}";
                return proc.ParseBinary();
            }

        }
        List<Ticket> input=new List<Ticket>();

        [TestInitialize]     
        public void Initialize()
        {
            using (var reader=File.OpenText("input5.txt"))
            {
                while (!reader.EndOfStream)
                {
                    input.Add(new Ticket(reader.ReadLine()));
                }
            }
        }

        [TestMethod]
        public void TestMethod1()
        { 
            var r=input.Select( ticket => ticket.SeatId ).Max();
            Assert.AreEqual(938u,r);
        }

        [TestMethod]
        public void TestMethod2()
        {
            var ordered=input.OrderBy(ticket => ticket.SeatId).ToArray();
            var mySeat=0u;
            for (int i=0;i<input.Count()-1;i++)
            {
                if (ordered[i].SeatId!=(ordered[i+1].SeatId-1))
                {
                    mySeat=ordered[i].SeatId+1;
                    break;
                }
            }

            Assert.AreEqual(696u,mySeat);
        }

        [TestMethod]
        public void TestBinaryParser()
        {
            var num="0b1100001".ParseBinary();
            Assert.ThrowsException<FormatException>(() => "0101".ParseBinary());
            Assert.AreEqual(97u,num);
        }
        [TestMethod]
        public void TestRowParse()
        {
            var r=Ticket.Parse("BBFFFFBLRL");
            Assert.AreEqual(778u,r);
        }
        [TestMethod]
        public void TestTicketParse()
        {
            var (row, col)=new Ticket("BBFFFFBLRL").Seat;
            Assert.AreEqual(97u, row);
            Assert.AreEqual(2u, col);
        }
        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace task4
{
    class Program
    {
        static void Main(string[] args)
        {
            String filename = args[0];
            List<Visitor> Visitors = new List<Visitor>();
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(fs);
            while (!reader.EndOfStream) {
                String time = reader.ReadLine();
                String[] parse = time.Split(' ', ':');
                Decimal hours = Convert.ToDecimal(parse[0]);
                Decimal minutes = Convert.ToDecimal(parse[1]);
                Decimal _in = hours + (minutes * (Decimal)0.01);
                hours = Convert.ToDecimal(parse[2]);
                minutes = Convert.ToDecimal(parse[3]);
                Decimal _out = hours+(minutes * (Decimal)0.01);
                Visitors.Add(new Visitor(_in, _out));
            }
            Visitors.Sort((a, b) => a.IN.CompareTo(b.IN));
            List<Visitor> Bank = new List<Visitor>();
            Int32 qty = 0, index=0, counter=1, N=Visitors.Count;
            Bank.Add(new Visitor(Visitors[0].IN, Visitors[0].OUT, ++qty));
            while (counter < N) {
                if (Visitors[counter].IN < Bank[index].OUT) {
                    Bank[index].IN = Visitors[counter].IN;
                    if (Bank[index].OUT > Visitors[counter].OUT)
                        Bank[index].OUT = Visitors[counter].OUT;
                    Bank[index].Qty = ++qty;
                }
                else if (Visitors[counter].IN > Bank[index].OUT) {
                    qty = 0;
                    for (Int32 i = 0; i < counter; i++) {
                        if (Visitors[i].OUT > Visitors[counter].IN)
                            ++qty;
                    }
                    if (qty > 0)
                    {
                        Bank.Add(new Visitor(Visitors[counter].IN, Visitors[counter].OUT, ++qty));
                        index++;
                        for (Int32 i = 0; i < counter; i++)
                        {
                            if (Bank[index].IN < Visitors[i].OUT)
                            {
                                if (Bank[index].OUT > Visitors[i].OUT)
                                    Bank[index].OUT = Visitors[counter].OUT;
                            }
                        }
                    }
                    else {
                        index++;
                        Bank.Add(new Visitor(Visitors[counter].IN, Visitors[counter].OUT, ++qty));
                    }
                }
                else {
                    for (Int32 i = 0; i < counter; i++) {
                        if (Visitors[i].OUT > Visitors[counter].IN) {
                            if (Visitors[i].OUT < Visitors[counter].OUT)
                                Bank[index].OUT = Visitors[i].OUT;
                            else
                                Bank[index].OUT = Visitors[counter].OUT;
                        }
                    }
                }
                counter++;
            }
            ShowResult(Bank);
            Console.Read();
        }
        static void ShowResult(List<Visitor> Intervals) {
            List<Visitor> Output = new List<Visitor>();
            Int32 MaxValue = Intervals.Max(a => a.Qty);
            for (Int32 i = 0; i < Intervals.Count; i++) 
                if (Intervals[i].Qty == MaxValue) 
                    Output.Add(Intervals[i]);

            List<String> Out = new List<String>();
            foreach (Visitor visitor in Output)
                Out.Add(Convertion(visitor));
            foreach (String interval in Out)
                Console.WriteLine(interval);
  
        }
        static String Convertion(Visitor v) {
            String hours, minutes, result;
            Int32 h, m;
            h = (Int32)v.IN; m = (Int32)((v.IN - h)*100);
            hours = h.ToString();
            minutes = (m < 10) ? "0" + m.ToString() : m.ToString();
            result = hours + ":" + minutes;

            h = (Int32)v.OUT; m = (Int32)((v.OUT - h) * 100);
            hours = h.ToString();
            minutes = (m < 10) ? "0" + m.ToString() : m.ToString();
            result = result + " " + hours + ":" + minutes;

            return result;
        }
    }
    class Visitor {
        public Decimal IN { get; set; }
        public Decimal OUT { get; set; }
        public Int32 Qty { get; set; }
        public Visitor(Decimal _in, Decimal _out) {
            IN = _in; OUT = _out;
        }
        public Visitor(Decimal _in, Decimal _out, Int32 _qty) {
            IN = _in; OUT = _out; Qty = _qty;
        }
    }
}

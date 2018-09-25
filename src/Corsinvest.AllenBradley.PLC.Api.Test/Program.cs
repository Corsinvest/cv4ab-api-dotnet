using System;
using System.Collections.Generic;
using Corsinvest.AllenBradley.PLC.Api;

namespace Corsinvest.AllenBradley.Test
{
    [Serializable]
    public class Test12
    {
        public int AA1 { get; set; }
        public int AA2 { get; set; }
        public int AA3 { get; set; }
        public int AA4 { get; set; }
        public int AA5 { get; set; }
        public int AA6 { get; set; }
        public int AA7 { get; set; }
        public int AA8 { get; set; }
    }

    public class Program
    {


        private static void PrintChange(string @event, ResultOperation result)
        {
            Console.Out.WriteLine($"{@event} {result.Timestamp} Changed: {result.Tag.Name} {result.StatusCode}");
        }

        static void TagChanged(ResultOperation result)
        {
            PrintChange("TagChanged", result);
        }
        static void GroupChanged(IEnumerable<ResultOperation> results)
        {
            foreach (var result in results) PrintChange("GroupTagChanged", result);
        }

        public static void Main(string[] args)
        {
            using (var controller = new Controller("10.155.128.192", "1, 0", CPUType.LGX))
            {
                Console.Out.WriteLine("Ping " + controller.Ping(true));
                var grp = controller.CreateGroup();

                var tagBPLC1 = grp.CreateTagInt8("TKP_PLC_B_P12");
                tagBPLC1.Read();

                var tagBPC1 = grp.CreateTagInt8("TKP_PC_B_P1");
                var tagBarcode = grp.CreateTagString("TKP_PLC_S_P1");

                var tag3 = grp.CreateTagArray<float[]>("OvenTemp", 36);

                //var tag_1 = grp.CreateTagArray<string[]>("Track", 300);

                //or
                var tag = grp.CreateTagType<string[]>("Track", TagSize.STRING, 300);
                tag.Changed += TagChanged;
                var aa = tag.Read();

                var tag1 = grp.CreateTagType<Test12>("Test");
                tag.Changed += TagChanged;

                var tag2 = grp.CreateTagFloat32("Fl32");


                grp.Changed += GroupChanged;
                grp.Read();

            }
        }
    }
}
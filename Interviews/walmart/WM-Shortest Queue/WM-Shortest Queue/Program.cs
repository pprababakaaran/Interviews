using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WM_Shortest_Queue
{
    class Program
    {
        static void Main(string[] args)
        {
            List<MyDataClass> items = new List<MyDataClass>();
            List<MyTravelClass> items_weight = new List<MyTravelClass>();
            //Scenario#1
            //items.Add(new MyDataClass { START_POINT = 1, END_POINT = 5, PPL_IN_QUEUE = 3 });
            //items.Add(new MyDataClass { START_POINT = 2, END_POINT = 5, PPL_IN_QUEUE = 8 });
            //items.Add(new MyDataClass { START_POINT = 7, END_POINT = 1, PPL_IN_QUEUE = 3 });
            
            //Scenario#2
            //items.Add(new MyDataClass { START_POINT = 1, END_POINT = 5, PPL_IN_QUEUE = 3 });
            
            //Scenario#3
            items.Add(new MyDataClass { START_POINT = 1, END_POINT = 5, PPL_IN_QUEUE = 3 });
            items.Add(new MyDataClass { START_POINT = 2, END_POINT = 5, PPL_IN_QUEUE = 8 });
            
            //Scenario#4
            //items.Add(new MyDataClass { START_POINT = 1, END_POINT = 5, PPL_IN_QUEUE = 9 });
            //items.Add(new MyDataClass { START_POINT = 2, END_POINT = 5, PPL_IN_QUEUE = 5 });
            //items.Add(new MyDataClass { START_POINT = 3, END_POINT = 6, PPL_IN_QUEUE = 1 });
            //items.Add(new MyDataClass { START_POINT = 4, END_POINT = 6, PPL_IN_QUEUE = 3 });
            //items.Add(new MyDataClass { START_POINT = 5, END_POINT = 7, PPL_IN_QUEUE = 7 });
            //items.Add(new MyDataClass { START_POINT = 6, END_POINT = 7, PPL_IN_QUEUE = 4 });

            int root = items.Select(s => s.END_POINT).Distinct().Except(items.Select(sq => sq.START_POINT).Distinct()).FirstOrDefault();
            int[] entries = items.Select(sp => sp.START_POINT).Distinct().Except(items.Select(ep => ep.END_POINT).Distinct()).ToArray();
            
            foreach (var entry in entries)
            {
                int t_waiting = 1;                
                string s_travel_path = string.Empty;
                var travel_node = items.Where(i => i.START_POINT == entry).FirstOrDefault();
                s_travel_path = travel_node.START_POINT.ToString();
                do
                {
                    int v_other_node_weightage;
                    t_waiting = t_waiting + travel_node.PPL_IN_QUEUE;
                    int v_start_point = travel_node.START_POINT;

                    s_travel_path = s_travel_path + " -> " + travel_node.END_POINT;
                    v_other_node_weightage = other_Node_Weight_Calculation(ref items, v_start_point, travel_node.END_POINT);
                    if (v_other_node_weightage >= t_waiting)
                    {
                        t_waiting = t_waiting * 2;
                    }
                    else
                    {
                        t_waiting = t_waiting + v_other_node_weightage;
                    }
                    if (travel_node.END_POINT == root)
                        break;

                    travel_node = items.Where(i => i.START_POINT == travel_node.END_POINT).FirstOrDefault();

                } while (true);
                items_weight.Add(new MyTravelClass { TIME_UNIT = t_waiting, TRAVEL_PATH = s_travel_path });
            }
            
            int opt_path = items_weight.Select(co => co.TIME_UNIT).Min();
            foreach (var path in items_weight.Where(fil => fil.TIME_UNIT == opt_path))
            {
                Console.WriteLine("The optical path is {0} and time unit to reach counter {1}", path.TRAVEL_PATH, path.TIME_UNIT - 1);
            }
            Console.ReadKey();
        }
        public static int other_Node_Weight_Calculation(ref List<MyDataClass> itemsref, int v_start_point, int v_end_point)
        {
            int temp_queue_count=0;
            var travelPath = itemsref.Where(i => i.END_POINT == v_end_point && i.START_POINT != v_start_point);
            while (travelPath.Count() > 0)
            {
                temp_queue_count = temp_queue_count + travelPath.Select(c => c.PPL_IN_QUEUE).Sum();
                int[] nodes = travelPath.Select(i => i.START_POINT).ToArray();
                travelPath = itemsref.Where(i => nodes.Contains(i.END_POINT));
            }
            return temp_queue_count;

        }        
    }
    public class MyDataClass
    {
        public int START_POINT { get; set; }
        public int END_POINT { get; set; }
        public int PPL_IN_QUEUE { get; set; }
       
    }
    public class MyTravelClass
    {
        public int TIME_UNIT { get; set; }
        public string TRAVEL_PATH { get; set; }
    }
}
    
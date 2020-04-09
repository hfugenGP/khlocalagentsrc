using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillsExport.Utils
{
    class Excerpt
    {
        public static List<T> extract<T>(List<T> list, int? start, int? end)
        {
            List<T> slicedList = new List<T>();
            int startIndex = start ?? 0;
            int endIndex = end ?? list.Count;

            startIndex = (startIndex < 0) ? list.Count + startIndex : startIndex;
            endIndex = (endIndex < 0) ? list.Count + endIndex : endIndex;

            startIndex = Math.Max(startIndex, 0);
            endIndex = Math.Min(endIndex, (list.Count > end ? list.Count - 1 : list.Count));

            if (list.Count == 1)
            {
                endIndex = 1;
            }

            if (list == null)
            {
                throw new ArgumentException("list");
            }

            if (list.Count == 0)
            {
                return slicedList;
            }

            for (int i = startIndex; i < endIndex; i++)
            {
                slicedList.Add(list[i]);
            }

            return slicedList;
        }
    }
}

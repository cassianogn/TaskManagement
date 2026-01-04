using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Application.TaskItems.Queries
{
    public class GetTaskItemsQuery
    {
        public GetTaskItemsQuery(){}
        public GetTaskItemsQuery(string? searchKey)
        {
            SearchKey = searchKey;
        }
        public string? SearchKey { get; set; }
    }
}

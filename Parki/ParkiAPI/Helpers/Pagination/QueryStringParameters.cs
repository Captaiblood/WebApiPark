using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkiAPI.Helpers.Pagination
{
    class QueryStringParameters
    {
        private readonly int maxRecordPerPage = 50;
        public int PageNumber { get; set; } = 1; //Default first page
        private int recordsPerPage = 10; //Default number of record per page
        public int RecordsPerPage
        {
            get
            {
                return recordsPerPage;
            }

            set
            {
                recordsPerPage = (value > maxRecordPerPage) ? maxRecordPerPage : value; //if user request for records per page is more than maximum value return return max value
            }

        }
    }
}

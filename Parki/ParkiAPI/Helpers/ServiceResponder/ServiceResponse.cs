using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Library.ServiceResponder
{
         /// <summary>
        /// Generic raper around web api serivice responce.
        /// to help front end to revieve service response.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class ServiceResponse<T>
        {

            public T Data { get; set; }
            public bool Success { get; set; } = true;
            public string Message { get; set; } = null;
            public string Error { get; set; } = null;

    }
   
}

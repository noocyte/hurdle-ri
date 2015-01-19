using System.Net;
using Microsoft.WindowsAzure.Storage.Table;

namespace WebAPI.Backend.Models
{
    public class TableResponse<T> where T : TableEntity
    {
        private readonly TableResult _result;

        public TableResponse(TableResult result)
        {
            _result = result;
        }



        public bool Success { get { return _result.HttpStatusCode < 300; }} // 304?
        public HttpStatusCode Status { get { return (HttpStatusCode)_result.HttpStatusCode; } }
        public T Result
        {
            get
            {
                if (Success)
                    return _result.Result as T;
                return default(T);
            }
        }
    }
}
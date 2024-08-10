using System.Net;

namespace Barber.UI.Entities.Responses
{
    public class ObjectResponse<T> where T : class
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        public List<T> Objects { get; set; }
        public T OneObject { get; set; }

        public ObjectResponse(HttpStatusCode statusCode, string message, List<T> objects, T oneObject)
        {
            StatusCode = statusCode;
            Message = message;
            Objects = objects;
            OneObject = oneObject;
        }

        public ObjectResponse(HttpStatusCode statusCode, string message, List<T> objects)
        {
            StatusCode = statusCode;
            Message = message;
            Objects = objects;
        }
        public ObjectResponse(HttpStatusCode statusCode, string message, T oneObject)
        {
            StatusCode = statusCode;
            Message = message;
            OneObject = oneObject;
        }
        public ObjectResponse()
        {

        }
    }
}

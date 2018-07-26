using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace orderservice.Repository
{
    public static class DummyRepo
    {
        public static void UpdateOrder(bool throwError)
        {
            string dummy = "Mocking repo call";
            if (throwError)
                throw new RepositoryException("Dummmy Repo called failed");
        }
    }

    public class RepositoryException : Exception
    {
        public RepositoryException() : base()
        {

        }

        public RepositoryException(string msg) : base(msg)
        {

        }

        public RepositoryException(string msg, Exception ex) : base(msg, ex)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Core.Exceptions
{
    public  class NotFoundException :Exception
    {
        public NotFoundException(string message) : base(message) { } //default constructor + chain to the base

        public NotFoundException(string name, object key)  //parametrized constructor +  chain to the base 
            : base($"Entity \"{name}\" ({key}) was not found.") { }
    }
}

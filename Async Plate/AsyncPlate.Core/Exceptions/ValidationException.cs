using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Core.Exceptions
{
    public class ValidationException : Exception
    {
        //all have a message field 
        //i need a dictionary for errors to make the output be  :
        //"Errors": {
        //"Email": ["Email is required.", "Invalid email format."],
        //"Password": ["Password must be at least 8 characters."]
        // }
        public IDictionary<string, string[]> Errors { get; }  //why public ? will be used outside 

        public ValidationException()
            : base("One or more validation failures have occurred.") 
        {
            Errors = new Dictionary<string, string[]>(); //initialization for the dictionary
        }

        public ValidationException(IDictionary<string, string[]> errors) : this()
        {
            //it chains first
            //then  assignment 
            Errors = errors; // Fills the list with real data
        }
    }
}

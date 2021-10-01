using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VisitorRegistrationApp.Helper
{
    public static class Guard 
    {
        //Validations maken voor null references, stringlength


        public static void AgainstNull(object property, string argumentName, object Model)
        {
            Type typeOfModel = Model.GetType();
            if (property == null)
            {
                throw new ArgumentNullException($"For the model of {typeOfModel.Name}, you need to give in a value for {argumentName}  ");

            }
          
        }

        //public static void  AgainstNullOrWhiteSpace(object argument, string argumentName, object Model)
        //{
        //    Type typeOfModel = Model.GetType();
        //    if(argument == null || string.IsNullOrWhiteSpace(argument.ToString()) )
        //    {
        //        throw new wh
        //    }
        //}

        //public static void AgainstInvalidTerms(Term term, string argumentName)
        //{
        //    // note: currently there are only two enum options
            
        //}

        public static void AgainstOutOfRange(int range, int length, string argumentName, object Model)
        {
            Type typeOfModel = Model.GetType();
            if (length < range && length > range)
            {
                throw new ArgumentOutOfRangeException($"For the model of {typeOfModel.Name},,Invalid range for the propery {argumentName}. The value needs to be between 0 and {range }");
            }

        }


       

    }



}

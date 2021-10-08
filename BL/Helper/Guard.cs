using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VisitorRegistrationApp.Helper
{
    public static class Guard 
    {

       
        public static string AgainstNull(object property, string argumentName)
        {
            
            if (property == null)
            {
               return $"you need to give in a value for {argumentName}  ";

            }
            return null;
        }

        public static string AgainstNullOrWhiteSpace(object argument, string argumentName)
        {
           
            if (argument == null || string.IsNullOrWhiteSpace(argument.ToString()))
            {
                return $"you need to give in a value for {argumentName}  ";
            }
            return null;
        }

  
        public static void AgainstOutOfRange(int range, int length, string argumentName)
        {
     
            if (length < range && length > range)
            {
                throw new ArgumentOutOfRangeException($"Invalid range for the propery {argumentName}. The value needs to be between 0 and {range }");
            }

        }


        public static IList<string> AgainstErrors(ValidationResult validationResult)
        {
            List<string> Errors = new List<string>(); 

            if (!validationResult.IsValid)
            {
                foreach (var failure in validationResult.Errors)
                {
                    Errors.Add("Property " + failure.PropertyName + " failed validation. Error was: " + failure.ErrorMessage);
                    //struct terug geven met errors en 


                }
                return Errors;
            }

            return Errors;
        }


        public static string AllLoggingErrors(IList<string> errors)
        {
            string errorSummary = string.Empty; 
            foreach(var error in errors)
            {
                errorSummary += error;
            }

            return errorSummary;
        }
       

    }



}

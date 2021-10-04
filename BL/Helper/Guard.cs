using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VisitorRegistrationApp.Helper
{
    public static class Guard 
    {

       
        public static string AgainstNull(object property, string argumentName, object Model)
        {
            Type typeOfModel = Model.GetType();
            if (property == null)
            {
               return $"For the model of {typeOfModel.Name}, you need to give in a value for {argumentName}  ";

            }
            return null;
        }

        public static string AgainstNullOrWhiteSpace(object argument, string argumentName, object Model)
        {
            Type typeOfModel = Model.GetType();
            if (argument == null || string.IsNullOrWhiteSpace(argument.ToString()))
            {
                return $"For the model of {typeOfModel.Name}, you need to give in a value for {argumentName}  ";
            }
            return null;
        }

  
        public static void AgainstOutOfRange(int range, int length, string argumentName, object Model)
        {
            Type typeOfModel = Model.GetType();
            if (length < range && length > range)
            {
                throw new ArgumentOutOfRangeException($"For the model of {typeOfModel.Name},,Invalid range for the propery {argumentName}. The value needs to be between 0 and {range }");
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

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace SimpleBusinessApp.Wrapper
{
    /// <summary>
    /// Base class for Client Wrapper.
    /// </summary>
    public class ModelWrapper<T> : NotifyDataErrorInfoBase
    {
        public ModelWrapper(T model)
        {
            Model = model;
        }

        public T Model { get; }

        /// <summary>
        /// This method gives us possibility to load property
        /// by calling single method (using reflection), instead of calling it directly like: 
        /// get {return Model.FirstName;} we call it get {GetValue &lt;string&gt;()}
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected virtual TValue GetValue<TValue>([CallerMemberName]string propertyName = null) // = null means this parametr is optional, also we changed T->TValue otherwise it would collide with T parametr of the class
        {
            return (TValue)typeof(T).GetProperty(propertyName).GetValue(Model);
        }
        
        /// <summary>
        /// This method gives us possibility to set property by calling single method (using reflection)
        /// instead of calling it like: 
        /// set {  Model.FirstName = value;
        ///        OnPropertyChanged(); } --> set {  SetValue(value); }
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        protected virtual void SetValue<TValue>(TValue value,
            [CallerMemberName]string propertyName = null) // here we need additional parameter
        {
            typeof(T).GetProperty(propertyName).SetValue(Model, value);
            OnPropertyChanged(propertyName);
            ValidatePropertyInternal(propertyName, value);
        }


        /// <summary>
        /// This method is called every time when set method is called.
        /// </summary>
        /// <param name="propertyName"></param>
        private void ValidatePropertyInternal(string propertyName, object currentValue)
        {
            ClearErrors(propertyName);

            // 1. Validate Data Annotations
            ValidateDataAnnotations(propertyName, currentValue);

            // 2. Validate Cusotm Errors
            ValidateCustomErrors(propertyName);
        }

        private void ValidateDataAnnotations(string propertyName, object currentValue)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(Model) { MemberName = propertyName };
            Validator.TryValidateProperty(currentValue, context, results);

            foreach (var result in results)
            {
                AddError(propertyName, result.ErrorMessage);
            }
        }

        private void ValidateCustomErrors(string propertyName)
        {
            var errors = ValidateProperty(propertyName);
            if (errors != null)
            {
                foreach (var error in errors)
                {
                    AddError(propertyName, error);
                }
            }
        }

        /// <summary>
        /// Client wrapper could ovveride this method
        /// to return errors for specific properties
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected virtual IEnumerable<string> ValidateProperty(string propertyName)
        {
            return null;
        }
    }
}

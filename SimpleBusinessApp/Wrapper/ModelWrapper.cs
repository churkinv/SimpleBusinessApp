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
        /// This method gives us possibility to load property by calling single method (using reflection)
        /// instead of calling it directly like:  get { return Model.FirstName; } -->  get { return  GetValue<string>(); }
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected virtual TValue GetValue<TValue>([CallerMemberName]string propertyName = null) // = null means this parametr is optional, also we changed T->TValue otherwise it would collide with T parametr of class
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
        }
    }
}

// ReSharper disable InconsistentNaming
namespace TestProj.Interfaces
{
    using System;
    using System.Data.Common;
    using System.IO;

    
    public interface ITest1 {}

    public interface ITestIn<in TData>
    {
        void TestData<T>(TData data, T f, Func<T, TData> fun) where T: new();
        T TestData2<T>(T[] arr) where T: struct;
    }
    
    interface IPrivateInt{}
    public interface ITestOut<out TData> where TData: ITestIn<TData> {}
    public interface public_interface: ITestIn<int>
    {
        void Do();
        Stream Run(Stream input);
    }

    public class PublicInterface : public_interface
    {
        private static DataAdapter private_static = null;

        public const string test_const = "asd";
        
        public int TestProp { get; protected set; }
        
        public int data_custom()
        {
            return 0;
        }
        
        public void Do()
        {
            throw new System.NotImplementedException();
        }

        public Stream Run(Stream input)
        {
            throw new System.NotImplementedException();
        }

        public void TestData<T>(int data, T f, Func<T, int> fun) where T : new()
        {
            throw new NotImplementedException();
        }

        public T TestData2<T>(T[] arr) where T : struct
        {
            throw new NotImplementedException();
        }
    }
}
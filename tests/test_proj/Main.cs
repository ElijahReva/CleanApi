// ReSharper disable InconsistentNaming
namespace TestProj
{
    using System.Data.Common;
    using System.IO;

    public class public_class
    {
        public class public_class_inner
        {
            public string public_both { get; set; }
            public void test(){}
        }
        
        public string public_withBody
        {
            get => throw new System.NotImplementedException();
            internal set => throw new System.NotImplementedException();
        }

        public string public_both { get; set; }
        public string public_get { get; internal set; }
        public string public_set { private get; set; }
    }

    public interface public_interface
    {
        void Do();
        Stream Run(Stream input);
    }

    public class PublicInterface : public_interface
    {
        private static DataAdapter private_static = null;

        public const string test_const = "asd";
        
        public void Do()
        {
            throw new System.NotImplementedException();
        }

        public Stream Run(Stream input)
        {
            throw new System.NotImplementedException();
        }
    }
}
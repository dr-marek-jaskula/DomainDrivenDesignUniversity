namespace Shopway.Tests.Integration.Constants;

public static partial class Constants
{
    public static class Product
    {
        public static class Name
        {
            public const string FirstTestProduct = "firstTestProduct";
            public const string SecondTestProduct = "secondTestProduct";
            public const string ThirdTestProduct = "thirdTestProduct";
        }

        public static class Price
        {
            public const decimal Cheap = 10m;
            public const decimal Balanced = 50m;
            public const decimal Expensive = 100m;
        }

        public static class UomCode
        {
            public const string Kg = "kg";
            public const string Pcs = "pcs";
        }

        public static class Revision
        {
            public const string One = "1.0";
            public const string Two = "2.0";
            public const string Three = "3.0";
        }
    }
}

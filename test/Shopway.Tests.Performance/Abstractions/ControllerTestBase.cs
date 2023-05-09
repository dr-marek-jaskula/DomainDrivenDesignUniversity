using Shopway.Tests.Performance.Persistance;

namespace Shopway.Tests.Performance.Abstractions;

public abstract class ControllerTestsBase
{
    protected readonly DatabaseFixture fixture;
    protected const string ShopwayApiUrl = "https://localhost:7236/api";

    public ControllerTestsBase()
    {
        fixture = new DatabaseFixture();
    }
}
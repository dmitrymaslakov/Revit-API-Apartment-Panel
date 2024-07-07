using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ApartmentPanel.UseCases.ElectricalFamily.Queries.GetElectricalFamily
{
    public class TestRequestHandler
        : IRequestHandler<TestRequest, int>
    {
        public async Task<int> Handle(TestRequest request, CancellationToken cancellationToken)
        {
            await Task.Delay(0);
            return 1;
        }
    }
}

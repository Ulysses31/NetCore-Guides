using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Toolchains;
using MultipleTasksAsync.Repository;

namespace ApiBenchmarkDotNet.BenchMarks
{
    [MemoryDiagnoser]
    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class MultipleApiExecutorBenchmark
    {
        private readonly Executor _executor = new(new EmployeeApiFacade());
        private readonly Guid _id = new Guid("7119e779-3054-493c-8cf7-c617b4aa0f4e");

        [Benchmark(Baseline = true)]
        public async Task ExecuteInSequence()
        {
            await _executor.ExecuteInSequence(_id);
        }

        [Benchmark]
        public async Task ExecuteInParallel()
        {
            await _executor.ExecuteInParallel(_id);
        }
    }
}

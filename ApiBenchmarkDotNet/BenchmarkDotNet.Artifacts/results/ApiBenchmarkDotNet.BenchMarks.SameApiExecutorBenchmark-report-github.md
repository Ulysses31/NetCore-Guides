``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22621
11th Gen Intel Core i7-1165G7 2.80GHz, 1 CPU, 8 logical and 4 physical cores
.NET SDK=7.0.402
  [Host]     : .NET 7.0.12 (7.0.1223.47720), X64 RyuJIT
  DefaultJob : .NET 7.0.12 (7.0.1223.47720), X64 RyuJIT


```
|                           Method |     Mean |    Error |   StdDev |   Median | Ratio | RatioSD | Rank | Allocated |
|--------------------------------- |---------:|---------:|---------:|---------:|------:|--------:|-----:|----------:|
| ExecuteUsingParallelForeachAsync | 30.77 ms | 0.610 ms | 0.932 ms | 31.09 ms |  0.36 |    0.04 |    1 |     88 KB |
|      ExecuteUsingParallelForeach | 31.01 ms | 0.334 ms | 0.296 ms | 31.03 ms |  0.37 |    0.04 |    1 |     81 KB |
|        ExecuteUsingNormalForEach | 88.79 ms | 2.296 ms | 6.769 ms | 91.13 ms |  1.00 |    0.00 |    2 |     91 KB |

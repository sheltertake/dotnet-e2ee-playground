``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19042
Intel Core i7-6600U CPU 2.60GHz (Skylake), 1 CPU, 4 logical and 2 physical cores
.NET Core SDK=5.0.202
  [Host]   : .NET Core 5.0.5 (CoreCLR 5.0.521.16609, CoreFX 5.0.521.16609), X64 RyuJIT
  ShortRun : .NET Core 5.0.5 (CoreCLR 5.0.521.16609, CoreFX 5.0.521.16609), X64 RyuJIT

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
|                   Method |    N |           Mean |          Error |       StdDev | Ratio | RatioSD |    Gen 0 |    Gen 1 |    Gen 2 | Allocated |
|------------------------- |----- |---------------:|---------------:|-------------:|------:|--------:|---------:|---------:|---------:|----------:|
| **EncryptDecryptV1Baseline** |    **0** |     **1,310.9 ns** |       **812.4 ns** |     **44.53 ns** |  **1.00** |    **0.00** |   **0.4425** |        **-** |        **-** |     **928 B** |
|         EncryptDecryptV2 |    0 |       788.8 ns |     1,001.2 ns |     54.88 ns |  0.60 |    0.04 |   0.2060 |        - |        - |     432 B |
|         EncryptDecryptV3 |    0 |       615.3 ns |     1,809.1 ns |     99.16 ns |  0.47 |    0.06 |   0.1755 |        - |        - |     368 B |
|         EncryptDecryptV4 |    0 |       777.4 ns |     3,250.8 ns |    178.19 ns |  0.59 |    0.13 |   0.2213 |        - |        - |     464 B |
|           EncryptDecrypt |    0 |       491.6 ns |       511.2 ns |     28.02 ns |  0.38 |    0.03 |   0.0839 |        - |        - |     176 B |
|                          |      |                |                |              |       |         |          |          |          |           |
| **EncryptDecryptV1Baseline** |  **100** |   **476,195.6 ns** |   **574,779.3 ns** | **31,505.60 ns** |  **1.00** |    **0.00** |  **90.8203** |        **-** |        **-** |  **192304 B** |
|         EncryptDecryptV2 |  100 |   333,988.4 ns |    93,871.0 ns |  5,145.38 ns |  0.70 |    0.06 |  57.6172 |        - |        - |  121616 B |
|         EncryptDecryptV3 |  100 |   113,997.5 ns |    29,067.6 ns |  1,593.29 ns |  0.24 |    0.01 |  57.8613 |        - |        - |  122160 B |
|         EncryptDecryptV4 |  100 |   133,739.5 ns |    78,504.4 ns |  4,303.09 ns |  0.28 |    0.02 |  29.2969 |        - |        - |   61968 B |
|           EncryptDecrypt |  100 |   118,371.5 ns |   173,434.2 ns |  9,506.51 ns |  0.25 |    0.01 |        - |        - |        - |     176 B |
|                          |      |                |                |              |       |         |          |          |          |           |
| **EncryptDecryptV1Baseline** | **1000** | **5,251,391.8 ns** |   **380,917.1 ns** | **20,879.35 ns** |  **1.00** |    **0.00** | **796.8750** | **664.0625** | **664.0625** | **2713697 B** |
|         EncryptDecryptV2 | 1000 | 3,524,946.4 ns |    60,265.4 ns |  3,303.35 ns |  0.67 |    0.00 | 328.1250 | 328.1250 | 328.1250 | 1219783 B |
|         EncryptDecryptV3 | 1000 | 1,471,757.2 ns | 1,001,476.4 ns | 54,894.30 ns |  0.28 |    0.01 | 333.9844 | 332.0313 | 332.0313 | 1222888 B |
|         EncryptDecryptV4 | 1000 | 1,427,907.1 ns |   806,019.4 ns | 44,180.64 ns |  0.27 |    0.01 | 164.0625 | 164.0625 | 164.0625 |  610251 B |
|           EncryptDecrypt | 1000 | 1,227,539.5 ns | 1,542,175.6 ns | 84,531.85 ns |  0.23 |    0.02 |        - |        - |        - |     177 B |
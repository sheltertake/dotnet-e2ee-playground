``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19042
Intel Core i7-6600U CPU 2.60GHz (Skylake), 1 CPU, 4 logical and 2 physical cores
.NET Core SDK=5.0.202
  [Host]     : .NET Core 5.0.5 (CoreCLR 5.0.521.16609, CoreFX 5.0.521.16609), X64 RyuJIT
  DefaultJob : .NET Core 5.0.5 (CoreCLR 5.0.521.16609, CoreFX 5.0.521.16609), X64 RyuJIT


```
|                          Method | N |       Mean |    Error |   StdDev |  StdErr |        Min |         Q1 |     Median |         Q3 |        Max |        Op/s | Ratio | Rank |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|-------------------------------- |-- |-----------:|---------:|---------:|--------:|-----------:|-----------:|-----------:|-----------:|-----------:|------------:|------:|-----:|-------:|------:|------:|----------:|
| BEncryptDecryptV1BaselineString | 0 | 1,038.3 ns | 14.63 ns | 12.97 ns | 3.47 ns | 1,022.9 ns | 1,026.8 ns | 1,038.2 ns | 1,047.2 ns | 1,065.1 ns |   963,079.0 |  1.00 |    8 | 0.4425 |     - |     - |     928 B |
|         BEncryptDecryptV2String | 0 |   635.5 ns |  1.52 ns |  1.42 ns | 0.37 ns |   633.0 ns |   634.7 ns |   635.6 ns |   636.6 ns |   638.1 ns | 1,573,448.8 |  0.61 |    7 | 0.2060 |     - |     - |     432 B |
|         BEncryptDecryptV3String | 0 |   458.5 ns |  2.27 ns |  1.90 ns | 0.53 ns |   456.8 ns |   457.4 ns |   457.9 ns |   458.2 ns |   462.6 ns | 2,181,092.5 |  0.44 |    3 | 0.1760 |     - |     - |     368 B |
|         BEncryptDecryptV4String | 0 |   469.1 ns |  1.42 ns |  1.18 ns | 0.33 ns |   467.1 ns |   468.6 ns |   469.4 ns |   469.7 ns |   471.4 ns | 2,131,864.2 |  0.45 |    4 | 0.2217 |     - |     - |     464 B |
|   BEncryptDecryptV5StringUnsafe | 0 |   417.8 ns |  1.44 ns |  1.20 ns | 0.33 ns |   415.8 ns |   417.1 ns |   417.9 ns |   418.7 ns |   419.8 ns | 2,393,365.4 |  0.40 |    2 | 0.0839 |     - |     - |     176 B |
|         BEncryptDecryptV6String | 0 |   508.9 ns |  3.52 ns |  3.12 ns | 0.83 ns |   504.9 ns |   506.5 ns |   508.1 ns |   510.5 ns |   515.4 ns | 1,964,883.9 |  0.49 |    6 | 0.1678 |     - |     - |     352 B |
|               BEncryptDecryptV6 | 0 |   418.4 ns |  1.72 ns |  1.61 ns | 0.42 ns |   416.0 ns |   417.4 ns |   418.2 ns |   419.6 ns |   421.4 ns | 2,389,802.2 |  0.40 |    2 | 0.0839 |     - |     - |     176 B |
|         BEncryptDecryptV7String | 0 |   515.5 ns |  2.71 ns |  2.54 ns | 0.65 ns |   510.5 ns |   513.8 ns |   515.8 ns |   517.4 ns |   519.5 ns | 1,939,816.7 |  0.50 |    6 | 0.1678 |     - |     - |     352 B |
|               BEncryptDecryptV7 | 0 |   405.6 ns |  3.37 ns |  2.82 ns | 0.78 ns |   403.2 ns |   403.6 ns |   404.7 ns |   406.2 ns |   411.3 ns | 2,465,343.1 |  0.39 |    1 | 0.0839 |     - |     - |     176 B |
|           BEncryptDecryptString | 0 |   481.6 ns |  1.76 ns |  1.47 ns | 0.41 ns |   479.5 ns |   480.7 ns |   481.4 ns |   482.6 ns |   484.6 ns | 2,076,417.5 |  0.46 |    5 | 0.1678 |     - |     - |     352 B |
|            BEncryptDecryptBytes | 0 |   408.5 ns |  1.13 ns |  1.00 ns | 0.27 ns |   407.3 ns |   407.6 ns |   408.5 ns |   409.3 ns |   410.0 ns | 2,447,747.8 |  0.39 |    1 | 0.0839 |     - |     - |     176 B |

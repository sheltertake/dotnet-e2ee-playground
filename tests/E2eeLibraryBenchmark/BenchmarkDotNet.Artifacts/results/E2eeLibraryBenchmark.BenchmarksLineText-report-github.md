``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19042
Intel Core i7-6600U CPU 2.60GHz (Skylake), 1 CPU, 4 logical and 2 physical cores
.NET Core SDK=5.0.202
  [Host]     : .NET Core 5.0.5 (CoreCLR 5.0.521.16609, CoreFX 5.0.521.16609), X64 RyuJIT
  DefaultJob : .NET Core 5.0.5 (CoreCLR 5.0.521.16609, CoreFX 5.0.521.16609), X64 RyuJIT


```
|                          Method | N |       Mean |    Error |   StdDev |   StdErr |        Min |         Q1 |     Median |         Q3 |        Max |        Op/s | Ratio | Rank |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|-------------------------------- |-- |-----------:|---------:|---------:|---------:|-----------:|-----------:|-----------:|-----------:|-----------:|------------:|------:|-----:|-------:|------:|------:|----------:|
| BEncryptDecryptV1BaselineString | 1 | 4,717.3 ns | 77.99 ns | 69.13 ns | 18.48 ns | 4,639.8 ns | 4,665.5 ns | 4,695.0 ns | 4,745.8 ns | 4,852.2 ns |   211,986.4 |  1.00 |    9 | 1.7395 |     - |     - |    3648 B |
|         BEncryptDecryptV2String | 1 | 3,031.5 ns | 24.34 ns | 21.57 ns |  5.77 ns | 3,006.6 ns | 3,016.6 ns | 3,026.4 ns | 3,042.2 ns | 3,077.1 ns |   329,871.6 |  0.64 |    8 | 0.7782 |     - |     - |    1632 B |
|         BEncryptDecryptV3String | 1 | 1,144.7 ns |  8.94 ns |  8.36 ns |  2.16 ns | 1,130.4 ns | 1,138.3 ns | 1,143.3 ns | 1,150.3 ns | 1,159.7 ns |   873,570.7 |  0.24 |    6 | 0.6962 |     - |     - |    1456 B |
|         BEncryptDecryptV4String | 1 |   998.8 ns |  3.59 ns |  3.00 ns |  0.83 ns |   993.4 ns |   996.6 ns |   998.8 ns | 1,000.4 ns | 1,004.5 ns | 1,001,185.3 |  0.21 |    5 | 0.4501 |     - |     - |     944 B |
|   BEncryptDecryptV5StringUnsafe | 1 |   957.6 ns |  5.20 ns |  4.87 ns |  1.26 ns |   948.5 ns |   954.8 ns |   958.8 ns |   960.9 ns |   965.9 ns | 1,044,280.6 |  0.20 |    4 | 0.0839 |     - |     - |     176 B |
|         BEncryptDecryptV6String | 1 | 1,126.5 ns |  5.22 ns |  4.63 ns |  1.24 ns | 1,120.2 ns | 1,123.4 ns | 1,124.4 ns | 1,131.0 ns | 1,134.5 ns |   887,701.5 |  0.24 |    6 | 0.5569 |     - |     - |    1168 B |
|               BEncryptDecryptV6 | 1 |   937.7 ns |  7.24 ns |  6.42 ns |  1.72 ns |   930.6 ns |   932.9 ns |   935.8 ns |   940.8 ns |   949.8 ns | 1,066,466.2 |  0.20 |    3 | 0.0839 |     - |     - |     176 B |
|         BEncryptDecryptV7String | 1 | 1,681.3 ns |  4.98 ns |  4.42 ns |  1.18 ns | 1,674.3 ns | 1,677.9 ns | 1,681.0 ns | 1,684.9 ns | 1,688.6 ns |   594,784.1 |  0.36 |    7 | 0.6657 |     - |     - |    1392 B |
|               BEncryptDecryptV7 | 1 |   895.7 ns |  3.55 ns |  3.15 ns |  0.84 ns |   890.0 ns |   894.3 ns |   895.9 ns |   897.0 ns |   901.0 ns | 1,116,450.2 |  0.19 |    2 | 0.0839 |     - |     - |     176 B |
|           BEncryptDecryptString | 1 | 1,134.1 ns |  8.86 ns |  7.85 ns |  2.10 ns | 1,124.8 ns | 1,129.8 ns | 1,132.3 ns | 1,137.3 ns | 1,149.9 ns |   881,720.4 |  0.24 |    6 | 0.5722 |     - |     - |    1200 B |
|            BEncryptDecryptBytes | 1 |   844.2 ns | 10.41 ns |  8.69 ns |  2.41 ns |   835.4 ns |   838.1 ns |   841.8 ns |   849.2 ns |   866.5 ns | 1,184,483.6 |  0.18 |    1 | 0.0839 |     - |     - |     176 B |

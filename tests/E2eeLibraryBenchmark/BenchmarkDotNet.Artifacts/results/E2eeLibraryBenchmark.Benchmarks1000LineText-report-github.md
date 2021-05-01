``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19042
Intel Core i7-6600U CPU 2.60GHz (Skylake), 1 CPU, 4 logical and 2 physical cores
.NET Core SDK=5.0.202
  [Host]     : .NET Core 5.0.5 (CoreCLR 5.0.521.16609, CoreFX 5.0.521.16609), X64 RyuJIT
  DefaultJob : .NET Core 5.0.5 (CoreCLR 5.0.521.16609, CoreFX 5.0.521.16609), X64 RyuJIT


```
|                          Method |    N |       Mean |    Error |   StdDev |   StdErr |        Min |         Q1 |     Median |         Q3 |        Max |    Op/s | Ratio | Rank |    Gen 0 |    Gen 1 |    Gen 2 | Allocated |
|-------------------------------- |----- |-----------:|---------:|---------:|---------:|-----------:|-----------:|-----------:|-----------:|-----------:|--------:|------:|-----:|---------:|---------:|---------:|----------:|
| BEncryptDecryptV1BaselineString | 1000 | 4,523.7 μs | 66.70 μs | 55.70 μs | 15.45 μs | 4,422.6 μs | 4,480.8 μs | 4,525.8 μs | 4,556.5 μs | 4,648.3 μs |   221.1 |  1.00 |    8 | 796.8750 | 664.0625 | 664.0625 | 2708608 B |
|         BEncryptDecryptV2String | 1000 | 3,153.4 μs | 41.28 μs | 32.23 μs |  9.30 μs | 3,113.5 μs | 3,139.7 μs | 3,145.0 μs | 3,151.6 μs | 3,241.6 μs |   317.1 |  0.70 |    7 | 328.1250 | 328.1250 | 328.1250 | 1221192 B |
|         BEncryptDecryptV3String | 1000 | 1,421.5 μs |  5.12 μs |  4.79 μs |  1.24 μs | 1,413.2 μs | 1,418.7 μs | 1,422.2 μs | 1,425.0 μs | 1,428.4 μs |   703.5 |  0.31 |    4 | 333.9844 | 332.0313 | 332.0313 | 1223816 B |
|         BEncryptDecryptV4String | 1000 | 1,394.6 μs |  3.89 μs |  3.45 μs |  0.92 μs | 1,388.6 μs | 1,392.4 μs | 1,394.1 μs | 1,397.6 μs | 1,399.8 μs |   717.1 |  0.31 |    3 | 164.0625 | 164.0625 | 164.0625 |  610460 B |
|   BEncryptDecryptV5StringUnsafe | 1000 |   936.8 μs |  2.27 μs |  2.01 μs |  0.54 μs |   933.6 μs |   934.8 μs |   937.1 μs |   938.4 μs |   939.5 μs | 1,067.5 |  0.21 |    2 |        - |        - |        - |     176 B |
|         BEncryptDecryptV6String | 1000 | 1,487.4 μs |  8.82 μs |  8.25 μs |  2.13 μs | 1,471.3 μs | 1,482.9 μs | 1,488.1 μs | 1,491.9 μs | 1,504.7 μs |   672.3 |  0.33 |    5 | 259.7656 | 257.8125 | 257.8125 |  912502 B |
|               BEncryptDecryptV6 | 1000 |   930.7 μs |  3.55 μs |  3.15 μs |  0.84 μs |   924.2 μs |   928.6 μs |   930.6 μs |   932.4 μs |   937.6 μs | 1,074.5 |  0.21 |    2 |        - |        - |        - |     176 B |
|         BEncryptDecryptV7String | 1000 | 1,587.6 μs |  5.75 μs |  5.38 μs |  1.39 μs | 1,579.2 μs | 1,583.4 μs | 1,588.1 μs | 1,592.0 μs | 1,595.1 μs |   629.9 |  0.35 |    6 | 261.7188 | 259.7656 | 259.7656 |  912890 B |
|               BEncryptDecryptV7 | 1000 |   926.7 μs |  3.30 μs |  3.09 μs |  0.80 μs |   919.0 μs |   925.1 μs |   927.1 μs |   928.8 μs |   931.0 μs | 1,079.1 |  0.20 |    2 |        - |        - |        - |     176 B |
|           BEncryptDecryptString | 1000 | 1,425.0 μs |  6.02 μs |  5.63 μs |  1.45 μs | 1,416.9 μs | 1,420.8 μs | 1,424.2 μs | 1,429.0 μs | 1,434.3 μs |   701.7 |  0.32 |    4 | 261.7188 | 259.7656 | 259.7656 |  917609 B |
|            BEncryptDecryptBytes | 1000 |   887.8 μs |  1.78 μs |  1.39 μs |  0.40 μs |   885.2 μs |   887.1 μs |   887.7 μs |   888.8 μs |   890.0 μs | 1,126.3 |  0.20 |    1 |        - |        - |        - |     176 B |
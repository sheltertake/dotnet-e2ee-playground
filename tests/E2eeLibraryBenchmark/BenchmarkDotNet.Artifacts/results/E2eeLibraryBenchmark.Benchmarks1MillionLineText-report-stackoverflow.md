
    BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19042
    Intel Core i7-6600U CPU 2.60GHz (Skylake), 1 CPU, 4 logical and 2 physical cores
    .NET Core SDK=5.0.202
      [Host]     : .NET Core 5.0.5 (CoreCLR 5.0.521.16609, CoreFX 5.0.521.16609), X64 RyuJIT
      DefaultJob : .NET Core 5.0.5 (CoreCLR 5.0.521.16609, CoreFX 5.0.521.16609), X64 RyuJIT


                              Method |       N |       Mean |    Error |   StdDev |  StdErr |        Min |         Q1 |     Median |         Q3 |        Max |   Op/s | Ratio | Rank |     Gen 0 |     Gen 1 |     Gen 2 |    Allocated |
    -------------------------------- |-------- |-----------:|---------:|---------:|--------:|-----------:|-----------:|-----------:|-----------:|-----------:|-------:|------:|-----:|----------:|----------:|----------:|-------------:|
     BEncryptDecryptV1BaselineString | 1000000 | 6,225.1 ms | 11.61 ms |  9.70 ms | 2.69 ms | 6,213.5 ms | 6,218.0 ms | 6,221.2 ms | 6,232.7 ms | 6,242.5 ms | 0.1606 |  1.00 |    9 | 2000.0000 | 2000.0000 | 2000.0000 | 4897150160 B |
             BEncryptDecryptV2String | 1000000 | 4,497.5 ms | 18.45 ms | 14.40 ms | 4.16 ms | 4,478.4 ms | 4,483.0 ms | 4,499.1 ms | 4,510.2 ms | 4,517.0 ms | 0.2223 |  0.72 |    8 | 2000.0000 | 2000.0000 | 2000.0000 | 3360053256 B |
             BEncryptDecryptV3String | 1000000 | 1,762.9 ms |  4.43 ms |  4.14 ms | 1.07 ms | 1,758.2 ms | 1,759.1 ms | 1,762.6 ms | 1,765.8 ms | 1,772.5 ms | 0.5673 |  0.28 |    7 |         - |         - |         - | 1221002216 B |
             BEncryptDecryptV4String | 1000000 | 1,275.5 ms |  2.47 ms |  2.19 ms | 0.58 ms | 1,271.5 ms | 1,274.0 ms | 1,275.5 ms | 1,276.9 ms | 1,279.1 ms | 0.7840 |  0.20 |    4 |         - |         - |         - |  610455184 B |
       BEncryptDecryptV5StringUnsafe | 1000000 |   959.5 ms |  3.19 ms |  2.98 ms | 0.77 ms |   954.0 ms |   957.8 ms |   959.7 ms |   961.0 ms |   964.5 ms | 1.0422 |  0.15 |    3 |         - |         - |         - |        560 B |
             BEncryptDecryptV6String | 1000000 | 1,602.5 ms | 16.73 ms | 15.65 ms | 4.04 ms | 1,585.4 ms | 1,590.4 ms | 1,593.9 ms | 1,616.6 ms | 1,630.4 ms | 0.6240 |  0.26 |    6 |         - |         - |         - |  915775496 B |
                   BEncryptDecryptV6 | 1000000 |   938.4 ms |  3.47 ms |  3.07 ms | 0.82 ms |   932.9 ms |   936.4 ms |   938.8 ms |   939.7 ms |   944.2 ms | 1.0657 |  0.15 |    2 |         - |         - |         - |        464 B |
             BEncryptDecryptV7String | 1000000 | 1,779.6 ms | 30.31 ms | 25.31 ms | 7.02 ms | 1,750.7 ms | 1,769.4 ms | 1,773.5 ms | 1,777.6 ms | 1,843.0 ms | 0.5619 |  0.29 |    7 |         - |         - |         - |  915780400 B |
                   BEncryptDecryptV7 | 1000000 |   964.9 ms |  3.21 ms |  2.85 ms | 0.76 ms |   961.3 ms |   963.0 ms |   963.8 ms |   966.6 ms |   970.7 ms | 1.0364 |  0.16 |    3 |         - |         - |         - |        464 B |
               BEncryptDecryptString | 1000000 | 1,557.0 ms | 17.66 ms | 16.52 ms | 4.27 ms | 1,536.1 ms | 1,543.2 ms | 1,554.7 ms | 1,569.9 ms | 1,591.5 ms | 0.6423 |  0.25 |    5 |         - |         - |         - |  915714704 B |
                BEncryptDecryptBytes | 1000000 |   894.9 ms |  3.02 ms |  2.52 ms | 0.70 ms |   890.7 ms |   893.7 ms |   894.4 ms |   895.2 ms |   899.7 ms | 1.1174 |  0.14 |    1 |         - |         - |         - |        464 B |

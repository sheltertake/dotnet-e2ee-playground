``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19042
Intel Core i7-6600U CPU 2.60GHz (Skylake), 1 CPU, 4 logical and 2 physical cores
.NET Core SDK=5.0.202
  [Host] : .NET Core 5.0.5 (CoreCLR 5.0.521.16609, CoreFX 5.0.521.16609), X64 RyuJIT
  Dry    : .NET Core 5.0.5 (CoreCLR 5.0.521.16609, CoreFX 5.0.521.16609), X64 RyuJIT

Job=Dry  IterationCount=1  LaunchCount=1  
RunStrategy=ColdStart  UnrollFactor=1  WarmupCount=1  

```
|                 Method |       N |    Mean | Error | Ratio |     Gen 0 |     Gen 1 |     Gen 2 | Allocated |
|----------------------- |-------- |--------:|------:|------:|----------:|----------:|----------:|----------:|
| **EncryptDecryptBaseline** |    **1000** | **6.662 s** |    **NA** |  **1.00** | **2000.0000** | **2000.0000** | **2000.0000** |   **4.56 GB** |
|         EncryptDecrypt |    1000 | 6.640 s |    NA |  1.00 | 2000.0000 | 2000.0000 | 2000.0000 |   4.56 GB |
|                        |         |         |       |       |           |           |           |           |
| **EncryptDecryptBaseline** | **1000000** | **7.436 s** |    **NA** |  **1.00** | **2000.0000** | **2000.0000** | **2000.0000** |   **4.56 GB** |
|         EncryptDecrypt | 1000000 | 6.592 s |    NA |  0.89 | 2000.0000 | 2000.0000 | 2000.0000 |   4.56 GB |

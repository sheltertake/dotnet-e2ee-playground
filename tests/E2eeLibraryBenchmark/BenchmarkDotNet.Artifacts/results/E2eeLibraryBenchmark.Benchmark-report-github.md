``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19042
Intel Core i7-6600U CPU 2.60GHz (Skylake), 1 CPU, 4 logical and 2 physical cores
.NET Core SDK=5.0.202
  [Host]   : .NET Core 5.0.5 (CoreCLR 5.0.521.16609, CoreFX 5.0.521.16609), X64 RyuJIT
  ShortRun : .NET Core 5.0.5 (CoreCLR 5.0.521.16609, CoreFX 5.0.521.16609), X64 RyuJIT

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
|                 Method |       N |    Mean |   Error |   StdDev | Ratio | RatioSD |     Gen 0 |     Gen 1 |     Gen 2 | Allocated |
|----------------------- |-------- |--------:|--------:|---------:|------:|--------:|----------:|----------:|----------:|----------:|
| **EncryptDecryptBaseline** |    **1000** | **7.221 s** | **4.216 s** | **0.2311 s** |  **1.00** |    **0.00** | **2000.0000** | **2000.0000** | **2000.0000** |   **4.56 GB** |
|         EncryptDecrypt |    1000 | 7.047 s | 1.170 s | 0.0641 s |  0.98 |    0.04 | 2000.0000 | 2000.0000 | 2000.0000 |   4.56 GB |
|                        |         |         |         |          |       |         |           |           |           |           |
| **EncryptDecryptBaseline** | **1000000** | **6.864 s** | **1.017 s** | **0.0558 s** |  **1.00** |    **0.00** | **2000.0000** | **2000.0000** | **2000.0000** |   **4.56 GB** |
|         EncryptDecrypt | 1000000 | 6.854 s | 1.464 s | 0.0803 s |  1.00 |    0.01 | 2000.0000 | 2000.0000 | 2000.0000 |   4.56 GB |

``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19042
Intel Core i7-6600U CPU 2.60GHz (Skylake), 1 CPU, 4 logical and 2 physical cores
.NET Core SDK=5.0.202
  [Host]   : .NET Core 5.0.5 (CoreCLR 5.0.521.16609, CoreFX 5.0.521.16609), X64 RyuJIT
  ShortRun : .NET Core 5.0.5 (CoreCLR 5.0.521.16609, CoreFX 5.0.521.16609), X64 RyuJIT

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
|                 Method |       N |    Mean |    Error |   StdDev | Ratio | RatioSD |     Gen 0 |     Gen 1 |     Gen 2 | Allocated |
|----------------------- |-------- |--------:|---------:|---------:|------:|--------:|----------:|----------:|----------:|----------:|
| **EncryptDecryptBaseline** |    **1000** | **6.995 s** |  **3.005 s** | **0.1647 s** |  **1.00** |    **0.00** | **2000.0000** | **2000.0000** | **2000.0000** |   **4.56 GB** |
|         EncryptDecrypt |    1000 | 4.810 s |  2.061 s | 0.1130 s |  0.69 |    0.03 | 1000.0000 | 1000.0000 | 1000.0000 |   3.13 GB |
|                        |         |         |          |          |       |         |           |           |           |           |
| **EncryptDecryptBaseline** | **1000000** | **6.898 s** |  **3.005 s** | **0.1647 s** |  **1.00** |    **0.00** | **2000.0000** | **2000.0000** | **2000.0000** |   **4.56 GB** |
|         EncryptDecrypt | 1000000 | 5.864 s | 19.948 s | 1.0934 s |  0.85 |    0.17 | 1000.0000 | 1000.0000 | 1000.0000 |   3.13 GB |

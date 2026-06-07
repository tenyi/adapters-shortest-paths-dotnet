# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## 原始來源 / 對應 Java 專案

本專案是 Java 版 `adapters-shortest-paths-java` 的 C#/.NET 移植，位於 sibling 路徑：

```
../adapters-shortest-paths-java
```

- Java 版的 `adapters-shortest-paths-impl-bsmock` → 此 repo 的 `Programmerare.ShortestPaths.Adaptee.Bsmock`
- Java 版的 `adapters-shortest-paths-impl-yanqi` → 此 repo 的 `Programmerare.ShortestPaths.Adaptee.YanQi`
- Java 版的 `adapters-shortest-paths-core`（介面與預設實作）→ 此 repo 的 `Programmerare.ShortestPaths.Core`
- Java 版另含 `impl-jgrapht` / `impl-mulavito` / `impl-reneargento` / `impl-jython_networkx` / `example-project-jpa-entities`，這些**在 C# 移植版不存在**（C# 版只 port 了 Bsmock + YanQi + QuikGraph 三種實作）。
- Java 版 `pom.xml` 是 Maven 設定；C# 版用 `.sln` / `.csproj`。

> 對照演算法原始碼時，優先看 Java 版的 `src/main/java/edu/ufl/cise/bsmock/graph/` 與 `src/main/java/edu/asu/emit/algorithm/` 兩個目錄。C# 版的命名空間刻意保留 Java 風格（不要重新命名）。

- YanQi 演算法原始 Java 原始碼（第三方 lib，非 adapter 層）位於 sibling 路徑：

  ```
  ../k-shortest-paths-java-version
  ```

  這是 `https://github.com/TomasJohansson/k-shortest-paths-java-version` 的 clone，分支 `programmatic-graph-creation-without-using-inputfile`。核心檔案在 `src/main/java/edu/asu/emit/algorithm/graph/` 下（`YenTopKShortestPathsAlg.java`、`DijkstraShortestPathAlg.java`、`Path.java`、`Graph.java` 等），與 .NET 版 `Adaptee.YanQi/src/edu.asu.emit.algorithm/` 一一對應。

## 專案用途

C# / .NET 10 Adapter 函式庫，封裝三種 K 最短路徑（Yen's algorithm）實作：`Bsmock`、`YanQi`（Java 移植）、`QuikGraph`，用於旅行路線規劃等需要從多個頂點間找出多條最短路徑的情境。

> 升級中：專案正從 .NET Framework 2.0/4.0/4.7.2 改為單一 target `net10.0`，並改用 BCL 集合（`System.Collections.Generic.List<T>` / `SimplePriorityQueue<T, float>` from `OptimizedPriorityQueue`）。多目標 `net20;net40`、NHibernate 範例、Adaptees.Common polyfill 層皆已移除。

## 常用指令

```bash
# 還原 + 建置整個 solution
dotnet restore adapters-shortest-paths-dotnet.sln
dotnet build adapters-shortest-paths-dotnet.sln

# 跑全部 NUnit 測試
dotnet test adapters-shortest-paths-dotnet.sln

# 跑單一測試專案
dotnet test Programmerare.ShortestPaths.Test/Programmerare.ShortestPaths.Test.csproj

# 跑單一測試方法
dotnet test --filter "FullyQualifiedName~PathFinderTestYanQi"

# 跨三種實作平行比較
dotnet run --project Parallel-Test
```

所有專案皆 `net10.0`，可在 macOS / Linux 上直接用 `dotnet` 跑（無需 mono / .NET Framework 相容 runtime）。`Programmerare.ShortestPaths.Example`（NHibernate + SQLite）已從 solution 移除。

## 架構（Adapter 設計模式）

```
Client  ──▶  Core  ◀── Adapter (Bsmock / YanQi / QuikGraph)
              │              │
              │              ▼
              │          Adaptee (第三方演算法原始碼)
              ▼
          OptimizedPriorityQueue (外部套件)
```

- **`Programmerare.ShortestPaths.Core`** — MIT。公開 API + 預設實作。
  - `Core/api/`：對外介面 `PathFinderFactory`、`PathFinder`、`Graph`、`Vertex`、`Edge`、`Weight`、`Path`、`StringRenderable`
  - `Core/api/generics/`：泛型版介面 `PathFinderGenerics<…>` 等
  - `Core/impl/`：預設實作 `GraphImpl`、`VertexImpl`、`EdgeImpl`、`WeightImpl`…
  - `Core/impl/generics/`：`PathFinderBase`、`PathFinderFactoryGenericsBase`
  - `Core/parsers/`、`Core/pathfactories/`、`Core/validation/`、`Utils/`
- **`Programmerare.ShortestPaths.Adapter.{Bsmock,YanQi,QuikGraph}`** — 3 個 Adapter 實作。每個目錄下同時有 `PathFinder*.cs`（具體型別，固定綁定 `Path/Edge/Vertex/Weight`）與 `Generics/PathFinder*Generics.cs`（泛型層）。
- **`Programmerare.ShortestPaths.Adaptee.{Bsmock,YanQi}`** — 第三方演算法原始碼翻譯 / 包裝，已改用 BCL `List<T>` / BCL `IDictionary` / `OptimizedPriorityQueue`（`SimplePriorityQueue<T, float>`）。
- **`Programmerare.ShortestPaths.Test`** — NUnit 測試，**同一份 assertion 會跑三種 Adapter 並比對結果**（見 `test/Programmerare.ShortestPaths.Adapter.Implementations/PathFinderImplementationsTest.cs`）。
- **`Parallel-Test`** — 用 `Task.Run` 平行呼叫三種實作做效能 / 結果比較。

> **已移除**：`Programmerare.ShortestPaths.Adaptees.Common`（Java polyfill）、`Programmerare.ShortestPaths.Example`（NHibernate 範例）、`Programmerare.ShortestPaths.Adaptees.Common.Test`。

## 公開 API 速查

- 入口介面：`Programmerare.ShortestPaths.Core.Api.PathFinderFactory`
- 實作：`PathFinderFactoryBsmock` / `PathFinderFactoryYanQi` / `PathFinderFactoryQuikGraph`
- 用法：

  ```csharp
  var factory = new PathFinderFactoryYanQi();            // 或 Bsmock / QuikGraph
  var pathFinder = factory.CreatePathFinder(graph);
  IList<Path> paths = pathFinder.FindShortestPaths(start, end, maxK);
  ```

- 領域模型：`Graph` 由 `Edge(StartVertex, EndVertex, Weight)` 組成；`Path` 為 `Edge` 列表加 `TotalWeight`。
- 靜態工廠（用 `using static Programmerare.ShortestPaths.Core.Impl.*Impl;`）：`CreateVertex` / `CreateEdge` / `CreateWeight` / `CreateGraph`。

## 升級注意事項

1. **套件版本**（升級後固定）：
   - NUnit 3.14.0、NUnit3TestAdapter 6.2.0、Microsoft.NET.Test.Sdk 18.6.0
   - QuikGraph 2.5.0
   - OptimizedPriorityQueue 5.1.0（API 用 `Enqueue` / `TryDequeue` / `Remove` / `Contains`，priority 為 `float`）
2. **升級期間修的 .NET 10 / OptimizedPriorityQueue 5.x 回歸**：
   - `Bsmock.Dijkstra`：`SimplePriorityQueue.Remove` 對未在 queue 的物件丟例外 → 先 `Contains` 再 `Remove`。
   - `Bsmock.Yen`：`SimplePriorityQueue.Dequeue` 對空 queue 丟例外 → 改用 `TryDequeue` 並於 `false` 時跳出迴圈。
3. **YanQi 演算法 bug（已修復）**：
   `YenTopKShortestPathsAlg` 內 `Dictionary<Path, BaseVertex>` 對 mutable `Path` 物件比對不穩（hash 在 mutate 後變動 ⇒ 去重失效）。已在 Fix-YanQi session 中修復。`YanQi.Path` 的 `Equals` / `GetHashCode` 改為內容比對。
4. **CS 警告（非錯誤）**：YanQi `Graph.cs` 變數 `ss` 從未使用；`Adapter.YanQi/Generics/PathFinderYanQiGenerics.cs` 等少數檔案缺 XML doc 註解、nullable null literal。
5. **全專案測試結果**：`dotnet test adapters-shortest-paths-dotnet.sln` → **Failed 0 / Passed 95 / Skipped 0 / Total 95**

## 授權邊界

- `Core` = MIT，可自由修改。
- `YanQi` / `Bsmock` = Apache 2.0。
- `QuikGraph` = MS-PL。
- 改 `Adaptee` / `Adapter` 內部時**需保留**各專案內的 `License.txt` 與 `NOTICE.txt`。
- 整體授權摘要見根目錄 `NOTICE.txt`。

## 慣例

- 套件命名空間沿用原作者風格：`Adaptee.Bsmock` 內是 `edu.ufl.cise.bsmock.graph.*`，`Adaptee.YanQi` 內是 `edu.asu.emit.algorithm.*`。**改這些檔案時不要重新命名命名空間**。
- 測試框架：NUnit 3 + NUnit3TestAdapter 6（皆已升級）。全部 95 個測試通過（0 failed / 0 skipped）。
- Adapter 命名對稱：每個演算法都有 `*Generics`（泛型）與具體型別版本兩層；新增 Adapter 時須保持此結構。
- `Core` 已設定 `DocumentationFile`，改動後請保留 XML doc 註解以利 `.xml` 產出。
- 測試圖形資源（XML / TXT）位於 `Programmerare.ShortestPaths.Test/test/.../resources/test_graphs/`，新圖形測資請放同層並在 `.csproj` 用 `<Content Include="…" CopyToOutputDirectory="PreserveNewest"/>` 註冊。
- 跨專案「借用」單檔（例：`FileUtil.cs` 給 YanQi.Test / Bsmock.Test 共用）以 `<Compile Include="..\..." Link="..." />` 註冊。

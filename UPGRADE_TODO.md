# .NET 10 升級進度

> 本檔追蹤把專案從 .NET Framework 2.0 / 4.0 / 4.7.2 升級到單一 `net10.0` 的所有進度。
> 已完成項目打勾 ✅；進行中 🟡；待辦 ⬜。

## Phase 1：建置系統與套件升級

- ✅ 全部 14 個 `.csproj` 改為 SDK-style、`<TargetFramework>net10.0</TargetFramework>`
- ✅ 刪除所有 `Properties/AssemblyInfo.cs`（SDK 自動產生）
- ✅ 移除 `Adaptees.Common.csproj` 與 `Adaptees.Common.Test.csproj`、從 `.sln` 摘除
- ✅ 移除 `Example` 專案（NHibernate 不支援 .NET 10）
- ✅ 套件升級：NUnit 3.14.0、NUnit3TestAdapter 6.2.0、Microsoft.NET.Test.Sdk 18.6.0
- ✅ 套件升級：QuikGraph 2.5.0、OptimizedPriorityQueue 5.1.0

## Phase 2：刪除 Adaptees.Common polyfill 層

- ✅ 從 Bsmock / YanQi Adaptee 程式碼移除 `using Programmerare.ShortestPaths.Adaptees.Common.*`
- ✅ 移除 `Adaptees.Common` 整個目錄（`git rm -rf`）
- ✅ 修復 `YanQi.BaseGraph` 內 `using Programmerare.ShortestPaths.Adaptees.Common.DotNetTypes.DotNet20` 與 `#if NET20` 條件式
- ✅ 修復 `YanQi.Graph` / `VariableGraph` 內 `G.IDictionary` / `G.IList` / `G.List` 別名
- ✅ 修復 `Bsmock.Graph` 內 polyfill using
- ✅ 修復 `YanQi.Graph.cs` / `VariableGraph.cs` 內 `ConsoleUtility.WriteLine` → `Console.WriteLine`
- ✅ 修復 `YanQi.Test/FileUtil.cs` 與 `Bsmock.Test/TestYen.cs` 內 `using Adaptees.Common`（重新指向 `Adaptee.YanQi.Test`）

## Phase 3：Adaptee 改用 BCL 集合

- ✅ `Bsmock/util/Path.cs`：`java.util.LinkedList<Edge>` → `List<Edge>`、`.get(i)` → `[i]`、`.subList()` → `.GetRange()`
- ✅ `Bsmock/util/Dijkstra.cs`：`java.util.PriorityQueue<DijkstraNode>` → `SimplePriorityQueue<DijkstraNode, float>`（`OptimizedPriorityQueue`）
- ✅ `Bsmock/ksp/Yen.cs`：`java.util.PriorityQueue<Path>` → `SimplePriorityQueue<Path, float>`
- ✅ `YanQi/graph/Path.cs`：`java.util.LinkedList<BaseVertex>` → `List<BaseVertex>`
- ✅ `YanQi/utils/QYPriorityQueue.cs`：內部 `List<E>` + 移除 `ConsoleUtility`
- ✅ `YanQi/graph/shortestpaths/DijkstraShortestPathAlg.cs`：移除 `#if NET20` 條件式、BCLRenamed
- ✅ `YanQi/graph/shortestpaths/YenTopKShortestPathsAlg.cs`：`.subList` → `.GetRange`、`.indexOf` → `.IndexOf`
- ✅ `YanQi/graph/Graph.cs` / `VariableGraph.cs`：移除 DotNetTypes alias
- ✅ 修復 `YanQi.Path` 的 `Equals` / `GetHashCode` 改用內容比對（先前用 `List<T>.Equals` 是 reference 比較）

## 升級後修的 .NET 10 / OptimizedPriorityQueue 5.x 回歸

- ✅ `Bsmock.Dijkstra`：`SimplePriorityQueue.Remove` 對未在 queue 的物件丟例外 → guard 為 `Contains` 後再 `Remove`
- ✅ `Bsmock.Yen`：`SimplePriorityQueue.Dequeue` 對空 queue 丟例外 → 改 `TryDequeue`，空時 `break` 跳出

## YanQi 演算法 bug 修復

- ✅ **已修復**（Fix-YanQi session）：`YenTopKShortestPathsAlg` 內 `Dictionary<Path, BaseVertex>` 對 mutable `Path` 物件比對不穩的問題
- ✅ 移除 `XmlDefinedTestCasesTest.cs` 內所有 `Assert.Ignore` / `IsKnownYanQiDictionaryBug` 臨時跳過邏輯
- ✅ `Programmerare.ShortestPaths.Adaptee.YanQi.Test` 全部 10 個測試通過（含 `testGraphPossibleToCreateProgrammatically`、`TestSmallGraph`）
- ✅ `Programmerare.ShortestPaths.Test` 全部 82 個測試通過（0 Skipped）

## Phase 8：全專案 build + 全測試套件驗證

- ✅ `dotnet build adapters-shortest-paths-dotnet.sln` 0 errors
- ✅ `dotnet test adapters-shortest-paths-dotnet.sln` → **Failed 0 / Passed 95 / Skipped 0 / Total 95**
  - `Programmerare.ShortestPaths.Test` → Passed 82
  - `K-shortest-paths-Test`（YanQi.Test）→ Passed 10
  - `Programmerare.ShortestPaths.Adaptee.Bsmock.Test` → Passed 2
  - `Programmerare.ShortestPaths.Adapter.QuikGraph.Test` → Passed 1

## Phase 4：Core 現代化（只動 public API 介面以外）

> 範圍經使用者同意：只動 `Core/api/`、`Core/api/generics/` 介面以外的程式碼。

- ⬜ `Core/impl/` 改用 file-scoped namespace（`namespace Programmerare.ShortestPaths.Core.Impl;`）
- ⬜ `Core/impl/generics/` 改用 file-scoped namespace
- ⬜ `Utils/*.cs` 改用 file-scoped namespace
- ⬜ `Core/impl/*Impl.cs`：考慮 `record class` / `init`-only setter
- ⬜ `Core/impl/generics/EdgeGenericsImpl.cs` 等：可改 collection expression `IList<Edge> edges = [...]`

## Phase 5：Adapter.* 與 Tests 現代化（只動 public API 介面以外）

- ⬜ `Adapter.*/PathFinder*.cs`：file-scoped namespace
- ⬜ `Adapter.*/Generics/PathFinder*Generics.cs`：file-scoped namespace、簡化泛型宣告
- ⬜ `Test/Utils/*.cs`：file-scoped namespace、移除 `Net20` 字串常數（已不需要）
- ⬜ 修正 `YanQi/Graph.cs:178` 警告：未使用變數 `ss`
- ⬜ 補回 `Adapter.YanQi/Generics/PathFinderYanQiGenerics.cs` 等缺漏的 XML doc 註解
- ⬜ 修掉 CS8625 nullable null literal 警告

## Phase 6：Example 改寫

- ⬜ ~~把 NHibernate 換成 EF Core + SQLite 重寫 Example~~ → **取消**：`Example` 已於 Phase 1 從 solution 移除（NHibernate 不支援 .NET 10），如需重作再開新任務

## Phase 7：NuGet 與文件

- ⬜ `Programmerare.ShortestPaths.nuspec`：移除 `<file src="...\bin\Release\net20\..." target="lib\net20\" />` 多目標設定，改為單一 `lib\net10.0\`
- ⬜ `nuspec` 升級版本號
- ⬜ `README.md`：更新 framework 描述、移除 NHibernate / multi-target 段落
- ⬜ `NOTICE.txt`：更新授權摘要

## 變更範圍（使用者裁示）

- ✅ 完整現代化（Adaptee 內部 + 套件升級）
- ✅ 目標 framework 為單一 `net10.0`
- ✅ Example 已從 solution 移除
- ✅ 公開 API 介面（`Core/api/`、`Core/api/generics/`）保持原貌

## 注意事項（給未來的自己）

- 改 `Adaptee` / `Adapter` 內部時，**必須保留** `License.txt` 與 `NOTICE.txt`
- 套件命名空間沿用原作者風格（不要重新命名 `edu.ufl.cise.bsmock.graph.*` / `edu.asu.emit.algorithm.*`）
- Adaptees.Common 整個目錄已不存在，不要再試圖 `using Programmerare.ShortestPaths.Adaptees.Common.*`

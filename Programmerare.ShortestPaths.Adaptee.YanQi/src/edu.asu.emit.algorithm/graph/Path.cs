/*	 
* The code in this project is based on the following Java project created by Yan Qi (https://github.com/yan-qi) :
* https://github.com/yan-qi/k-shortest-paths-java-version
* Tomas Johansson later forked the above Java project into this location:
* https://github.com/TomasJohansson/k-shortest-paths-java-version
* Tomas Johansson later translated the above Java code to C#.NET
* That C# code is currently a part of the Visual Studio solution located here:
* https://github.com/TomasJohansson/adapters-shortest-paths-dotnet/
* The current name of the subproject (within the VS solution) with the translated C# code:
* Programmerare.ShortestPaths.Adaptee.YanQi
* 
* Regarding the latest license, Yan Qi has released (19th of Januari 2017) the code with Apache License 2.0
* https://github.com/yan-qi/k-shortest-paths-java-version/commit/d028fd873ff34efc1e851421be380d2382dcb104
* https://github.com/yan-qi/k-shortest-paths-java-version/blob/master/License.txt
* 
* You can also find license information in the files "License.txt" and "NOTICE.txt" in the project root directory.
* 
*/

/*
 * The below copyright statements are included from the original Java code found here:
 * https://github.com/yan-qi/k-shortest-paths-java-version
 * Regarding the translation of that Java code to this .NET code, see above (the top of this source file) for more information.
 * 
 * 
 * Copyright (c) 2004-2008 Arizona State University.  All rights
 * reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 *
 * 1. Redistributions of source code must retain the above copyright
 *    notice, this list of conditions and the following disclaimer.
 *
 * 2. Redistributions in binary form must reproduce the above copyright
 *    notice, this list of conditions and the following disclaimer in
 *    the documentation and/or other materials provided with the
 *    distribution.
 *
 * THIS SOFTWARE IS PROVIDED BY ARIZONA STATE UNIVERSITY ``AS IS'' AND
 * ANY EXPRESSED OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO,
 * THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR
 * PURPOSE ARE DISCLAIMED.  IN NO EVENT SHALL ARIZONA STATE UNIVERSITY
 * NOR ITS EMPLOYEES BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
 * SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
 * LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
 * DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
 * THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
 * OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *
 */
using System.Collections.Generic;
using edu.asu.emit.algorithm.graph.abstraction;
using System;

namespace edu.asu.emit.algorithm.graph {

    /**
     * The class defines a path in graph.
     * 
     * @author yqi
     * 
     * Regarding the above javadoc author statement it applies to the original Java code found here:
     * https://github.com/yan-qi/k-shortest-paths-java-version
     * Regarding the translation of that Java code to this .NET code, see the top of this source file for more information.
     */
    public class Path : BaseElementWithWeight {
	
        // 重構 Path 類別為不可變物件 (Immutable)。
        // 欄位皆設為 readonly，且在建構與讀取時進行防禦性複製，以避免 Dictionary 鍵值雜湊改變的問題。
	    private readonly List<BaseVertex> vertexList;
	    private readonly double weight;
	
	    public Path(IEnumerable<BaseVertex> vertexList, double weight) {
		    this.vertexList = new List<BaseVertex>(vertexList);
		    this.weight = weight;
	    }

	    public double GetWeight() {
		    return weight;
	    }
	
	    // 取值時回傳複本（防禦性複製），相容於 .NET 2.0 且防止外部修改內部列表
	    public List<BaseVertex> GetVertexList() {
		    return new List<BaseVertex>(vertexList);
	    }
	
	    public override bool Equals(object right) {

		    if (right is Path) {
			    Path rPath = (Path) right;
			    if (vertexList.Count != rPath.vertexList.Count) {
				    return false;
			    }
			    for (int i = 0; i < vertexList.Count; i++) {
				    if (!Equals(vertexList[i], rPath.vertexList[i])) {
					    return false;
				    }
			    }
			    return true;
		    }
		    return false;
	    }

	    public override int GetHashCode() {
		    int hash = 0;
		    foreach (BaseVertex v in vertexList) {
			    hash = unchecked(hash * 31 + (v == null ? 0 : v.GetHashCode()));
		    }
		    return hash;
	    }
	
	    public override String ToString() {
		    return vertexList.ToString() + ":" + weight;
	    }
    }
}
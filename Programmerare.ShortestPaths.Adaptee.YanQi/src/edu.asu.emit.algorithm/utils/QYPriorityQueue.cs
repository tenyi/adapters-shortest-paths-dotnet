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
 * Copyright (c) 2004-2009 Arizona State University.  All rights
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
 * PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL ARIZONA STATE UNIVERSITY
 * NOR ITS EMPLOYEES BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
 * SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
 * LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
 * DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
 * THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
 * OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *
 */
namespace edu.asu.emit.algorithm.utils
{
using System;
using System.Collections.Generic;
using edu.asu.emit.algorithm.graph.abstraction;
/**
 * The class defines a priority queue.
 * @author yqi
 * @date Mar 16, 2015
 * @param <E> the type of the element in the queue
 *
 * Regarding the above javadoc author statement it applies to the original Java code found here:
 * https://github.com/yan-qi/k-shortest-paths-java-version
 * Regarding the translation of that Java code to this .NET code, see the top of this source file for more information.
 */
public class QYPriorityQueue<E> where E : BaseElementWithWeight {
    // 用於排序的優先權佇列，底層為排序好的 List
	private List<E> elementWeightPairList = new List<E>();
	private int limitSize = -1;
    // 將原本容易誤導的 isIncremental 命名改為 sortDescending
    // sortDescending = false：升序排序（從小到大），Poll() 傳回最小值
    // sortDescending = true ：降序排序（從大到小），Poll() 傳回最大值
	private bool sortDescending = false;

	/**
	 * Default constructor.
	 */
	public QYPriorityQueue() { }

	/**
	 * Constructor.
	 * @param pLimitSize
	 */
	public QYPriorityQueue(int pLimitSize, bool pSortDescending) {
		limitSize = pLimitSize;
		sortDescending = pSortDescending;
	}


	public override String ToString() {
		return elementWeightPairList.ToString();
	}

	/**
	 * 使用二分搜尋法尋找新元素的正確插入位置，以維持佇列的排序狀態
	 * @param weight
	 * @return 新元素的插入位置 index
	 */
	private int BinLocatePos(double weight, bool sortDescending)	{
		int mid = 0;
		int low = 0;
		int high = elementWeightPairList.Count - 1;
		//
		while (low <= high) {
			mid = (low + high) / 2;
			if (elementWeightPairList[mid].GetWeight() == weight) {
				return mid + 1;
			}

			if (sortDescending) { // 降序邏輯 (大的在前，遞減排列)
				if (elementWeightPairList[mid].GetWeight() < weight) {
					high = mid - 1;
				} else {
					low = mid + 1;
				}
			} else { // 升序邏輯 (小的在前，遞增排列)
				if (elementWeightPairList[mid].GetWeight() > weight) {
					high = mid - 1;
				} else {
					low = mid + 1;
				}
			}
		}
		return low;
	}

	/**
	 * Add a new element in the queue.
	 * @param element
	 */
	public void Add(E element) {
		elementWeightPairList.Insert(BinLocatePos(element.GetWeight(), sortDescending), element);

		if (limitSize > 0 && elementWeightPairList.Count > limitSize) {
			int sizeOfResults = elementWeightPairList.Count;
			elementWeightPairList.RemoveAt(sizeOfResults - 1);
		}
	}

	/**
	 * It only reflects the size of the current results.
	 * @return
	 */
	public int Size() {
		return elementWeightPairList.Count;
	}

	/**
	 * Get the i th element.
	 * @param i
	 * @return
	 */
	public E Get(int i) {
		if (i >= elementWeightPairList.Count) {
			Console.WriteLine("The result :" + i + " doesn't exist!!!");
		}
		return elementWeightPairList[i];
	}

	/**
	 * Get the first element, and then remove it from the queue.
	 * @return
	 */
	public E Poll() {
		E ret = elementWeightPairList[0];
		elementWeightPairList.RemoveAt(0);
		return ret;
	}

	/**
	 * Check if it's empty.
	 * @return
	 */
	public bool IsEmpty() {
		return elementWeightPairList.Count == 0;
	}

}
}

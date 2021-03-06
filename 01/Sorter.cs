﻿#region using

using System;
using System.Diagnostics;
using System.Linq;
using static _01.Helper;

#endregion

namespace _01 {
    internal class Heap : ISorter {
        #region Implementation of ISorter

        /// <inheritdoc />
        public CounterArray<int> MArray { get; set; }

        /// <inheritdoc />
        public void Sort() { heapSort( this.MArray, this.MArray.Length ); }

        private void heapSort(CounterArray<int> arr, int n) {
            for ( var i = n / 2 - 1; i >= 0; i-- ) {
                Sleep( this.MArray.Length );
                heapify( arr, n, i );
            }

            for ( var i = n - 1; i >= 0; i-- ) {
                var temp = arr[0];
                arr[0] = arr[i];
                arr[i] = temp;

                Sleep( this.MArray.Length );
                heapify( arr, i, 0 );
            }
        }

        private void heapify(CounterArray<int> arr, int n, int i) {
            var largest                                           = i;
            var left                                              = 2 * i + 1;
            var right                                             = 2 * i + 2;
            if ( left  < n && arr[left]  > arr[largest] ) largest = left;
            if ( right < n && arr[right] > arr[largest] ) largest = right;
            if ( largest != i ) {
                var swap = arr[i];
                arr[i]       = arr[largest];
                arr[largest] = swap;
                heapify( arr, n, largest );
            }
        }

        #endregion
    }

    internal class Merge : ISorter {
        #region Implementation of ISorter

        /// <inheritdoc />
        public CounterArray<int> MArray { get; set; }

        /// <inheritdoc />
        public void Sort() { mergeSort( 0, this.MArray.Length - 1 ); }


        private void merges(int l, int m, int r) {
            int i, j, k;
            var n1 = m - l + 1;
            var n2 = r     - m;

            var L = new int[n1];
            var R = new int [n2];

            for ( i = 0; i < n1; i++ ) L[i] = this.MArray[l     + i];
            for ( j = 0; j < n2; j++ ) R[j] = this.MArray[m + 1 + j];

            i = 0; // Initial index of first subarray 
            j = 0; // Initial index of second subarray 
            k = l; // Initial index of merged subarray 
            while ( i < n1 && j < n2 ) {
                if ( L[i] <= R[j] ) {
                    this.MArray[k] = L[i];
                    i++;
                }
                else {
                    this.MArray[k] = R[j];
                    j++;
                }

                k++;
                Sleep( this.MArray.Length * 4 );
            }

            while ( i < n1 ) {
                this.MArray[k] = L[i];
                i++;
                k++;
            }

            while ( j < n2 ) {
                this.MArray[k] = R[j];
                j++;
                k++;
            }
        }

        private void mergeSort(int l, int r) {
            if ( l < r ) {
                var m = l + ( r - l ) / 2;

                mergeSort( l,     m );
                mergeSort( m + 1, r );

                merges( l, m, r );
            }
        }

        #endregion
    }

    internal class Bubble : ISorter {
        #region Implementation of ISorter

        /// <inheritdoc />
        public CounterArray<int> MArray { get; set; }

        /// <inheritdoc />
        public void Sort() {
            for ( var write = 0; write < this.MArray.Length; write++ ) {
                Sleep( this.MArray.Length );
                for ( var sort = 0; sort < this.MArray.Length - 1; sort++ )
                    if ( this.MArray[sort] > this.MArray[sort + 1] ) {
                        var temp = this.MArray[sort + 1];
                        this.MArray[sort + 1] = this.MArray[sort];
                        this.MArray[sort]     = temp;
                    }
            }
        }

        #endregion
    }

    internal class Quick : ISorter {
        private CounterArray<int> _array;

        private void quicksort(int links, int rechts, ref CounterArray<int> daten) {
            if ( links < rechts ) {
                var teiler = teile( links, rechts, ref daten );
                quicksort( links, teiler - 1, ref daten );
                quicksort( teiler        + 1, rechts, ref daten );
            }
        }

        private int teile(int links, int rechts, ref CounterArray<int> daten) {
            var i     = links;
            var j     = rechts - 1;
            var pivot = daten[rechts];

            do {
                while ( daten[i] <= pivot && i < rechts ) i += 1;
                while ( daten[j] >= pivot && j > links ) j  -= 1;

                Sleep( this.MArray.Length );
                
                if ( i < j ) {
                    var z = daten[i];
                    daten[i] = daten[j];
                    daten[j] = z;
                }
            } while ( i < j );

            if ( daten[i] > pivot ) {
                var z = daten[i];
                daten[i]      = daten[rechts];
                daten[rechts] = z;
            }

            return i;
        }

        #region Implementation of ISorter

        /// <inheritdoc />
        public CounterArray<int> MArray { get => this._array; set => this._array = value; }

        /// <inheritdoc />
        public void Sort() { quicksort( 0, this._array.Length - 1, ref this._array ); }

        #endregion
    }

    internal class Cycle : ISorter {
        #region Implementation of ISorter

        /// <inheritdoc />
        public CounterArray<int> MArray { get; set; }

        /// <inheritdoc />
        public void Sort() { cycleSort( this.MArray, this.MArray.Length ); }

        public void cycleSort(CounterArray<int> arr, int n) {
            var writes = 0;

            for ( var cycle_start = 0; cycle_start <= n - 2; cycle_start++ ) {
                var item = arr[cycle_start];

                var pos = cycle_start;
                for ( var i = cycle_start + 1; i < n; i++ )
                    if ( arr[i] < item )
                        pos++;

                if ( pos == cycle_start ) continue;

                while ( item == arr[pos] ) pos += 1;

                if ( pos != cycle_start ) {
                    var temp = item;
                    item     = arr[pos];
                    arr[pos] = temp;
                    writes++;

                    Sleep( this.MArray.Length );
                }

                while ( pos != cycle_start ) {
                    pos = cycle_start;

                    for ( var i = cycle_start + 1; i < n; i++ )
                        if ( arr[i] < item )
                            pos += 1;

                    while ( item == arr[pos] ) pos += 1;

                    if ( item != arr[pos] ) {
                        var temp = item;
                        item     = arr[pos];
                        arr[pos] = temp;
                        writes++;

                        Sleep( this.MArray.Length );
                    }
                }
            }
        }

        #endregion
    }

    internal class My : ISorter {
        #region Implementation of ISorter

        private readonly ISorter _inner;

        /// <inheritdoc />
        public CounterArray<int> MArray { get; set; }

        /// <inheritdoc />
        public void Sort() {
            sort( this.MArray );
            Console.WriteLine( "change" );
            this._inner.MArray = this.MArray;
            this._inner.Sort();
        }

        private int getNextGap(int gap) {
            // Shrink gap by Shrink factor 
            gap = gap * 10                / 13;
            if ( gap < this.MArray.Length / 50 ) return 1;
            return gap;
        }

        private void sort(CounterArray<int> arr) {
            var n = arr.Length;

            var gap = n;

            var swapped = true;

            while ( gap != 1 || swapped ) {
                gap = getNextGap( gap );
                if ( gap == 1 ) break;
                swapped = false;

                for ( var i = 0; i < n - gap; i++ )
                    if ( arr[i] > arr[i + gap] ) {
                        Sleep( this.MArray.Length * 4 );
                        var temp = arr[i];
                        arr[i] = arr[i + gap];
                        arr[i          + gap] = temp;

                        swapped = true;
                    }
            }
        }

        public My() => this._inner = new Selection();

        #endregion
    }

    internal class Comb : ISorter {
        private static int getNextGap(int gap) {
            // Shrink gap by Shrink factor 
            gap = gap * 10 / 13;
            if ( gap < 1 ) return 1;
            return gap;
        }

        private void sort(CounterArray<int> arr) {
            var n = arr.Length;

            var gap = n;

            var swapped = true;

            while ( gap != 1 || swapped ) {
                gap = getNextGap( gap );

                swapped = false;

                for ( var i = 0; i < n - gap; i++ )
                    if ( arr[i] > arr[i + gap] ) {
                        Sleep( this.MArray.Length );
                        var temp = arr[i];
                        arr[i] = arr[i + gap];
                        arr[i          + gap] = temp;

                        swapped = true;
                    }
            }
        }

        #region Implementation of ISorter

        /// <inheritdoc />
        public CounterArray<int> MArray { get; set; }

        /// <inheritdoc />
        public void Sort() { sort( this.MArray ); }

        #endregion
    }

    internal class Pigeonhole : ISorter {
        public void pigeonhole_sort(CounterArray<int> arr, int n) {
            var min = arr[0];
            var max = arr[0];
            int range, i, j, index;

            for ( var a = 0; a < n; a++ ) {
                if ( arr[a] > max ) max = arr[a];
                if ( arr[a] < min ) min = arr[a];
            }

            range = max - min + 1;
            var phole = new int[range];

            for ( i = 0; i < n; i++ ) phole[i] = 0;

            for ( i = 0; i < n; i++ ) phole[arr[i] - min]++;

            index = 0;

            for ( j = 0; j < range; j++ )
                while ( phole[j]-- > 0 ) {
                    arr[index++] = j + min;

                    Sleep( this.MArray.Length );
                }
        }

        #region Implementation of ISorter

        /// <inheritdoc />
        public CounterArray<int> MArray { get; set; }

        /// <inheritdoc />
        public void Sort() { pigeonhole_sort( this.MArray, this.MArray.Length ); }

        #endregion
    }

    internal class Selection : ISorter {
        private void sort() {
            var n = this.MArray.Length;

            for ( var i = 0; i < n - 1; i++ ) {
                Sleep( this.MArray.Length );
                var min_idx = i;
                for ( var j = i + 1; j < n; j++ )
                    if ( this.MArray[j] < this.MArray[min_idx] )
                        min_idx = j;

                var temp = this.MArray[min_idx];
                this.MArray[min_idx] = this.MArray[i];
                this.MArray[i]       = temp;
            }
        }

        #region Implementation of ISorter

        /// <inheritdoc />
        public CounterArray<int> MArray { get; set; }

        /// <inheritdoc />
        public void Sort() { sort(); }

        #endregion
    }

    internal class Insertion : ISorter {
        #region Implementation of ISorter

        /// <inheritdoc />
        public CounterArray<int> MArray { get; set; }

        /// <inheritdoc />
        public void Sort() { sort(); }

        private void sort() {
            var n = this.MArray.Length;
            for ( var i = 1; i < n; ++i ) {
                Sleep( this.MArray.Length );

                var key = this.MArray[i];
                var j   = i - 1;

                while ( j >= 0 && this.MArray[j] > key ) {
                    this.MArray[j + 1] = this.MArray[j];
                    j                  = j - 1;
                }

                this.MArray[j + 1] = key;
            }
        }

        private void insertionSortRecursive(int n) {
            if ( n <= 1 ) return;

            insertionSortRecursive( n - 1 );

            var last = this.MArray[n - 1];
            var j    = n - 2;

            while ( j >= 0 && this.MArray[j] > last ) {
                Sleep( this.MArray.Length );
                this.MArray[j + 1] = this.MArray[j];
                j--;
            }

            this.MArray[j + 1] = last;
        }

        #endregion
    }

    internal class Counting : ISorter {
        #region Implementation of ISorter

        /// <inheritdoc />
        public CounterArray<int> MArray { get; set; }

        /// <inheritdoc />
        public void Sort() {
            //throw new NotImplementedException();
            countSort();
        }

        private void countSort() {
            var max    = this.MArray.InnerArray.Max();
            var min    = this.MArray.InnerArray.Min();
            var range  = max - min + 1;
            var count  = new int[range];
            var output = new int[this.MArray.Length];
            for ( var i = 0; i < this.MArray.Length; i++ ) count[this.MArray[i] - min]++;

            for ( var i = 1; i < count.Length; i++ ) count[i] += count[i - 1];

            for ( var i = this.MArray.Length - 1; i >= 0; i-- ) {
                output[count[this.MArray[i] - min] - 1] = this.MArray[i];
                count[this.MArray[i]               - min]--;
            }

            for ( var i = 0; i < this.MArray.Length; i++ ){
                Sleep(MArray.Length ); this.MArray[i] = output[i];}
        }

        #endregion
    }

    internal class Radix : ISorter {
        #region Implementation of ISorter

        /// <inheritdoc />
        public CounterArray<int> MArray { get; set; }

        /// <inheritdoc />
        public void Sort() { radixsort( this.MArray.Length ); }

        public int getMax(int n) {
            var mx = this.MArray[0];
            for ( var i = 1; i < n; i++ )
                if ( this.MArray[i] > mx )
                    mx = this.MArray[i];
            return mx;
        }

        public void countSort(int n, int exp) {
            var output = new int[n];
            int i;
            var count = new int[10];

            for ( i = 0; i < 10; i++ ) count[i] = 0;

            for ( i = 0; i < n; i++ ) count[this.MArray[i] / exp % 10]++;
            for ( i = 1; i < 10; i++ ) count[i] += count[i - 1];

            for ( i = n - 1; i >= 0; i-- ) {
                Sleep( this.MArray.Length );

                output[count[this.MArray[i] / exp % 10] - 1] = this.MArray[i];
                count[this.MArray[i] / exp % 10]--;
            }

            for ( i = 0; i < n; i++ ) this.MArray[i] = output[i];
        }

        public void radixsort(int n) {
            var m = getMax( n );

            for ( var exp = 1; m / exp > 0; exp *= 10 ) countSort( n, exp );
        }

        #endregion
    }

    internal class Cocktail : ISorter {
        #region Implementation of ISorter

        /// <inheritdoc />
        public CounterArray<int> MArray { get; set; }

        /// <inheritdoc />
        public void Sort() { cocktailSort(); }

        private void cocktailSort() {
            var swapped = true;
            var start   = 0;
            var end     = this.MArray.Length;

            while ( swapped ) {
                Sleep( this.MArray.Length / 2 );

                swapped = false;

                for ( var i = start; i < end - 1; ++i )
                    if ( this.MArray[i] > this.MArray[i + 1] ) {
                        var temp = this.MArray[i];
                        this.MArray[i] = this.MArray[i + 1];
                        this.MArray[i                  + 1] = temp;
                        swapped                             = true;
                    }

                if ( swapped == false ) break;

                swapped = false;

                end = end - 1;

                for ( var i = end - 1; i >= start; i-- )
                    if ( this.MArray[i] > this.MArray[i + 1] ) {
                        var temp = this.MArray[i];
                        this.MArray[i] = this.MArray[i + 1];
                        this.MArray[i                  + 1] = temp;
                        swapped                             = true;
                    }

                start = start + 1;
            }
        }

        #endregion
    }

    internal class Bitonic : ISorter {
        #region Implementation of ISorter

        /// <inheritdoc />
        public CounterArray<int> MArray { get; set; }

        /// <inheritdoc />
        public void Sort() {
            bitonicSort( 0, this.MArray.Length, 1 );
            new Selection { MArray = this.MArray }.Sort();
        }

        private static void Swap <T>(ref T lhs, ref T rhs) {
            T temp;
            temp = lhs;
            lhs  = rhs;
            rhs  = temp;
        }

        public void compAndSwap(int i, int j, int dir) {
            int k;
            if ( this.MArray[i] > this.MArray[j] )
                k = 1;
            else
                k = 0;
            if ( dir == k ) {
                var temp = this.MArray[i];
                this.MArray[i] = this.MArray[j];
                this.MArray[j] = temp;
            }
        }

        public void bitonicMerge(int low, int cnt, int dir) {
            if ( cnt > 1 ) {
                Sleep( this.MArray.Length * 2 );

                var k = cnt / 2;
                for ( var i = low; i < low + k; i++ ) compAndSwap( i, i + k, dir );
                bitonicMerge( low,     k, dir );
                bitonicMerge( low + k, k, dir );
            }
        }

        public void bitonicSort(int low, int cnt, int dir) {
            if ( cnt > 1 ) {
                var k = cnt / 2;

                bitonicSort( low, k, 1 );

                bitonicSort( low + k, k, 0 );

                bitonicMerge( low, cnt, dir );
            }
        }

        #endregion
    }

    internal class Shell : ISorter {
        #region Implementation of ISorter

        /// <inheritdoc />
        public CounterArray<int> MArray { get; set; }

        /// <inheritdoc />
        public void Sort() { sort(); }

        private int sort() {
            var n = this.MArray.Length;

            for ( var gap = n / 2; gap > 0; gap /= 2 ) {
                for ( var i = gap; i < n; i += 1 ) {
                    Sleep( this.MArray.Length * 4 );
                    var temp = this.MArray[i];

                    int j;
                    for ( j = i; j >= gap && this.MArray[j - gap] > temp; j -= gap ) this.MArray[j] = this.MArray[j - gap];

                    this.MArray[j] = temp;
                }
            }

            return 0;
        }

        #endregion
    }

    internal class Time : ISorter {
        #region Implementation of ISorter

        /// <inheritdoc />
        public CounterArray<int> MArray { get; set; }

        /// <inheritdoc />
        public void Sort() { timSort( this.MArray.Length ); }

        public const int RUN = 32;

        public void insertionSort(int left, int right) {
            for ( var i = left + 1; i <= right; i++ ) {
                var temp = this.MArray[i];
                var j    = i - 1;
                while ( this.MArray[j] > temp && j >= left ) {
                    this.MArray[j + 1] = this.MArray[j];
                    j--;

                    if ( j < 0 ) break;
                }

                this.MArray[j + 1] = temp;
            }
        }

        public void merge(int l, int m, int r) {
            int len1                                  = m - l + 1, len2 = r - m;
            var left                                  = new int[len1];
            var right                                 = new int[len2];
            for ( var x = 0; x < len1; x++ ) left[x]  = this.MArray[l     + x];
            for ( var x = 0; x < len2; x++ ) right[x] = this.MArray[m + 1 + x];

            var i = 0;
            var j = 0;
            var k = l;

            while ( i < len1 && j < len2 ) {
                Sleep( this.MArray.Length );
                if ( left[i] <= right[j] ) {
                    this.MArray[k] = left[i];
                    i++;
                }
                else {
                    this.MArray[k] = right[j];
                    j++;
                }

                k++;
            }

            while ( i < len1 ) {
                this.MArray[k] = left[i];
                k++;
                i++;
            }

            while ( j < len2 ) {
                this.MArray[k] = right[j];
                k++;
                j++;
            }
        }

        public void timSort(int n) {
            for ( var i = 0; i < n; i += RUN ) insertionSort( i, Math.Min( i + 31, n - 1 ) );

            for ( var size = RUN; size < n; size = 2 * size ) {
                for ( var left = 0; left < n; left += 2 * size ) {
                    var mid   = left + size - 1;
                    var right = Math.Min( left + 2 * size - 1, n - 1 );

                    merge( left, mid, right );
                }
            }
        }

        #endregion
    }

    internal class Binary : ISorter {
        #region Implementation of ISorter

        /// <inheritdoc />
        public CounterArray<int> MArray { get; set; }

        /// <inheritdoc />
        public void Sort() { sort(); }

        private void sort() {
            for ( var i = 1; i < this.MArray.Length; i++ ) {
                var x = this.MArray[i];

                Sleep( this.MArray.Length );
                var j = Math.Abs( Array.BinarySearch( this.MArray.InnerArray, 0, i, x ) + 1 );

                Array.Copy( this.MArray.InnerArray, j, this.MArray.InnerArray, j + 1, i - j );
                //this.MArray[i] = this.MArray[i];

                this.MArray[j] = x;
            }
        }

        #endregion
    }

    internal class Gnome : ISorter {
        #region Implementation of ISorter

        /// <inheritdoc />
        public CounterArray<int> MArray { get; set; }

        /// <inheritdoc />
        public void Sort() { gnomeSort( this.MArray.Length ); }

        private void gnomeSort(int n) {
            var index = 0;

            while ( index < n ) {
                if ( index == 0 ) index++;
                if ( this.MArray[index] >= this.MArray[index - 1] ) {
                    index++;
                }
                else {
                    var temp = 0;
                    temp               = this.MArray[index];
                    this.MArray[index] = this.MArray[index - 1];
                    this.MArray[index                      - 1] = temp;
                    index--;
                }
            }
        }

        #endregion
    }

    internal class Stooge : ISorter {
        #region Implementation of ISorter

        /// <inheritdoc />
        public CounterArray<int> MArray { get; set; }

        /// <inheritdoc />
        public void Sort() { stoogesort( 0, this.MArray.Length - 1 ); }


        private void stoogesort(int l, int h) {
            if ( l >= h ) return;
            if ( this.MArray[l] > this.MArray[h] ) {
                var t = this.MArray[l];
                this.MArray[l] = this.MArray[h];
                this.MArray[h] = t;
                Sleep( this.MArray.Length * 64 );
            }

            if ( h - l + 1 > 2 ) {
                var t = ( h - l + 1 ) / 3;

                stoogesort( l, h - t );
                stoogesort( l    + t, h );
                stoogesort( l,        h - t );
            }
        }

        #endregion
    }

    internal class Tree : ISorter {
        #region Implementation of ISorter

        /// <inheritdoc />
        public CounterArray<int> MArray { get; set; }

        /// <inheritdoc />
        public void Sort() {
            var tree = new GFG();
            tree.treeins( this.MArray );
            tree.inorderRec( tree.root );
        }

        private class GFG {
            public Node root;

            public GFG() => this.root = null;

            private void insert(int key) { this.root = insertRec( this.root, key ); }

            private Node insertRec(Node root, int key) {
                if ( root == null ) {
                    root = new Node( key );
                    return root;
                }

                if ( key < root.key )
                    root.left                         = insertRec( root.left,  key );
                else if ( key > root.key ) root.right = insertRec( root.right, key );

                return root;
            }

            public void inorderRec(Node root) {
                if ( root != null ) {
                    inorderRec( root.left );
                    inorderRec( root.right );
                }
            }

            public void treeins(CounterArray<int> arr) {
                for ( var i = 0; i < arr.Length; i++ ) {
                    Sleep( arr.Length );
                    insert( arr[i] );
                }
            }

            public class Node {
                public readonly int  key;
                public          Node left, right;

                public Node(int item) {
                    this.key  = item;
                    this.left = this.right = null;
                }
            }
        }

        #endregion
    }

    internal class OddEven : ISorter {
        #region Implementation of ISorter

        /// <inheritdoc />
        public CounterArray<int> MArray { get; set; }

        /// <inheritdoc />
        public void Sort() { oddEvenSort( this.MArray.Length ); }

        public void oddEvenSort(int n) {
            var isSorted = false;

            while ( !isSorted ) {
                isSorted = true;
                int temp;

                for ( var i = 1; i <= n - 2; i = i + 2 )
                    if ( this.MArray[i] > this.MArray[i + 1] ) {
                        temp           = this.MArray[i];
                        this.MArray[i] = this.MArray[i + 1];
                        this.MArray[i                  + 1] = temp;
                        isSorted                            = false;
                    }

                Sleep( this.MArray.Length / 2 );

                for ( var i = 0; i <= n - 2; i = i + 2 )
                    if ( this.MArray[i] > this.MArray[i + 1] ) {
                        temp           = this.MArray[i];
                        this.MArray[i] = this.MArray[i + 1];
                        this.MArray[i                  + 1] = temp;
                        isSorted                            = false;
                    }
            }
        }

        #endregion
    }

    internal class Quick3 : ISorter {
        #region Implementation of ISorter

        /// <inheritdoc />
        public CounterArray<int> MArray { get; set; }

        /// <inheritdoc />
        public void Sort() { quicksort( 0, this.MArray.Length - 1 ); }

        private void swap(int lhs, int rhs) {
            var temp = this.MArray[lhs];
            this.MArray[lhs] = this.MArray[rhs];
            this.MArray[rhs] = temp;
        }

        public void partition(int l, int r, ref int i, ref int j) {
            i = l - 1;
            j = r;
            int p = l - 1, q = r;
            var v = this.MArray[r];

            while ( true ) {
                while ( this.MArray[++i] < v ) ;

                while ( v < this.MArray[--j] )
                    if ( j == l )
                        break;

                if ( i >= j ) break;

                swap( i, j );

                if ( this.MArray[i] == v ) {
                    p++;
                    swap( p, i );
                }

                if ( this.MArray[j] == v ) {
                    q--;
                    swap( j, q );
                }
            }

            swap( i, r );

            Sleep( this.MArray.Length / 2 );
            j = i - 1;
            for ( var k = l; k < p; k++, j-- ) swap( k, j );

            i = i + 1;
            for ( var k = r - 1; k > q; k--, i++ ) swap( i, k );
        }

        public void quicksort(int l, int r) {
            if ( r <= l ) return;

            int i = 0, j = 0;

            Sleep( this.MArray.Length / 2 );
            partition( l, r, ref i, ref j );
            quicksort( l, j );
            quicksort( i, r );
        }

        #endregion
    }

    internal class Introsort : ISorter {
        #region Implementation of ISorter

        /// <inheritdoc />
        public CounterArray<int> MArray { get; set; }

        /// <inheritdoc />
        public void Sort() { IntroSort(); }

        private void IntroSort() {
            var partitionSize = Partition( 0, this.MArray.Length - 1 );

            if ( partitionSize < 16 )
                InsertionSort();
            else if ( partitionSize > 2 * Math.Log( this.MArray.Length ) )
                HeapSort();
            else
                QuickSortRecursive( 0, this.MArray.Length - 1 );
        }

        private void InsertionSort() {
            for ( var i = 1; i < this.MArray.Length; ++i ) {
                var j = i;
                Sleep( this.MArray.Length );

                while ( j > 0 )
                    if ( this.MArray[j - 1] > this.MArray[j] ) {
                        this.MArray[j - 1] ^= this.MArray[j];
                        this.MArray[j]     ^= this.MArray[j - 1];
                        this.MArray[j                       - 1] ^= this.MArray[j];

                        --j;
                    }
                    else {
                        break;
                    }
            }
        }

        private void HeapSort() {
            var heapSize = this.MArray.Length;

            for ( var p = ( heapSize - 1 ) / 2; p >= 0; --p ) {
                Sleep( this.MArray.Length );
                MaxHeapify( heapSize, p );
            }

            for ( var i = this.MArray.Length - 1; i > 0; --i ) {
                var temp = this.MArray[i];
                this.MArray[i] = this.MArray[0];
                this.MArray[0] = temp;

                --heapSize;
                Sleep( this.MArray.Length );
                MaxHeapify( heapSize, 0 );
            }
        }

        private void MaxHeapify(int heapSize, int index) {
            var left    = ( index + 1 ) * 2 - 1;
            var right   = ( index + 1 ) * 2;
            var largest = 0;

            if ( left < heapSize && this.MArray[left] > this.MArray[index] )
                largest = left;
            else
                largest = index;

            if ( right < heapSize && this.MArray[right] > this.MArray[largest] ) largest = right;

            if ( largest != index ) {
                var temp = this.MArray[index];
                this.MArray[index]   = this.MArray[largest];
                this.MArray[largest] = temp;

                MaxHeapify( heapSize, largest );
            }
        }

        private void QuickSortRecursive(int left, int right) {
            if ( left < right ) {
                Sleep( this.MArray.Length );
                var q = Partition( left, right );
                QuickSortRecursive( left, q - 1 );
                QuickSortRecursive( q       + 1, right );
            }
        }

        private int Partition(int left, int right) {
            var pivot = this.MArray[right];
            int temp;
            var i = left;

            for ( var j = left; j < right; ++j )
                if ( this.MArray[j] <= pivot ) {
                    temp           = this.MArray[j];
                    this.MArray[j] = this.MArray[i];
                    this.MArray[i] = temp;
                    i++;
                    Sleep( this.MArray.Length );
                }

            this.MArray[right] = this.MArray[i];
            this.MArray[i]     = pivot;

            return i;
        }

        #endregion
    }

    internal interface ISorter {
        CounterArray<int> MArray { get; set; }
        void              Sort();
    }

    internal class CounterArray <T> {
        public CounterArray(ref T[] array) => this.InnerArray = array;

        public T this[int index] {
            get {
                this.Get?.Invoke( index );
                return this.InnerArray[index];
            }
            set {
                this.Set?.Invoke( index, value );
                this.InnerArray[index] = value;
            }
        }

        public int Length => this.InnerArray.Length;

        public T[] InnerArray { [DebuggerStepThrough] get; }

        public event Action<int>    Get;
        public event Action<int, T> Set;

        public T    GetNoEvent(int index)          => this.InnerArray[index];
        public void SetNoEvent(int index, T value) => this.InnerArray[index] = value;
    }
}
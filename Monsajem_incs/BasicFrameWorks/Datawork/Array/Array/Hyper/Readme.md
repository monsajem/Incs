# Hyper BaseArray Module Documentation

This document describes the `BaseArray.cs` file in the `Datawork/Array/Array/Hyper` folder of the Incs framework. It implements a segmented, hyper-dynamic array capable of handling very large sequences by splitting storage into multiple sub-arrays.

## 1. Class Hierarchy and Purpose

* **`ArrayHuge`**: A convenience subclass of `Array`, preconfigured with large default parameters (`MinCount=50000`, `Sub=100`, `MinSub=500`).
* **`Array`**: The core segmented array implementation, built on dynamic-size inner arrays.
* **`ArrayInstance`**: A helper class representing each segment, containing:

  * `int FromPos`: the starting global index of this segment.
  * `IArray<ArrayType> ar`: the underlying inner array segment.

`Array` extends `Base.IArray<ArrayType>` and uses a `DynamicSize.Array` of `ArrayInstance` to manage segments.

## 2. Configuration and Construction

### 2.1 Options (`MyOptions`)

* Exposes and sets a tuple `(MinCount, Sub, MinSub)`:

  * **`MinCount`**: base segment size.
  * **`Sub`**: number of segments threshold to trigger deeper nesting.
  * **`MinSub`**: minimum sub-segment size.
* On setting, computes:

  * `SubMinCount = MinCount / Sub` (falling back to zero if `MinSub` exceeds this).
  * Initializes `MakeInner` factory: either a fixed-size array or another hyper-filled `Array` for nested segmentation.
  * Sets buffer thresholds: `MaxLen = MinCount-1`, `MaxLen_Div2 = MaxLen/2`.
  * Lazily initializes the top-level `DynamicSize.Array`, inserting a single `ArrayInstance` beginning at `FromPos=0`.

### 2.2 Constructors

* **Parameterless**: defaults to `(500,0,0)` which resets to proper values internally.
* **Parameterized**: allows custom `(MinCount, Sub, MinSub)` and optionally an initial `ArrayType[]` to insert.

## 3. Indexer: `this[int Pos]`

Retrieves or sets an element at global index `Pos`:

1. Perform a binary search on the `DynamicSize.Array` of `ArrayInstance`s to locate the segment.
2. Calculate segment-local offset as `Pos - FromPos`.
3. Forward to the underlying segment’s indexer.

All lookup code is marked `[AggressiveInlining|Optimization]` for performance.

## 4. Insertion and Deletion

### 4.1 `Insert(ArrayType Value, int Position)`

* Finds the target segment via binary search.
* If segment is at full capacity (`Length == MinCount`):

  1. Splits the segment at `Position`:

     * Creates a new inner segment via `MakeInner()`.
     * Distributes elements between old and new segments.
  2. Inserts the new value in the appropriate segment.
* Otherwise, directly inserts into the found segment.
* Updates `FromPos` for all subsequent segments and increments global `Length`.

### 4.2 `DeleteByPosition(int Position)`

* Locates segment, deletes element at local offset.
* Decrements `FromPos` of subsequent segments.
* Calls `Optimization` on the affected segment index to merge or remove empty segments if needed.
* Decrements global `Length`.

## 5. Segment Optimization: `Optimization` Methods

Periodically rebalances segments to maintain efficient sizes:

* **Expansion**: Splits overly large segments by popping half of their elements into a new segment.
* **Contraction**: Merges underfilled segments into neighboring segments when combined length falls below half of `MaxLen`.
* **Removal**: Deletes empty segments entirely unless it’s the only one left.

These methods ensure segments remain within defined size thresholds.

## 6. Advanced Range Operations

* **`DeleteFrom(int from)`**: Removes all elements from global index `from` onward, adjusting segment contents or removing segments.
* **`AddFromTo(int From, Array Ar, int Ar_From, int Ar_Len)`**: Inserts a block of elements from another array at position `From`, handling segment splits and updating offsets.
* **`GetArrayFrom(int From, out int Ar_From, out int Ar_Len)`**: Retrieves a raw `System.Array` slice starting at `From` within a segment.
* **`GetArrayPos(int Ar_Pos, ...)`**: Retrieves entire segment `Ar_Pos` as a raw array slice.

## 7. Segmentation Strategy

* **Top-Level**: A dynamic-size array of `ArrayInstance` segments.
* **Inner Segments**: Either fixed-size or recursively nested hyper `Array` for deeper scaling.

This multi-level design allows the structure to efficiently manage extremely large collections by avoiding monolithic buffers.

---

*End of Hyper BaseArray Module documentation.*

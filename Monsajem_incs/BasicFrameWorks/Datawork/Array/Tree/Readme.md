# Tree-Based Array (`_Base.cs`) Documentation

This document explains the `_Base.cs` implementation of a tree-backed, indexable array using an AVL-balanced binary search tree in the Incs framework.

## 1. Overview

The `Array` class implements `IArray<ValueType>` and maintains elements in an AVL tree where in-order traversal represents array order. It provides O(log n) performance for insertions, deletions, and random access by index.

## 2. Node Structure

```csharp
public class Node {
  public ValueType Value;        // Stored element
  public Node Next, Before;      // Right and left child pointers
  public Node Holder;            // Parent pointer
  public bool IsNext;            // True if this node is the right child of its parent
  public int NextLen, BeforeLen; // Sizes of right and left subtrees (for indexing)
  public int NextDeep, BeforeDeep; // Depths of right and left subtrees (for balancing)
  public int Balance => NextDeep - BeforeDeep; // AVL balance factor
}
```

* `NextLen`/`BeforeLen` track subtree sizes to map indices to nodes.
* `NextDeep`/`BeforeDeep` and `Balance` enforce AVL height constraints.

## 3. Indexed Access

```csharp
public override ValueType this[int position] { get; set; }
```

* Uses `GetItem(position, out before, out next)` to traverse the tree: subtracts `BeforeLen+1` from `position` when moving right.
* Throws an exception if `position` is out of range.

## 4. Insertion

```csharp
public override void Insert(ValueType value, int position);
public override int BinaryInsert(ValueType value);
```

* **Insert(value, position)**:

  1. Locate predecessor (`before`) and successor (`next`) nodes via `GetItem`.
  2. Create and link a new `Node` between them.
  3. Update `NextLen`/`BeforeLen` and `NextDeep`/`BeforeDeep` up to the root.
  4. Call `FixBalance` to rebalance using rotations.
  5. Increment `Length` and invoke `ChangedNextSequence`.

* **BinaryInsert(value)**:

  * Performs a binary search for the correct sorted position, then inserts.

## 5. Deletion

```csharp
public override void DeleteByPosition(int position);
public override (int Index, ValueType Value) BinaryDelete(ValueType value);
```

* **DeleteByPosition(position)**:

  1. Find the target node with `GetItem`.
  2. Call `Drop(node)`:

     * If leaf: `DropFromHolder` unlinks it.
     * If one child: replace node with its child.
     * If two children: swap with in-order successor then drop.
  3. Call `FixWay` to rebalance upwards.
  4. Decrement `Length` and invoke `ChangedNextSequence`.

* **BinaryDelete(value)**:

  * Locate a node by key, drop it, and return its original index and value.

## 6. Balancing Logic

### FixBalance(Node node)

Walks up from the insertion or deletion point to the root, recalculating depths and performing single or double rotations via `MoveToHolder` whenever `Balance` exceeds ±1.

### MoveToHolder(Node node)

Executes the actual rotation:

* Re-links child and parent pointers.
* Updates `NextLen`, `BeforeLen`, `NextDeep`, and `BeforeDeep` for the involved nodes.
* Updates the tree root if necessary.

## 7. Enumeration

```csharp
public override IEnumerator GetEnumerator() {
  var node = GetItem(0, out _, out _);
  while (node != null) {
    yield return node.Value;
    node = node.FindNextSequnce();
  }
}
```

* Starts at the leftmost node and uses `FindNextSequnce()` for in-order traversal.

## 8. Debug Support (DEBUG builds only)

* **`ItemsForDebug`**: Auxiliary flat array used to verify tree contents.
* **`CheckBugs()`**: Recursively validates:

  * Subtree sizes and depths.
  * Parent/child pointer consistency.
  * No lost or duplicated nodes.
  * Match with `ItemsForDebug`.
  

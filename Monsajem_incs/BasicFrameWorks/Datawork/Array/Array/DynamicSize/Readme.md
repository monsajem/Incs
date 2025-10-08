# DynamicSize Array Module Documentation

This document describes the `Array_.cs` file in the `Datawork/Array/Array/DynamicSize` folder of the Incs framework. It implements a dynamic-size array with automatic buffer resizing based on usage.

## 1. Overview

The `Array` class extends a base array implementation to support dynamic resizing. Instead of a fixed capacity, the internal buffer grows or shrinks in configurable increments when elements are added or removed.

Key features:

* **Configurable margin (`MinCount`)** to control resize thresholds.
* **Automatic expansion** when the logical length exceeds the upper limit (`MaxLen`).
* **Automatic contraction** when the logical length falls below the lower limit (`MinLen`).
* **Implicit conversion** to a standard fixed-length array.

## 2. Core Properties and Fields

| Property / Field | Type     | Description                                                              |
| ---------------- | -------- | ------------------------------------------------------------------------ |
| `MinCount`       | `int`    | Number of elements above/below `Length` to trigger resize (default 500). |
| `MinLen`         | `int`    | `Length - MinCount`. Shrink when buffer length < `MinLen`.               |
| `MaxLen`         | `int`    | `Length + MinCount`. Expand when buffer length > `MaxLen`.               |
| `MyOptions`      | `object` | Overrides base options to configure `MinCount`.                          |

## 3. Resizing Mechanism

### 3.1 Initialization

* On setting `MyOptions`, if the internal array is uninitialized, it allocates a buffer of size `MaxLen = Length + MinCount`.
* Otherwise, it updates `MinLen` and `MaxLen` relative to the current `Length`.

### 3.2 Expansion (`AddLength`)

```csharp
protected override void AddLength(int count)
{
    Length += count;
    if (Length > MaxLen)
    {
        MaxLen = Length + MinCount;
        MinLen = Length - MinCount;
        Array.Resize(ref ar, MaxLen);
    }
}
```

* Increases `Length` by `count`.
* If `Length` exceeds `MaxLen`, recomputes `MaxLen` and `MinLen` and resizes the buffer.

### 3.3 Contraction (`DeleteFrom`)

```csharp
protected override void DeleteFrom(int from)
{
    Length = from;
    if (Length < MinLen)
    {
        MaxLen = Length + MinCount;
        MinLen = Length - MinCount;
        Array.Resize(ref ar, MaxLen);
    }
}
```

* Sets `Length` to `from` (removing trailing elements).
* If `Length` falls below `MinLen`, recomputes limits and shrinks the buffer.

## 4. Enumeration and Filtering

The class overrides `Browse` methods to filter or map elements while preserving dynamic-size behavior:

```csharp
public new Array Browse(Func<T, bool> predicate)
{
    var result = new Array(MinCount);
    foreach (var item in base.Browse(predicate))
        result.Add(item);
    return result;
}

public new Array Browse<R>(Func<T, R> selector)
{
    var result = new Array(MinCount);
    foreach (var item in base.Browse(selector))
        result.Add(item);
    return result;
}
```

* Both create a new dynamic array with the same `MinCount`.
* Insert matching items using the underlying `Add` logic.

## 5. Implicit Conversion

```csharp
public static implicit operator T[](Array ar)
{
    var fixedArray = new T[ar.Length];
    Array.Copy(ar.ar, fixedArray, ar.Length);
    return fixedArray;
}
```

* Converts the dynamic array to a standard fixed-size array of length `Length`.

## 6. Factory Method (`MakeSameNew`)

```csharp
protected override Array MakeSameNew()
{
    return new Array(MinCount);
}
```

* Creates a new instance of `Array` preserving the current `MinCount` setting.

---

*End of DynamicSize Array Module documentation.*

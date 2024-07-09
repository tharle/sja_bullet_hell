
using System.Collections.Generic;
using System.Numerics;

public static class Surcharge
{
   public static T[] Add<T>(this T[] _arr, T value)
    {
        T[] newArr;

        if (_arr == null) newArr = new T[1];
        else newArr = new T[_arr.Length + 1];

        _arr.CopyTo(newArr, 0);
        newArr[newArr.Length - 1] = value;

        return newArr;
    }
}

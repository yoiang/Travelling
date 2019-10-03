/* IntMinMaxRangeAttribute.cs
* based on code by Eddie Cameron – For the public domain
* —————————-
* Use a MinMaxRange class to replace twin float range values (eg: float minSpeed, maxSpeed; becomes MinMaxRange speed)
* Apply a [MinMaxRange( minLimit, maxLimit )] attribute to a MinMaxRange instance to control the limits and to show a
* slider in the inspector
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IntMinMaxRangeAttribute : PropertyAttribute
{
public int minLimit, maxLimit;

public IntMinMaxRangeAttribute( int minLimit, int maxLimit )
{
this.minLimit = minLimit;
this.maxLimit = maxLimit;
}
}

[System.Serializable]
public class IntMinMaxRange
{
public int rangeStart, rangeEnd;
public IntMinMaxRange(int rangeStart, int rangeEnd) {
    this.rangeStart = rangeStart;
    this.rangeEnd = rangeEnd;
}

public int GetRandomValue()
{
return Random.Range( rangeStart, rangeEnd );
}
}
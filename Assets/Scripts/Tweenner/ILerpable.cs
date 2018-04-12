using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


interface ILerpable<T>
{
    T Lerp(float value);
}


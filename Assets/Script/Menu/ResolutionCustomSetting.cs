using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionCustomSetting
{
    public int height;
    public int width;

    public ResolutionCustomSetting(int height, int width)
    {
        this.height = height;
        this.width = width;
    }

    public override string ToString()
    {
        return width.ToString() + " x " + height.ToString();
    }

    public override bool Equals(object obj)
    {
        ResolutionCustomSetting other = obj as ResolutionCustomSetting;

        return (height ==  other.height && width == other.width);
    }

    public override int GetHashCode()
    {
        return height * 2 + width * 2;
    }
}


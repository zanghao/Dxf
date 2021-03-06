﻿// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;

namespace IxMilia.Dxf
{
    public partial class DxfLineType : IDxfItemInternal
    {
        IEnumerable<DxfPointer> IDxfItemInternal.GetPointers()
        {
            return Elements.SelectMany(e => ((IDxfItemInternal)e).GetPointers());
        }

        IEnumerable<IDxfItemInternal> IDxfItemInternal.GetChildItems()
        {
            return Elements.Select(e => (IDxfItemInternal)e);
        }

        internal override void AddValuePairs(List<DxfCodePair> pairs, DxfAcadVersion version, bool outputHandles)
        {
            if (version >= DxfAcadVersion.R13)
            {
                pairs.Add(new DxfCodePair(100, AcDbText));
            }

            pairs.Add(new DxfCodePair(2, Name));
            pairs.Add(new DxfCodePair(70, (short)StandardFlags));
            pairs.Add(new DxfCodePair(3, (Description)));
            pairs.Add(new DxfCodePair(72, (short)(AlignmentCode)));
            pairs.Add(new DxfCodePair(73, (short)Elements.Count));
            pairs.Add(new DxfCodePair(40, (TotalPatternLength)));
            foreach (var element in Elements)
            {
                element.AddValuePairs(pairs);
            }

            if (XData != null)
            {
                XData.AddValuePairs(pairs, version, outputHandles);
            }
        }

        internal static DxfLineType FromBuffer(DxfCodePairBufferReader buffer)
        {
            var item = new DxfLineType();
            while (buffer.ItemsRemain)
            {
                var pair = buffer.Peek();
                if (pair.Code == 0)
                {
                    break;
                }

                buffer.Advance();
                switch (pair.Code)
                {
                    case 70:
                        item.StandardFlags = pair.ShortValue;
                        break;
                    case DxfCodePairGroup.GroupCodeNumber:
                        var groupName = DxfCodePairGroup.GetGroupName(pair.StringValue);
                        item.ExtensionDataGroups.Add(DxfCodePairGroup.FromBuffer(buffer, groupName));
                        break;
                    case 3:
                        item.Description = (pair.StringValue);
                        break;
                    case 72:
                        item.AlignmentCode = (pair.ShortValue);
                        break;
                    case 73:
                        item._elementCount = (pair.ShortValue);
                        break;
                    case 40:
                        item.TotalPatternLength = (pair.DoubleValue);
                        break;
                    case 49:
                        // start a new element
                        item.Elements.Add(new DxfLineTypeElement() { DashDotSpaceLength = pair.DoubleValue });
                        break;
                    case 74:
                        // add to last element
                        if (item.Elements.Count > 0)
                        {
                            item.Elements.Last().ComplexFlags = pair.ShortValue;
                        }
                        break;
                    case 75:
                        // add to last element
                        if (item.Elements.Count > 0)
                        {
                            item.Elements.Last().ShapeNumber = pair.ShortValue;
                        }
                        break;
                    case 340:
                        // add to last element
                        if (item.Elements.Count > 0)
                        {
                            item.Elements.Last().StylePointer.Handle = UIntHandle(pair.StringValue);
                        }
                        break;
                    case 46:
                        // add to last element
                        if (item.Elements.Count > 0)
                        {
                            item.Elements.Last().ScaleValues.Add(pair.DoubleValue);
                        }
                        break;
                    case 50:
                        // add to last element
                        if (item.Elements.Count > 0)
                        {
                            item.Elements.Last().RotationAngle = pair.DoubleValue;
                        }
                        break;
                    case 44:
                        // add to last element, start a new offset value
                        if (item.Elements.Count > 0)
                        {
                            item.Elements.Last().Offsets.Add(new DxfVector(pair.DoubleValue, 0.0, 0.0));
                        }
                        break;
                    case 45:
                        // add to last element, add to last offset value
                        if (item.Elements.Count > 0 && item.Elements.Last().Offsets.Count > 0)
                        {
                            item.Elements.Last().Offsets.Last().Y = pair.DoubleValue;
                        }
                        break;
                    case 9:
                        // add to last element
                        if (item.Elements.Count > 0)
                        {
                            item.Elements.Last().TextString = pair.StringValue;
                        }
                        break;
                    case (int)DxfXDataType.ApplicationName:
                        item.XData = DxfXData.FromBuffer(buffer, pair.StringValue);
                        break;
                    default:
                        item.TrySetPair(pair);
                        break;
                }
            }

            return item;
        }
    }
}

// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

// The contents of this file are automatically generated by a tool, and should not be directly modified.

using System;
using System.Collections.Generic;
using System.Linq;
using IxMilia.Dxf.Collections;

namespace IxMilia.Dxf.Entities
{
    /// <summary>
    /// DxfRText class
    /// </summary>
    public partial class DxfRText : DxfEntity
    {
        public override DxfEntityType EntityType { get { return DxfEntityType.RText; } }
        protected override DxfAcadVersion MinVersion { get { return DxfAcadVersion.R2000; } }
        protected override DxfAcadVersion MaxVersion { get { return DxfAcadVersion.R2000; } }
        public DxfPoint InsertionPoint { get; set; }
        public DxfVector ExtrusionDirection { get; set; }
        public double RotationAngle { get; set; }
        public double TextHeight { get; set; }
        public string TextStyle { get; set; }
        public int TypeFlags { get; set; }
        public string Contents { get; set; }

        // TypeFlags flags

        public bool IsExpression
        {
            get { return DxfHelpers.GetFlag(TypeFlags, 1); }
            set
            {
                var flags = TypeFlags;
                DxfHelpers.SetFlag(value, ref flags, 1);
                TypeFlags = flags;
            }
        }

        public bool IsInlineMTextSequencesEnabled
        {
            get { return DxfHelpers.GetFlag(TypeFlags, 2); }
            set
            {
                var flags = TypeFlags;
                DxfHelpers.SetFlag(value, ref flags, 2);
                TypeFlags = flags;
            }
        }

        public DxfRText()
            : base()
        {
        }

        protected override void Initialize()
        {
            base.Initialize();
            this.InsertionPoint = DxfPoint.Origin;
            this.ExtrusionDirection = DxfVector.ZAxis;
            this.RotationAngle = 0.0;
            this.TextHeight = 0.0;
            this.TextStyle = "STANDARD";
            this.TypeFlags = 0;
            this.Contents = null;
        }

        protected override void AddValuePairs(List<DxfCodePair> pairs, DxfAcadVersion version, bool outputHandles)
        {
            base.AddValuePairs(pairs, version, outputHandles);
            pairs.Add(new DxfCodePair(100, "RText"));
            pairs.Add(new DxfCodePair(10, InsertionPoint?.X ?? default(double)));
            pairs.Add(new DxfCodePair(20, InsertionPoint?.Y ?? default(double)));
            pairs.Add(new DxfCodePair(30, InsertionPoint?.Z ?? default(double)));
            if (this.ExtrusionDirection != DxfVector.ZAxis)
            {
                pairs.Add(new DxfCodePair(210, ExtrusionDirection?.X ?? default(double)));
                pairs.Add(new DxfCodePair(220, ExtrusionDirection?.Y ?? default(double)));
                pairs.Add(new DxfCodePair(230, ExtrusionDirection?.Z ?? default(double)));
            }

            pairs.Add(new DxfCodePair(50, (this.RotationAngle)));
            pairs.Add(new DxfCodePair(40, (this.TextHeight)));
            pairs.Add(new DxfCodePair(7, (this.TextStyle)));
            pairs.Add(new DxfCodePair(70, (short)(this.TypeFlags)));
            pairs.Add(new DxfCodePair(1, (this.Contents)));
        }

        internal override bool TrySetPair(DxfCodePair pair)
        {
            switch (pair.Code)
            {
                case 1:
                    this.Contents = (pair.StringValue);
                    break;
                case 7:
                    this.TextStyle = (pair.StringValue);
                    break;
                case 10:
                    this.InsertionPoint.X = pair.DoubleValue;
                    break;
                case 20:
                    this.InsertionPoint.Y = pair.DoubleValue;
                    break;
                case 30:
                    this.InsertionPoint.Z = pair.DoubleValue;
                    break;
                case 40:
                    this.TextHeight = (pair.DoubleValue);
                    break;
                case 50:
                    this.RotationAngle = (pair.DoubleValue);
                    break;
                case 70:
                    this.TypeFlags = (int)(pair.ShortValue);
                    break;
                case 210:
                    this.ExtrusionDirection.X = pair.DoubleValue;
                    break;
                case 220:
                    this.ExtrusionDirection.Y = pair.DoubleValue;
                    break;
                case 230:
                    this.ExtrusionDirection.Z = pair.DoubleValue;
                    break;
                default:
                    return base.TrySetPair(pair);
            }

            return true;
        }
    }
}
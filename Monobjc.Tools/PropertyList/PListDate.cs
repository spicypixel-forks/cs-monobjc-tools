//
// This file is part of Monobjc, a .NET/Objective-C bridge
// Copyright (C) 2007-2013 - Laurent Etiemble
//
// Monobjc is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// any later version.
//
// Monobjc is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with Monobjc.  If not, see <http://www.gnu.org/licenses/>.
//
using System;
using System.Globalization;
using System.Xml;

namespace Monobjc.Tools.PropertyList
{
    /// <summary>
    ///   Represent a timestamp value in PList file.
    /// </summary>
    public class PListDate : PListItem<DateTime>
    {
        private const String DATEFORMAT = "yyyy-mm-ddThh:mm:ssZ";

        /// <summary>
        ///   Initializes a new instance of the <see cref = "PListDate" /> class.
        /// </summary>
        public PListDate() {}

        /// <summary>
        ///   Initializes a new instance of the <see cref = "PListDate" /> class.
        /// </summary>
        /// <param name = "value">The value.</param>
        public PListDate(DateTime value) : base(value) {}

        /// <summary>
        ///   Sets the value of this instance.
        /// </summary>
        /// <param name = "value">The value.</param>
        public override void SetValue(string value)
        {
            DateTime val;
            if (DateTime.TryParse(value, out val))
            {
                this.Value = val;
            }
        }

        /// <summary>
        ///   Writes this instance to Xml stream.
        /// </summary>
        /// <param name = "writer">The writer.</param>
        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("date", this.Value.ToUniversalTime().ToString(DATEFORMAT, CultureInfo.InvariantCulture));
        }

        /// <summary>
        ///   Performs an implicit conversion from <see cref = "Monobjc.Tools.PropertyList.PListDate" /> to <see cref = "System.DateTime" />.
        /// </summary>
        /// <param name = "value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator DateTime(PListDate value)
        {
            return value.Value;
        }
    }
}
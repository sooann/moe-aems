﻿
using System;
namespace MvcDynamicForms
{
    /// <summary>
    /// Stores arbitrary data.
    /// </summary>
    [Serializable]
    public class DataItem
    {
        public DataItem(object value, bool clientSide)
        {
            Value = value;
            ClientSide = clientSide;
        }

        /// <summary>
        /// The stored object.
        /// </summary>
        public object Value { get; set; }
        /// <summary>
        /// Whether to render the data.
        /// </summary>
        public bool ClientSide { get; set; }
    }
}
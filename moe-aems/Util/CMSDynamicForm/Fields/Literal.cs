﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace MvcDynamicForms.Fields
{
    /// <summary>
    /// Represents html to be rendered on the form.
    /// </summary>
    [Serializable]
    public class Literal : Field
    {
        /// <summary>
        /// The html to be rendered on the form.
        /// </summary>
        public string Html { get; set; }

        public override string RenderHtml()
        {
            var html = new StringBuilder(Template);
            html.Replace(PlaceHolders.Literal, Html);

            // wrapper id
            html.Replace(PlaceHolders.FieldWrapperId, GetWrapperId());

            return html.ToString();
        }

        protected override string BuildDefaultTemplate()
        {
            return PlaceHolders.Literal;
            
        }
    }
}

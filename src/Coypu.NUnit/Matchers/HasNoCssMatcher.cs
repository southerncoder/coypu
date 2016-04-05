﻿using System.Text.RegularExpressions;
using NUnit.Framework.Constraints;

namespace Coypu.NUnit.Matchers
{
    public class HasNoCssMatcher : Constraint
    {
        private readonly string _expectedCss;
        private readonly Options _options;
        private string _actualContent;
        private Regex textPattern;
        private string exactText;

        public HasNoCssMatcher(string expectedCss, Options options)
        {
            _expectedCss = expectedCss;
            _options = options;
        }

        public HasNoCssMatcher(string expectedCss, Regex textPattern, Options options)
            : this(expectedCss, options)
        {
            this.textPattern = textPattern;
        }

        public HasNoCssMatcher(string expectedCss, string exactText, Options options)
            : this(expectedCss, options)
        {
            this.exactText = exactText;
        }

        public override ConstraintResult ApplyTo<TActual>(TActual actual)
        {
            var scope = ((Scope)actual);

            bool hasNoCss;
            if (exactText != null)
                hasNoCss = scope.FindCss(_expectedCss, exactText, _options).Missing();
            else if (textPattern != null)
                hasNoCss = scope.FindCss(_expectedCss, textPattern, _options).Missing();
            else
                hasNoCss = scope.FindCss(_expectedCss, _options).Missing();

            if (!hasNoCss)
                _actualContent = scope.Now().InnerHTML;

            return new ConstraintResult(this, actual, hasNoCss);
        }
    }
}
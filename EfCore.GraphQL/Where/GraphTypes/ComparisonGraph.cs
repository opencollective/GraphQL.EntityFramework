﻿using System;
using System.Collections.Generic;
using GraphQL.Language.AST;
using GraphQL.Types;

namespace EfCoreGraphQL
{
    public class ComparisonGraph : EnumerationGraphType<Comparison>
    {
        static Dictionary<string, Comparison> comparisons = new Dictionary<string, Comparison>();

        static ComparisonGraph()
        {
            Add(Comparison.Equal, "==");
            Add(Comparison.NotEqual, "!=");
            Add(Comparison.GreaterThan, ">");
            Add(Comparison.GreaterThanOrEqual, ">=");
            Add(Comparison.LessThan, "<");
            Add(Comparison.LessThanOrEqual, "<=");
        }

        public override object ParseLiteral(IValue value)
        {
            var literal = base.ParseLiteral(value);
            if (literal != null)
            {
                return literal;
            }

            if (value.Value is string stringValue)
            {
                if (
                    Enum.TryParse(stringValue, true, out Comparison comparison) ||
                    comparisons.TryGetValue(stringValue, out comparison)
                )
                {
                    return comparison;
                }
            }

            return null;
        }

        static void Add(Comparison comparison, string name)
        {
            comparisons[name] = comparison;
        }
    }
}
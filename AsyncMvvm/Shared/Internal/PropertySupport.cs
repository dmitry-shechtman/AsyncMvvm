// Copyright (c) Microsoft Corporation. All rights reserved. See LICENSE in the project root for license information.

using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Ditto.AsyncMvvm.Internal
{
    internal static class PropertySupport
    {
        public static string ExtractPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression == null)
                throw new ArgumentNullException("propertyExpression");
            var memberExpression = propertyExpression.Body as MemberExpression;
            if (memberExpression == null)
                throw new ArgumentException("The expression is not a member access expression.", "propertyExpression");
            var property = memberExpression.Member as PropertyInfo;
            if (property == null)
                throw new ArgumentException("The member access expression does not access a property.", "propertyExpression");
            var getMethod = property.GetGetMethod();
            if (getMethod.IsStatic)
                throw new ArgumentException("The referenced property is a static property.", "propertyExpression");
            return property.Name;
        }
    }
}

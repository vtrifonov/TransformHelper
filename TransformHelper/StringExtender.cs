using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TransformHelper
{
    public static class StringExtender
    {
        static Regex regexSplitByCapitalLetters = new Regex(@"
                (?<=[A-Z])(?=[A-Z][a-z]) |
                 (?<=[^A-Z])(?=[A-Z]) |
                 (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

        public static string SplitByCapitalLetters(this string instance)
        {
            return regexSplitByCapitalLetters.Replace(instance, " ");
        }

        /// <summary>
        /// Removes the first symbols if they are the same as the stringToRemove parameter
        /// </summary>
        /// <param name="instance">string Instance</param>
        /// <param name="stringToRemove">string pattern to check</param>
        /// <returns>string</returns>
        public static string RemoveStartingString(this string instance, string stringToRemove, bool ignoreCase = true)
        {
            if (!string.IsNullOrEmpty(instance))
            {
                if (instance.StartsWith(stringToRemove, ignoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.CurrentCulture))
                {
                    return instance.Remove(0, stringToRemove.Length);
                }
            }

            return instance;
        }

        /// <summary>
        /// Removes the last symbols if they are the same as the stringToRemove parameter
        /// </summary>
        /// <param name="instance">string Instance</param>
        /// <param name="stringToRemove">string pattern to check</param>
        /// <returns>string</returns>
        public static string RemoveEndingString(this string instance, string stringToRemove, bool ignoreCase = true)
        {
            if (!string.IsNullOrEmpty(instance))
            {
                if (instance.EndsWith(stringToRemove, ignoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.CurrentCulture))
                {
                    return instance.Remove(instance.Length - stringToRemove.Length, stringToRemove.Length);
                }
            }
            return instance;
        }

        public static string GetAfterLastDot(this string instance)
        {
            return instance.GetAfterLastChar('.');
        }

        public static string GetAfterLastChar(this string instance, char character)
        {
            int lastIndex = instance.LastIndexOf(character);
            if (lastIndex > -1)
            {
                return instance.Remove(0, lastIndex + 1);
            }
            else
            {
                return instance;
            }
        }

        public static string GetBeforeLastChar(this string instance, char character)
        {
            int lastIndex = instance.LastIndexOf(character);
            if (lastIndex > -1)
            {
                return instance.Remove(lastIndex, instance.Length - lastIndex);
            }
            else
            {
                return instance;
            }
        }

        public static bool EndsWithEnumerable(this string value, StringComparison stringComparison, IEnumerable<string> ends)
        {
            foreach (string end in ends)
            {
                if (value.EndsWith(end, stringComparison))
                {
                    return true;
                }
            }
            return false;
        }

        public static string ReplaceSafe(this string instance, string oldValue, string newValue)
        {
            if (string.IsNullOrWhiteSpace(instance))
            {
                return instance;
            }
            else
            {
                return instance.Replace(oldValue, newValue);
            }
        }

        public static string FormatWith(this string instance, params object[] args)
        {
            if (string.IsNullOrWhiteSpace(instance))
            {
                throw new ArgumentNullException();
            }

            return string.Format(instance, args);
        }

        /// <summary>
        /// Determines whether [is equal to] [the specified instance] using StringComparison.InvariantCultureIgnoreCase
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="value">The other string.</param>
        /// <returns>
        ///   <c>true</c> if [is equal to] [the specified instance]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsEqualTo(this string instance, string value)
        {
            if (string.IsNullOrWhiteSpace(instance))
            {
                return string.IsNullOrWhiteSpace(value);
            }
            else
            {
                return instance.Equals(value, StringComparison.InvariantCultureIgnoreCase);
            }
        }

        /// <summary>
        /// Determines whether [is starting with] [the specified instance] using StringComparison.InvariantCultureIgnoreCase.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="value">The other string.</param>
        /// <returns>
        ///   <c>true</c> if [is starting with] [the specified instance]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsStartingWith(this string instance, string value)
        {
            return instance.StartsWith(value, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Determines whether [is ending with] [the specified instance] using StringComparison.InvariantCultureIgnoreCase.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="value">The other string.</param>
        /// <returns>
        ///   <c>true</c> if [is ending with] [the specified instance]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsEndingWith(this string instance, string value)
        {
            return instance.EndsWith(value, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}

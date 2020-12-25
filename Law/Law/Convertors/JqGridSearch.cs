//using System;
//using System.Collections.Generic;
//using System.Collections.Specialized;
//using System.Linq;
//using System.Text.RegularExpressions;
//using System.Web;

//namespace Law.Convertors
//{
//    public class JqGridSearch
//    {
//        private static readonly Dictionary<string, string> _whereOperation =
//                    new Dictionary<string, string>
//    {
//                 {"in" , " {0} = @{1} "},//is in
//                        {"eq" , " {0} = @{1} "},
//                        {"ni" , " {0} != @{1} "},//is not in
//                        {"ne" , " {0} != @{1} "},
//                        {"lt" , " {0} < @{1} "},
//                        {"le" , " {0} <= @{1} "},
//                        {"gt" , " {0} > @{1} "},
//                        {"ge" , " {0} >= @{1} "},
//                        {"bw" , " {0}.StartsWith(@{1}) "},//begins with
//                        {"bn" , " !{0}.StartsWith(@{1}) "},//does not begin with
//                        {"ew" , " {0}.EndsWith(@{1}) "},//ends with
//                        {"en" , " !{0}.EndsWith(@{1}) "},//does not end with
//                        {"cn" , " {0}.Contains(@{1}) "},//contains
//                        {"nc" , " !{0}.Contains(@{1}) "}//does not contain
//                    };


//        private static readonly Dictionary<string, string> _validOperators =
//                     new Dictionary<string, string>
//     {
//     {"Object",""},
//     {"Boolean","eq:ne:"},
//     {"Char",""},
//     {"String","eq:ne:lt:le:gt:ge:bw:bn:cn:nc:"},
//     {"SByte",""},
//     {"Byte","eq:ne:lt:le:gt:ge:"},
//     {"Int16","eq:ne:lt:le:gt:ge:"},
//     {"UInt16",""},
//     {"Int32","eq:ne:lt:le:gt:ge:"},
//     {"UInt32",""},
//     {"Nullable`1","eq:cn:"},
//     {"Int64","eq:ne:lt:le:gt:ge:"},
//     {"UInt64",""},
//     {"Decimal","eq:ne:lt:le:gt:ge:"},
//     {"Single","eq:ne:lt:le:gt:ge:"},
//     {"Double","eq:ne:lt:le:gt:ge:"},
//     {"DateTime","eq:ne:lt:le:gt:ge:"},
//     {"TimeSpan",""},
//     {"Guid",""}
//     };

//        private int _parameterIndex;

//        public IQueryable<T> ApplyFilter<T>(IQueryable<T> query, bool _search, string searchField, string searchString,
//                                        string searchOper, string filters, NameValueCollection form)
//        {
//            if (!_search)
//                return query;

//            if (!string.IsNullOrWhiteSpace(searchString) &&
//                !string.IsNullOrWhiteSpace(searchOper) &&
//                !string.IsNullOrWhiteSpace(searchField))
//            {

//                searchField = searchField.Substring(0, 40);
//                searchString = searchString.Substring(0, 60);

//                var regexForSearchField = new Regex(@"[^a-zA-Z0-9ا-ی\s.,]");
//                var regexForFieldName = new Regex(@"[^a-zA-Z0-9,.]");
//                searchField = (regexForFieldName.Replace(searchField, "")).Replace(" ", "");
//                searchString = regexForSearchField.Replace(searchString, "");
//                var searchOptions = new string[] { "eq", "lt", "le", "gt", "ge", "bw", "bn", "in", "ni", "ew", "en", "cn", "nc" };
//                searchOper = searchOptions.Contains(searchOper) ? searchOper : "eq";

//                return manageSingleFieldSearch(query, searchField, searchString, searchOper);
//            }

//            return !string.IsNullOrWhiteSpace(filters) ?
//                    manageMultiFieldSearch(query, filters) : manageToolbarSearch(query, form);
//        }

//        private Tuple<string, object> getPredicate<T>(string searchField, string searchOper, string searchValue)
//        {
//            if (string.IsNullOrWhiteSpace(searchValue))
//                return null;

//            var type = typeof(T).FindFieldType(searchField);
//            if (type == null)
//                throw new InvalidOperationException(searchField + " is not defined.");

//            if (!_validOperators[type.Name].Contains(searchOper + ":"))
//            {
//                return null;
//            }

//            var resultValue = Convert.ChangeType(searchValue, type);
//            return new Tuple<string, object>(getSearchOperator(searchOper, searchField), resultValue);
//        }

//        private string getSearchOperator(string ruleSearchOperator, string searchField)
//        {
//            string whereOperation;
//            if (!_whereOperation.TryGetValue(ruleSearchOperator, out whereOperation))
//            {
//                throw new NotSupportedException(string.Format("{0} is not supported.", ruleSearchOperator));
//            }

//            var searchOperator = string.Format(whereOperation, searchField, _parameterIndex);
//            _parameterIndex++;
//            return searchOperator;
//        }

//        public IQueryable<T> manageMultiFieldSearch<T>(IQueryable<T> query, string filters)
//        {
//            var filtersArray = JsonConvert.DeserializeObject<SearchFilter>(filters);
//            var groupOperator = filtersArray.groupOp;
//            if (filtersArray.rules == null)
//                return query;

//            var valuesList = new List<object>();
//            var filterExpression = String.Empty;
//            foreach (var rule in filtersArray.rules)
//            {
//                var predicate = getPredicate<T>(rule.field, rule.op, rule.data);
//                if (predicate == null)
//                    continue;

//                valuesList.Add(predicate.Item2);
//                filterExpression = filterExpression + predicate.Item1 + " " + groupOperator + " ";
//            }

//            if (string.IsNullOrWhiteSpace(filterExpression))
//                return query;

//            filterExpression = filterExpression.Remove(filterExpression.Length - groupOperator.Length - 2);
//            query = query.Where(filterExpression, valuesList.ToArray());
//            return query;
//        }

//        private IQueryable<T> manageSingleFieldSearch<T>(IQueryable<T> query, string searchField, string searchString,
//            string searchOper)
//        {
//            var predicate = getPredicate<T>(searchField, searchOper, searchString);
//            if (predicate != null)
//                query = query.Where(predicate.Item1, new[] { predicate.Item2 });
//            return query;
//        }

//        private IQueryable<T> manageToolbarSearch<T>(IQueryable<T> query, NameValueCollection form)
//        {
//            var filterExpression = String.Empty;
//            var valuesList = new List<object>();
//            foreach (var postDataKey in form.AllKeys)
//            {
//                if (!postDataKey.Equals("nd") && !postDataKey.Equals("sidx")
//                    && !postDataKey.Equals("sord") && !postDataKey.Equals("page")
//                    && !postDataKey.Equals("rows") && !postDataKey.Equals("_search")
//                    && form[postDataKey] != null)
//                {
//                    var predicate = getPredicate<T>(postDataKey, "eq", form[postDataKey]);
//                    if (predicate == null)
//                        continue;

//                    valuesList.Add(predicate.Item2);
//                    filterExpression = filterExpression + predicate.Item1 + " And ";
//                }
//            }

//            if (string.IsNullOrWhiteSpace(filterExpression))
//                return query;

//            filterExpression = filterExpression.Remove(filterExpression.Length - 5);
//            query = query.Where(filterExpression, valuesList.ToArray());
//            return query;
//        }
//    }
//}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using moe_aems.Models;
using System.Linq.Expressions;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace moe_aems.Util
{
    class JQGridSvc
    {
        public JQGridResponse<T> Query<T>(Entities entity, HttpRequestMessage Request)
        {
            dynamic instance = Activator.CreateInstance(typeof(T));
            
            int PageSize = 0;
            int Page = 0;
            string SortColumn = "";
            string SortOrder = "";
            bool search = false;
            string SingleSearch_Field = "";
            string SingleSearch_Value = "";
            string SingleSearch_Oper = "";
            MethodCallExpression WhereClause = null;
            
            var model = entity.Set(typeof(T)).OfType<T>();

            if (Request.GetQueryNameValuePairs().Where(x => x.Key == "rows").First().Value != "")
            {
                PageSize = int.Parse(Request.GetQueryNameValuePairs().Where(x => x.Key == "rows").First().Value);
            }

            if (Request.GetQueryNameValuePairs().Where(x => x.Key == "page").First().Value != "")
            {
                Page = int.Parse(Request.GetQueryNameValuePairs().Where(x => x.Key == "page").First().Value);
            }

            if (Request.GetQueryNameValuePairs().Where(x => x.Key == "sidx").First().Value != "")
            {
                SortColumn = Request.GetQueryNameValuePairs().Where(x => x.Key == "sidx").First().Value;
            }

            if (Request.GetQueryNameValuePairs().Where(x => x.Key == "sord").First().Value != "")
            {
                SortOrder = Request.GetQueryNameValuePairs().Where(x => x.Key == "sord").First().Value;
            }

            Type type = typeof(T);
            ParameterExpression pe = Expression.Parameter(type, "p");

            //search options
            if (Request.GetQueryNameValuePairs().Where(x => x.Key == "_search").First().Value == "true")
            {
                search = true;
                Expression Queryfilter = null;

                if (Request.GetQueryNameValuePairs().Where(x => x.Key == "searchField").First().Value != "")
                {
                    //single search
                    SingleSearch_Field = Request.GetQueryNameValuePairs().Where(x => x.Key == "searchField").First().Value;
                    SingleSearch_Value = Request.GetQueryNameValuePairs().Where(x => x.Key == "searchString").First().Value;
                    SingleSearch_Oper = Request.GetQueryNameValuePairs().Where(x => x.Key == "searchOper").First().Value;
                    
                    Queryfilter = FilterExpression<T>(pe,SingleSearch_Oper,SingleSearch_Field,SingleSearch_Value);
                }
                else
                {
                    //get Search options
                    string searchstring = Request.GetQueryNameValuePairs().Where(x => x.Key == "filters").First().Value;

                    JQGridSearch objJSON = new JQGridSearch();
                    JObject jobject = JObject.Parse(searchstring);
                    objJSON.groupOp = (string)jobject["groupOp"];
                    objJSON.rules = JsonConvert.DeserializeObject<List<JQGridSearchRule>>(jobject["rules"].ToString());
                    if (jobject["groups"]!=null)
                    {
                        objJSON.groups = JsonConvert.DeserializeObject<List<JQGridSearch>>(jobject["groups"].ToString());
                    }
                    else
                    {
                        objJSON.groups = new List<JQGridSearch>();
                    }
                    //groups = (List<JQGridSearch>)jobject.Children()["groups"].Values().ToList();

                    Queryfilter = GenerateFilter<T>(pe,objJSON);                    
                }

                WhereClause = Expression.Call(
                    typeof(Queryable),
                    "Where",
                    new Type[] { model.ElementType },
                    model.Expression,
                    Expression.Lambda<Func<T, bool>>(Queryfilter, new ParameterExpression[] { pe }));

                model = model.Provider.CreateQuery<T>(WhereClause);

            }

            JQGridResponse<T> response = new JQGridResponse<T>();
            response.page = Page;
            response.records = model.LongCount();
            response.total = (response.records % PageSize) == 0 ? response.records / PageSize : (response.records / PageSize) + 1;
            
            if (PageSize != 0 && Page != 0 && SortColumn!="" && SortOrder!="")
            {
                if (search) {
                    model = model.Provider.CreateQuery<T>(WhereClause);
                    response.rows = SortBy<T>(model,SortColumn,SortOrder).Skip((Page - 1) * PageSize).Take(PageSize);   
                } else {
                    response.rows = SortBy<T>(model,SortColumn,SortOrder).Skip((Page - 1) * PageSize).Take(PageSize);   
                }
            }
            else
            {
                if (search) {
                    response.rows = model.Provider.CreateQuery<T>(WhereClause);
                } else {
                    response.rows = model;
                }
            }

            return response;
        }

        private IQueryable<T> SortBy<T>(IQueryable<T> source, string propertyName, string sortOrder)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            // DataSource control passes the sort parameter with a direction
            // if the direction is descending          
            
            if (String.IsNullOrEmpty(propertyName))
            {
                return source;
            }

            ParameterExpression parameter = Expression.Parameter(source.ElementType, String.Empty);
            MemberExpression property = Expression.Property(parameter, propertyName);
            LambdaExpression lambda = Expression.Lambda(property, parameter);

            string methodName = (sortOrder.ToUpper() != "DESC") ? "OrderBy" : "OrderByDescending";

            Expression methodCallExpression = Expression.Call(typeof(Queryable), methodName,
                                                new Type[] { source.ElementType, property.Type },
                                                source.Expression, Expression.Quote(lambda));

            return source.Provider.CreateQuery<T>(methodCallExpression);
        }

        private Expression GenerateFilter<T>(ParameterExpression pe, JQGridSearch search)
        {
            //multiple search query
            Expression Query = null;

            if (search.groups.Count() > 0)
            {
                //multiple group query
                foreach (JQGridSearch item in search.groups)
                {
                    if (Query == null)
                    {
                        Query = GenerateFilter<T>(pe, item);
                    }
                    else
                    {
                        if (search.groupOp == "AND")
                        {
                            Query = Expression.AndAlso(Query, GenerateFilter<T>(pe, item));
                        }
                        else
                        {
                            Query = Expression.OrElse(Query, GenerateFilter<T>(pe, item));
                        }
                    }
                }
            }
            else if (search.rules.Count() > 0)
            {
                foreach (JQGridSearchRule item in search.rules)
                {
                    if (Query == null)
                    {
                        Query = FilterExpression<T>(pe, item);
                    }
                    else
                    {
                        if (search.groupOp == "AND")
                        {
                            Query = Expression.AndAlso(Query, FilterExpression<T>(pe, item));
                        }
                        else
                        {
                            Query = Expression.OrElse(Query, FilterExpression<T>(pe, item));
                        }
                    }
                }

            }

            return Query;
        }

        private Expression FilterExpression<T>(ParameterExpression pe, JQGridSearchRule searchrule)
        {
            return FilterExpression<T>(pe, searchrule.op, searchrule.field, searchrule.data);
        }

        private Expression FilterExpression<T>(ParameterExpression pe, string filterType, string field, object value)
        {

            Type type = typeof(T);
            //ParameterExpression pe = Expression.Parameter(type, "p");
            Expression left = Expression.Property(pe, field);
            Expression right = Expression.Constant(value, typeof(string));
            
            Expression where = null;
            
            switch (filterType)
            {
                case "bw":
                    where = Expression.Call(left, typeof(string).GetMethod("StartsWith", new Type[] {typeof (string)}), right);
                    break;
                case "bn":
                    where = Expression.Not(Expression.Call(left, typeof(string).GetMethod("StartsWith", new Type[] {typeof (string)}), right));
                    break;
                case "cn":
                    where = Expression.Call(left, typeof(string).GetMethod("Contains", new Type[] {typeof (string)}), right);
                    break;
                case "nc":
                    where = Expression.Not(Expression.Call(left, typeof(string).GetMethod("Contains", new Type[] {typeof (string)}), right));
                    break;
                case "ew":
                    where = Expression.Call(left, typeof(string).GetMethod("EndsWith", new Type[] {typeof (string)}), right);
                    break;
                case "en":
                    where = Expression.Not(Expression.Call(left, typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) }), right));
                    break;
                case "eq":
                    where = Expression.Equal(left, right);
                    break;
                case "ne":
                    where = Expression.Not(Expression.Equal(left, right));
                    break;
                case "lt":
                    where = Expression.LessThan(left, right);
                    break;
                case "le":
                    where = Expression.LessThanOrEqual(left, right);
                    break;
                case "gt":
                    where = Expression.GreaterThan(left, right);
                    break;
                case "ge":
                    where = Expression.GreaterThanOrEqual(left, right);
                    break;
                case "nu":
                    where = Expression.Equal(left,Expression.Constant(typeof(Nullable)));
                    break;
                case "nn":
                    where = Expression.Not(Expression.Equal(left,Expression.Constant(typeof(Nullable))));
                    break;
            }
            
            return where;
        }

    }
}
 
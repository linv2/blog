using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroBlog.Core.Model.DbModel;

namespace MetroBlog.Core.Cache
{
    public class CacheKey
    {
        public const string ArticleItemKey = "Article_Item_{0}";
        public const string ArticleKey = "Article";

        public const string CategoryItemAliasKey = "CategoryAlias_Item_{0}";
        public const string CategoryItemKey = "Category_Item_{0}";
        public const string CategoryKey = "Category";


        public const string MenuKey = "MenuKey";
        public const string SettingKey = "SettingKey";
    }

    public static class CacheManage
    {
        #region Article
        private static string GetArticleCacheKey(this Article article)
        {
            return GetArticleCacheKey(article.Id);
        }

        public static string GetArticleCacheKey(int articleId)
        {
            return string.Format(CacheKey.ArticleItemKey, articleId);
        }

        public static void SaveToCache(this Article article)
        {
            Blog.Cache.Save(article.GetArticleCacheKey(), article);
        }

        public static Model.ViewModel.Article GetArticle(int articleId)
        {
            return Blog.Cache.Get<Model.ViewModel.Article>(GetArticleCacheKey(articleId));
        }
        #endregion Article

        #region Category
        private static string GetCategoryCacheKey(this Category category)
        {
            return GetCategoryCacheKey(category.Id);
        }

        public static string GetCategoryCacheKey(int categoryId)
        {
            return string.Format(CacheKey.CategoryItemKey, categoryId);
        }

        private static string GetCategoryAliasCacheKey(this Category category)
        {
            return GetCategoryAliasCacheKey(category.Alias);
        }

        public static string GetCategoryAliasCacheKey(string alias)
        {
            return string.Format(CacheKey.CategoryItemAliasKey, alias);
        }
        public static void SaveToCache(this Category category)
        {
            Blog.Cache.Save(category.GetCategoryCacheKey(), category);
            Blog.Cache.Save(category.GetCategoryAliasCacheKey(), category);
        }

        public static Model.ViewModel.Category GetCategory(int categoryId)
        {
            return Blog.Cache.Get<Model.ViewModel.Category>(GetCategoryCacheKey(categoryId));
        }
        public static Model.ViewModel.Category GetCategory(string alias)
        {
            return Blog.Cache.Get<Model.ViewModel.Category>(GetCategoryAliasCacheKey(alias));
        }
        public static IList<Model.ViewModel.Category> GetCategoryList()
        {
            return Blog.Cache.Get<IList<Model.ViewModel.Category>>(CacheKey.CategoryKey);
        }

        public static void SaveToCache(this IList<Model.ViewModel.Category> categoryList)
        {
            Blog.Cache.Save(CacheKey.CategoryKey, categoryList);
        }

        public static void RemoveCategory()
        {
            Blog.Cache.Remove(CacheKey.CategoryKey);
        }
        #endregion Category


        #region 菜单
        public static void SaveToCache(this IList<Model.ViewModel.Menu> menus)
        {
            Blog.Cache.Save(CacheKey.MenuKey, menus);
        }

        public static IList<Model.ViewModel.Menu> GetMenu()
        {
            return Blog.Cache.Get<IList<Model.ViewModel.Menu>>(CacheKey.MenuKey);
        }

        public static void RemoveMenu()
        {
            Blog.Cache.Remove(CacheKey.MenuKey);
        }
        #endregion 菜单

        #region 设置
        public static void SaveToCache(this Model.ViewModel.Setting setting)
        {
            Blog.Cache.Save(CacheKey.SettingKey, setting);
        }

        public static Model.ViewModel.Setting GetSetting()
        {
            return Blog.Cache.Get<Model.ViewModel.Setting>(CacheKey.SettingKey);
        }

        public static void RemoveSetting()
        {
            Blog.Cache.Remove(CacheKey.SettingKey);
        }
        #endregion 设置
    }
}
